using Riptide;
using Riptide.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ServerToClientID : ushort
{
    sync,
    playerSpawned = 1,
    playerMovement,
    serverEvent,
    weaponState,
    meleeAttack,
    gunAngle,
    gunAttack,
    spawnBullet,
    rangedEvent,
    enemySpawned,
    enemyDied,
    enemyMovement,
    abilityEvent,
    destroyIndicators,
    droppedItems,
    pickUpMelee,
}

public enum ClientToServerID : ushort
{
    name = 1,
    input,
    enemySpawned,
    weaponState,
    meleeAttack,
    gunAttack,
    gunAngle,
}

public class NetworkManager : MonoBehaviour
{
    [SerializeField] Button connectButton;
    [SerializeField] GameObject serverAddressInput;
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] Canvas inGameCanvas;
    [SerializeField] Camera mainCamera;

    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
                if (_singleton != value)
                {
                    Destroy(value);
                }
            }
        }
    }

    public Client Client { get; private set; }
    private ushort _serverTick;
    public ushort ServerTick
    {
        get => _serverTick;
        private set
        {
            _serverTick = value;
            InterpolationTick = (ushort)(value - TicksBetweenPositionUpdates);
        }
    }
    public ushort InterpolationTick { get; private set; }
    private ushort _ticksBetweenPositionUpdates = 2;
    public ushort TicksBetweenPositionUpdates
    {
        get => _ticksBetweenPositionUpdates;
        private set
        {
            _ticksBetweenPositionUpdates = value;
            InterpolationTick = (ushort)(ServerTick - value);
        }
    }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;
    [SerializeField] private string username;
    [Space(10)]
    [SerializeField] private ushort tickDivergenceTolerence = 1;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {   
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);

        Client = new Client();

        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;

        ServerTick = 2;
    }

    public void ConnectToServer()
    {
        username = usernameInput.text;
        Client.Connect(serverAddressInput.GetComponent<TMP_InputField>().text, 8);
    }

    private void DidConnect(object sender, EventArgs e)
    {
        SendName();
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerID.enemySpawned);
        Client.Send(message);
        ActivateUI(false);
    }

    private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
    {
        ActivateUI(true);
        OnDisconnect();
    }

    private void DidDisconnect(object sender, DisconnectedEventArgs e)
    {
        OnDisconnect();
    }

    private void OnDisconnect()
    {
        for(ushort i = 1; i <= Player.list.Count; i++)
        {
            if(Player.list.TryGetValue(i, out Player player))
            {
                Destroy(player.gameObject);
            }
            ActivateUI(true);
        }
        foreach(GameObject obj in DestroyOnDisconnect.list)
        {
            Destroy(obj);
        }
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    private void ActivateUI(bool value)
    {
        serverAddressInput.SetActive(value);

        connectButton.gameObject.SetActive(value);
        usernameInput.gameObject.SetActive(value);

        mainCamera.gameObject.SetActive(value);

        inGameCanvas.gameObject.SetActive(!value);
    }

    private void FixedUpdate()
    {
        Client.Update();

        if(ServerTick % 6498 == 0)
        {
            ServerTick = 0;
        }

        ServerTick++;
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerID.name);
        message.AddString(username);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SpawnEnemy()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerID.enemySpawned);
        NetworkManager.Singleton.Client.Send(message);
    }

    private void SetTick(ushort serverTick)
    {
        if(Mathf.Abs(ServerTick - serverTick) > tickDivergenceTolerence)
        {
            Debug.Log($"Client Tick: {ServerTick} -> {serverTick}");
            ServerTick = serverTick;
        }
    }

    [MessageHandler((ushort)ServerToClientID.sync)]
    private static void Sync(Message message)
    {
        Singleton.SetTick(message.GetUShort());
    }
}
