using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    const string GameTypeName = "MONSTERLOVER";

    // dev --
    // local address
    const string LocalServerIP = "127.0.0.1";
    const int ServerPort = 25003;

    string playerName;
    string gameServerName;

    // @override
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
        playerName = "Player" + Random.Range(0, 99999999).ToString();
        gameServerName = "Server" + Random.Range(0, 99999999).ToString();
        UpdateHostList();
	}
	
    public enum Status
    {
        NoError,

        LaunchingServer, // process
        ServerLaunched, // complete
        LaunchServerFailed, // fail

        ConnectingToServer, // access process
        ConnectedToServer, // complete access
        ConnectToServerFailed, // access fail

        DisconnectedFromServer,
    };

    Status _status = Status.NoError;
    public Status status { get { return _status; } private set { _status = value; } }

    public void LaunchServer(string roomName)
    {
        status = Status.LaunchingServer;
        StartCoroutine(LaunchServerCoroutine(gameServerName));
    }
	
    // hole punching
    bool useNat = false;
    IEnumerator CheckNat()
    {
        bool doneTesting = false;
        bool probingPublicIP = false;
        float timer = 0;
        useNat = false;

        // ip valid test
        while (!doneTesting)
        {
            ConnectionTesterStatus connectionTestResult = Network.TestConnection();
            switch (connectionTestResult)
            {
                case ConnectionTesterStatus.Error:
                    doneTesting = true;
                    break;
                case ConnectionTesterStatus.Undetermined: // still check
                    doneTesting = false;
                    break;
                case ConnectionTesterStatus.PublicIPIsConnectable: // public ip ok
                    useNat = false;
                    doneTesting = true;
                    break;
                case ConnectionTesterStatus.PublicIPPortBlocked: // ip ok but port is blocked
                    useNat = false;
                    if (!probingPublicIP)
                    {
                        connectionTestResult = Network.TestConnectionNAT();
                        probingPublicIP = true;
                        timer = Time.time + 10;
                    }
                    else if (Time.time > timer)
                    {
                        // reset
                        probingPublicIP = false;
                        useNat = true;
                        doneTesting = true;
                    }
                    break;
                case ConnectionTesterStatus.PublicIPNoServerStarted:
                    break;
                case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
                case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
                    // hole punching is limited. maybe access fail
                    useNat = true;
                    doneTesting = true;
                    break;
                case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
                case ConnectionTesterStatus.NATpunchthroughFullCone:
                    // all ok
                    useNat = true;
                    doneTesting = true;
                    break;
                default:
                    Debug.Log("Error in test routine, got " + connectionTestResult);
                    break;
            }
            yield return null;
        }
    }

    IEnumerator LaunchServerCoroutine(string roomName)
    {
        yield return StartCoroutine(CheckNat());

        NetworkConnectionError error = Network.InitializeServer(32, ServerPort, useNat);
        if(error != NetworkConnectionError.NoError)
        {
            Debug.Log("Can't Launch Server");
            status = Status.LaunchServerFailed;
        }
        else
        {
            // relay server entry
            MasterServer.RegisterHost(GameTypeName, gameServerName);
        }
    }

    public void ConnectToServer(string serverGuid, bool connectLocalServer) // guid = surrogate
    {
        status = Status.ConnectingToServer;
        if (connectLocalServer)
        {
            Network.Connect(LocalServerIP, ServerPort);
        }
        else
        {
            Network.Connect(serverGuid);
        }
    }

    void OnServerInitialized()
    {
        status = Status.ServerLaunched;
    }
    
    void OnConnectedToServer()
    {
        status = Status.ConnectedToServer;
    }

    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("FailedToConnect: " + error.ToString());
        status = Status.ConnectToServerFailed;
    }

    // client disconnect / server code
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    // server disconnect
    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        Debug.Log("DisconnectedFromServer: " + info.ToString());
        status = Status.DisconnectedFromServer;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public Status GetStatus()
    {
        return status;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    // @override
    void OnDestroy()
    {
        if (Network.isServer)
        {
            MasterServer.UnregisterHost();
            Network.Disconnect();
        }
    }

    // --------- entrance part method ------------

    // relay server game list refresh
    public void UpdateHostList()
    {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList(GameTypeName);
    }

    // game list pick up
    public HostData[] GetHostList()
    {
        return MasterServer.PollHostList();
    }

    // relay server & nat IP set
    void SetMasterServerAndNatFacilitatorIP(string masterServerAddress, string facilitatorAddress)
    {
        MasterServer.ipAddress = masterServerAddress;
        Network.natFacilitatorIP = facilitatorAddress;
    }

    // relay server delete
    public void UnregisterHost()
    {
        MasterServer.UnregisterHost();
    }

    // ------------- GUI ------------------------

    //@override
    void OnGUI()
    {
        if((Network.isServer || Network.isClient))
        {
            return;
        }

        float scale = Screen.height / 480.0f;
        GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0),
            Quaternion.identity, new Vector3(scale, scale, 1.0f));

        GUI.Window(0, new Rect(-200, -200, 400, 400), NetworkSettingWindow, "Network Setting");
    }

    Vector2 scrollPosition;

    void NetworkSettingWindow(int windowID)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Player Name: ");
        playerName = GUILayout.TextField(playerName, 32);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Game Server Name: ");
        gameServerName = GUILayout.TextField(gameServerName, 32);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Launch"))
        {
            LaunchServer(gameServerName);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Game Server List (Click To Connect): ");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Refresh"))
        {
            UpdateHostList();
        }
        GUILayout.EndHorizontal();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(380), GUILayout.Height(200));

        HostData[] hosts = GetHostList();
        if(hosts.Length > 0)
        {
            foreach(HostData host in hosts)
            {
                if(GUILayout.Button(host.gameName, GUI.skin.box, GUILayout.Width(360)))
                {
                    ConnectToServer(host.guid, false);
                }
            }
        }
        else
        {
            GUILayout.Label("No Server");
        }
        GUILayout.EndScrollView();

        if (GUILayout.Button("Connect Local Server"))
        {
            ConnectToServer("", true);
        }

        GUILayout.Label("Status: " + status.ToString());
    }
}