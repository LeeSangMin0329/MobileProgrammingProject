using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSynchronizer : MonoBehaviour {

    enum BitField
    {
        BasicAttack1,
        BasicAttack2,
        BasicAttack3,
        Tumbling,
        Died,
        KnockDown,
        Hit,
        Skill111,
        Skill123,
        Skill121,
        SkillOn,
    };

    NetworkView netView;

    Vector3 position;
    Quaternion rotation;

    CharacterStatus status;

	// Use this for initialization
	void Start () {
        position = transform.position;
        rotation = transform.rotation;
        status = GetComponent<CharacterStatus>();
        netView = GetComponent<NetworkView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!netView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 5.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);
        }
	}

    //@override
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            // send
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            if(status != null)
            {
                int hp = status.HP;
                float stamina = status.Stamina;
                int packedFlags = PackStatusFlags();
                stream.Serialize(ref hp);
                stream.Serialize(ref stamina);
                stream.Serialize(ref packedFlags);
            }
        }
        else
        {
            // receive
            stream.Serialize(ref position);
            stream.Serialize(ref rotation);
            if(status != null)
            {
                int hp = 0;
                float stamina = 0;
                int flags = 0;
                stream.Serialize(ref hp);
                stream.Serialize(ref stamina);
                stream.Serialize(ref flags);
                status.HP = hp;
                status.Stamina = stamina;
                UnpackStatusBit(flags);
            }
        }
    }

    int PackStatusFlags()
    {
        int packed = 0;
        packed |= status.basicAttack1 ? (1 << (int)BitField.BasicAttack1) : 0;
        packed |= status.basicAttack2 ? (1 << (int)BitField.BasicAttack2) : 0;
        packed |= status.basicAttack3 ? (1 << (int)BitField.BasicAttack3) : 0;
        packed |= status.tumbling ? (1 << (int)BitField.Tumbling) : 0;
        packed |= status.died ? (1 << (int)BitField.Died) : 0;
        packed |= status.knockDown ? (1 << (int)BitField.KnockDown) : 0;
        packed |= status.hit ? (1 << (int)BitField.Hit) : 0;
        packed |= status.skill111 ? (1 << (int)BitField.Skill111) : 0;
        packed |= status.skill123 ? (1 << (int)BitField.Skill123) : 0;
        packed |= status.skill121 ? (1 << (int)BitField.Skill121) : 0;
        packed |= status.skillOn ? (1 << (int)BitField.SkillOn) : 0;

        return packed;
    }
    void UnpackStatusBit(int bit)
    {
        status.basicAttack1 = (bit & (1 << (int)BitField.BasicAttack1)) != 0;
        status.basicAttack2 = (bit & (1 << (int)BitField.BasicAttack2)) != 0;
        status.basicAttack3 = (bit & (1 << (int)BitField.BasicAttack3)) != 0;
        status.tumbling = (bit & (1 << (int)BitField.Tumbling)) != 0;
        status.died = (bit & (1 << (int)BitField.Died)) != 0;
        status.knockDown = (bit & (1 << (int)BitField.KnockDown)) != 0;
        status.hit = (bit & (1 << (int)BitField.Hit)) != 0;
        status.skill111 = (bit & (1 << (int)BitField.Skill111)) != 0;
        status.skill123 = (bit & (1 << (int)BitField.Skill123)) != 0;
        status.skill121 = (bit & (1 << (int)BitField.Skill121)) != 0;
        status.skillOn = (bit & (1 << (int)BitField.SkillOn)) != 0;
    }
}
