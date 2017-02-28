using UnityEngine;
using LostPolygon.AndroidBluetoothMultiplayer;
using LostPolygon.AndroidBluetoothMultiplayer.Examples;
using System;

/*
#if UNITY_ANDROID
public class BluetoothManager : INetwork {

    private static BluetoothMultiplayerMode _desiredMode = BluetoothMultiplayerMode.None;
    private bool _initResult;

    public static string deviceName;

    public BluetoothManager() {
        try {
            // Setting the UUID. Must be unique for every application
            _initResult = AndroidBluetoothMultiplayer.Initialize("8ce255c0-200a-11e0-ac64-0800200c9a66");

            // Enabling verbose logging. See log cat!
            AndroidBluetoothMultiplayer.SetVerboseLog(true);
            InitializeBluetooth();
            deviceName = AndroidBluetoothMultiplayer.GetCurrentDevice().Name;
            Debug.Log("From BT constructor, Devicename: " + deviceName);
        } catch(Exception ex) {
            Debug.Log("Error constructing Bluetooth: " + ex);
            PopupManager.ShowErrorPopup("Error", "Failed to start Bluetooth, please try again or use wifi.");
            SceneManager.LoadLevel(SceneManager.Menu);
        }
    }

    public void CreateServer() {
        Debug.Log("creating server");
        // If Bluetooth is enabled, then we can do something right on
        if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {
            AndroidBluetoothMultiplayer.RequestEnableDiscoverability(120);
            Network.Disconnect(); // Just to be sure
            AndroidBluetoothMultiplayer.StartServer(NetworkManager.ConnectionPort);
        }
        else {
            // Otherwise we have to enable Bluetooth first and wait for callback
            _desiredMode = BluetoothMultiplayerMode.Server;
            AndroidBluetoothMultiplayer.RequestEnableDiscoverability(120);
        }
    }

    public void ConnectToServer(string ipAddress) {
        // If Bluetooth is enabled, then we can do something right on
        if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {
            Network.Disconnect(); // Just to be sure
            AndroidBluetoothMultiplayer.ShowDeviceList(); // Open device picker dialog
        }
        else {
            // Otherwise we have to enable Bluetooth first and wait for callback
            _desiredMode = BluetoothMultiplayerMode.Client;
            AndroidBluetoothMultiplayer.RequestEnableBluetooth();
        }
    }

    private void InitializeBluetooth() {
        // Registering the event delegates
        AndroidBluetoothMultiplayer.ListeningStarted += OnBluetoothListeningStarted;
        AndroidBluetoothMultiplayer.ListeningStopped += OnBluetoothListeningStopped;
        AndroidBluetoothMultiplayer.AdapterEnabled += OnBluetoothAdapterEnabled;
        AndroidBluetoothMultiplayer.AdapterEnableFailed += OnBluetoothAdapterEnableFailed;
        AndroidBluetoothMultiplayer.AdapterDisabled += OnBluetoothAdapterDisabled;
        AndroidBluetoothMultiplayer.DiscoverabilityEnabled += OnBluetoothDiscoverabilityEnabled;
        AndroidBluetoothMultiplayer.DiscoverabilityEnableFailed += OnBluetoothDiscoverabilityEnableFailed;
        AndroidBluetoothMultiplayer.ConnectedToServer += OnBluetoothConnectedToServer;
        AndroidBluetoothMultiplayer.ConnectionToServerFailed += OnBluetoothConnectionToServerFailed;
        AndroidBluetoothMultiplayer.DisconnectedFromServer += OnBluetoothDisconnectedFromServer;
        AndroidBluetoothMultiplayer.ClientConnected += OnBluetoothClientConnected;
        AndroidBluetoothMultiplayer.ClientDisconnected += OnBluetoothClientDisconnected;
        AndroidBluetoothMultiplayer.DevicePicked += OnBluetoothDevicePicked;
    }

    private void DeinitializeBluetooth() {
        AndroidBluetoothMultiplayer.ListeningStarted -= OnBluetoothListeningStarted;
        AndroidBluetoothMultiplayer.ListeningStopped -= OnBluetoothListeningStopped;
        AndroidBluetoothMultiplayer.AdapterEnabled -= OnBluetoothAdapterEnabled;
        AndroidBluetoothMultiplayer.AdapterEnableFailed -= OnBluetoothAdapterEnableFailed;
        AndroidBluetoothMultiplayer.AdapterDisabled -= OnBluetoothAdapterDisabled;
        AndroidBluetoothMultiplayer.DiscoverabilityEnabled -= OnBluetoothDiscoverabilityEnabled;
        AndroidBluetoothMultiplayer.DiscoverabilityEnableFailed -= OnBluetoothDiscoverabilityEnableFailed;
        AndroidBluetoothMultiplayer.ConnectedToServer -= OnBluetoothConnectedToServer;
        AndroidBluetoothMultiplayer.ConnectionToServerFailed -= OnBluetoothConnectionToServerFailed;
        AndroidBluetoothMultiplayer.DisconnectedFromServer -= OnBluetoothDisconnectedFromServer;
        AndroidBluetoothMultiplayer.ClientConnected -= OnBluetoothClientConnected;
        AndroidBluetoothMultiplayer.ClientDisconnected -= OnBluetoothClientDisconnected;
        AndroidBluetoothMultiplayer.DevicePicked -= OnBluetoothDevicePicked;
    }

    public void Disconnect() {
        Network.Disconnect();
    }

    public void OnQuit() {
        Network.Disconnect();

        try {
            AndroidBluetoothMultiplayer.StopDiscovery();
            AndroidBluetoothMultiplayer.Stop();
        }
        catch {
            Debug.Log("Something went wrong stopping Bluetooth");
        }
    }

    public void OnDestroyEvent() {
        DeinitializeBluetooth();
    }

#region Bluetooth events

    private void OnBluetoothListeningStarted() {
        Debug.Log("Event - ListeningStarted");

        // Starting Unity networking server if Bluetooth listening started successfully
        Network.InitializeServer(4, NetworkManager.ConnectionPort, false);
    }

    private void OnBluetoothListeningStopped() {
        Debug.Log("Event - ListeningStopped");

        // For demo simplicity, stop server if listening was canceled
        AndroidBluetoothMultiplayer.Stop();
    }

    private void OnBluetoothDevicePicked(BluetoothDevice device) {
        Debug.Log("Event - DevicePicked: " + BluetoothExamplesTools.FormatDevice(device));

        // Trying to connect to a device user had picked
        AndroidBluetoothMultiplayer.Connect(device.Address, NetworkManager.ConnectionPort);
    }

    private void OnBluetoothClientDisconnected(BluetoothDevice device) {
        Debug.Log("Event - ClientDisconnected: " + BluetoothExamplesTools.FormatDevice(device));
    }

    private void OnBluetoothClientConnected(BluetoothDevice device) {
        Debug.Log("Event - ClientConnected: " + BluetoothExamplesTools.FormatDevice(device));
    }

    private void OnBluetoothDisconnectedFromServer(BluetoothDevice device) {
        Debug.Log("Event - DisconnectedFromServer: " + BluetoothExamplesTools.FormatDevice(device));
        Disconnect();
    }

    private void OnBluetoothConnectionToServerFailed(BluetoothDevice device) {
        Debug.Log("Event - ConnectionToServerFailed: " + BluetoothExamplesTools.FormatDevice(device));
        Disconnect();
        PopupManager.ShowErrorPopup("Connection Error", "Unable to connect to chosen device.");
        SceneManager.LoadLevel(SceneManager.Menu);
    }

    private void OnBluetoothConnectedToServer(BluetoothDevice device) {
        Debug.Log("Event - ConnectedToServer: " + BluetoothExamplesTools.FormatDevice(device));

        // Trying to negotiate a Unity networking connection, 
        // when Bluetooth client connected successfully
        Network.Connect(NetworkManager.ConnectionIp, NetworkManager.ConnectionPort);
    }

    private void OnBluetoothAdapterDisabled() {
        Debug.Log("Event - AdapterDisabled");
    }

    private void OnBluetoothAdapterEnableFailed() {
        Debug.Log("Event - AdapterEnableFailed");
    }

    private void OnBluetoothAdapterEnabled() {
        Debug.Log("Event - AdapterEnabled");

        // Resuming desired action after enabling the adapter
        switch (_desiredMode) {
            case BluetoothMultiplayerMode.Server:
                Network.Disconnect();
                AndroidBluetoothMultiplayer.StartServer(NetworkManager.ConnectionPort);
                break;
            case BluetoothMultiplayerMode.Client:
                Network.Disconnect();
                AndroidBluetoothMultiplayer.ShowDeviceList();
                break;
        }

        _desiredMode = BluetoothMultiplayerMode.None;
    }

    private void OnBluetoothDiscoverabilityEnableFailed() {
        Debug.Log("Event - DiscoverabilityEnableFailed");
        PopupManager.ShowErrorPopup("Error", "Enable Bluetooth to connect.");
        SceneManager.LoadLevel(SceneManager.Menu);
    }

    private void OnBluetoothDiscoverabilityEnabled(int discoverabilityDuration) {
        Debug.Log(string.Format("Event - DiscoverabilityEnabled: {0} seconds", discoverabilityDuration));
    }

#endregion Bluetooth events

#region Network events
    public void OnPlayerDisconnected(NetworkPlayer player) {
        Debug.Log("Player disconnected: " + player.GetHashCode());
    }

    public void OnFailedToConnect(NetworkConnectionError error) {
        Debug.Log("Can't connect to the networking server");

        // Stopping all Bluetooth connectivity on Unity networking disconnect event
        AndroidBluetoothMultiplayer.Stop();
    }

    public void OnDisconnectedFromServer(NetworkDisconnection info) {
        Debug.Log("Disconnected from server");

        // Stopping all Bluetooth connectivity on Unity networking disconnect event
        AndroidBluetoothMultiplayer.Stop();
    }

    public void OnConnectedToServer() {
        Debug.Log("Connected to server");
    }

    public void OnServerInitialized() {
        Debug.Log("Server initialized");
    }

    #endregion Network events
}
#endif
*/