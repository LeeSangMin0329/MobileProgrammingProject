using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusGui : MonoBehaviour {

    float baseWidth = 854f;
    float baseHeight = 480f;

    CharacterStatus playerStatus;
    Vector2 playerStatusOffset = new Vector2(8f, 80f);

    Rect nameRect = new Rect(0f, 0f, 90f, 16f);
    public GUIStyle nameLabelStyle;

    public Texture backLifeBarTexture;
    public Texture frontLifeBarTexture;
    float frontLifeBarOffsetX = 2f;
    float lifeBarTextureWidth = 128f;
    Rect playerLifeBarRect = new Rect(0f, 0f, 90f, 10f);
    Color playerFrontLifeBarColor = Color.green;
    Rect enemyLifeBarRect = new Rect(0f, 0f, 128f, 24f);
    Color enemyFrontLifeBarColor = Color.red;

    Rect ownHpBarRect = new Rect(0f, 0f, 256f, 16f);

    void DrawPlayerStatus()
    {
        float x = baseWidth - playerLifeBarRect.width - playerStatusOffset.x;
        float y = playerStatusOffset.y;
        float deltaHeight = nameRect.height + playerLifeBarRect.height;
        PlayerCtrl[] players = FindObjectsOfType<PlayerCtrl>() as PlayerCtrl[];

        foreach(PlayerCtrl player in players)
        {
            CharacterStatus status = player.GetComponent<CharacterStatus>();
            if(status != null)
            {
                DrawCharacterStatus(
                    x, y,
                    status,
                    playerLifeBarRect,
                    playerFrontLifeBarColor);
                y += deltaHeight;
            }
        }

        
        DrawHpBar(
                    playerStatusOffset.x, 0,
                    playerStatus,
                    ownHpBarRect,
                    playerFrontLifeBarColor);
        DrawStaminaBar(
                    playerStatusOffset.x, ownHpBarRect.height,
                    playerStatus,
                    ownHpBarRect,
                    Color.yellow);
    }

    void DrawEnemyStatus()
    {
        if(playerStatus.lastAttackTarget != null)
        {
            TerrorDragonStatus targetStatus = playerStatus.lastAttackTarget.GetComponent<TerrorDragonStatus>();
            DrawMonsterStatus(
                (ownHpBarRect.width + playerStatusOffset.x * 3), 8f,
                targetStatus,
                enemyLifeBarRect,
                enemyFrontLifeBarColor);
        }
    }

    void DrawCharacterStatus(float x, float y, CharacterStatus status, Rect barRect, Color frontColor)
    {
        GUI.Label(
            new Rect(x, y, nameRect.width, nameRect.height),
            status.characterName,
            nameLabelStyle);

        float lifeValue = (float)status.HP / status.MaxHP;
        if(backLifeBarTexture != null)
        {
            // back bar
            y += nameRect.height;
            GUI.DrawTexture(new Rect(x, y, barRect.width, barRect.height), backLifeBarTexture);
        }

        // front bar
        if(frontLifeBarTexture != null)
        {
            float resizeFrontBarOffsetX = frontLifeBarOffsetX * barRect.width / lifeBarTextureWidth;
            float frontBarWidth = barRect.width - resizeFrontBarOffsetX * 2;
            var guiColor = GUI.color;
            GUI.color = frontColor;
            GUI.DrawTexture(new Rect(x + resizeFrontBarOffsetX, y, frontBarWidth * lifeValue, barRect.height), frontLifeBarTexture);
            GUI.color = guiColor;
        }
    }
    void DrawHpBar(float x, float y, CharacterStatus status, Rect barRect, Color frontColor)
    {
        float lifeValue = (float)status.HP / status.MaxHP;
        if (backLifeBarTexture != null)
        {
            // back bar
            y += nameRect.height;
            GUI.DrawTexture(new Rect(x, y, barRect.width, barRect.height), backLifeBarTexture);
        }

        // front bar
        if (frontLifeBarTexture != null)
        {
            float resizeFrontBarOffsetX = frontLifeBarOffsetX * barRect.width / lifeBarTextureWidth;
            float frontBarWidth = barRect.width - resizeFrontBarOffsetX * 2;
            var guiColor = GUI.color;
            GUI.color = frontColor;
            GUI.DrawTexture(new Rect(x + resizeFrontBarOffsetX, y, frontBarWidth * lifeValue, barRect.height), frontLifeBarTexture);
            GUI.color = guiColor;
        }
    }
    void DrawStaminaBar(float x, float y, CharacterStatus status, Rect barRect, Color frontColor)
    {
        float lifeValue = (float)status.Stamina / status.MaxStamina;
        if (backLifeBarTexture != null)
        {
            // back bar
            y += nameRect.height;
            GUI.DrawTexture(new Rect(x, y, barRect.width, barRect.height), backLifeBarTexture);
        }

        // front bar
        if (frontLifeBarTexture != null)
        {
            float resizeFrontBarOffsetX = frontLifeBarOffsetX * barRect.width / lifeBarTextureWidth;
            float frontBarWidth = barRect.width - resizeFrontBarOffsetX * 2;
            var guiColor = GUI.color;
            GUI.color = frontColor;
            GUI.DrawTexture(new Rect(x + resizeFrontBarOffsetX, y, frontBarWidth * lifeValue, barRect.height), frontLifeBarTexture);
            GUI.color = guiColor;
        }
    }

    void DrawMonsterStatus(float x, float y, TerrorDragonStatus status, Rect barRect, Color frontColor)
    {
        GUI.Label(
            new Rect(x, y, nameRect.width, nameRect.height),
            status.enemyName,
            nameLabelStyle);

        float lifeValue = (float)status.HP / status.MaxHP;
        if (backLifeBarTexture != null)
        {
            // back bar
            y += nameRect.height;
            GUI.DrawTexture(new Rect(x, y, barRect.width, barRect.height), backLifeBarTexture);
        }

        // front bar
        if (frontLifeBarTexture != null)
        {
            float resizeFrontBarOffsetX = frontLifeBarOffsetX * barRect.width / lifeBarTextureWidth;
            float frontBarWidth = barRect.width - resizeFrontBarOffsetX * 2;
            var guiColor = GUI.color;
            GUI.color = frontColor;
            GUI.DrawTexture(new Rect(x + resizeFrontBarOffsetX, y, frontBarWidth * lifeValue, barRect.height), frontLifeBarTexture);
            GUI.color = guiColor;
        }

    }

    //@override
    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(
            Vector3.zero,
            Quaternion.identity,
            new Vector3(Screen.width / baseWidth, Screen.height / baseHeight, 1f));

        GameRuleCtrl gameRuleCtal = FindObjectOfType(typeof(GameRuleCtrl)) as GameRuleCtrl;
        if(gameRuleCtal.player != null)
        {
            playerStatus = gameRuleCtal.player.GetComponent<CharacterStatus>();

            DrawPlayerStatus();
            DrawEnemyStatus();
        }
    }
}
