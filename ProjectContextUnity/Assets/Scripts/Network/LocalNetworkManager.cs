using UnityEngine;
using System.Collections;
using System;

public class LocalNetworkManager : INetwork {

    public LocalNetworkManager() { }

    public void CreateServer(string serverCode) {
        Debug.Log("Hosting server with code: " + serverCode);
        //Network.InitializeServer(2, NetworkManager.ConnectionPort, !Network.HavePublicAddress());
        UDPServerDiscovery.HostServer(serverCode);
    }

    public void ConnectToServer(string ipAddress) {
        Debug.Log("attempting to join server at: " + ipAddress);
        Network.Connect(ipAddress, NetworkManager.ConnectionPort);
    }

    public void Disconnect() {
        Debug.Log("Disconnecting from server");
        Network.Disconnect(200);
    }

    public void OnQuit() {
        if(Network.isServer)
            UDPServerDiscovery.StopHostingServer();
        else 
            Disconnect();
    }

    public void OnDestroyEvent() { }

    #region Network Events
    public void OnServerInitialized() {
        Debug.Log("Server initialized");
    }

    public void OnFailedToConnect(NetworkConnectionError error) {
        Debug.Log("Failed to connect to server: " + error);
    }

    public void OnConnectedToServer() {
        Debug.Log("Connected to server");
    }

    public void OnDisconnectedFromServer(NetworkDisconnection info) {
        if (Network.isServer) {
            Debug.Log("Local server connection disconnected as host");
            if(info == NetworkDisconnection.LostConnection)
                PopupManager.Instance.ShowPopup("Error", "Connection lost, please try again");
        } else {
            if (info == NetworkDisconnection.LostConnection) {
                Debug.Log("Lost connection to the server");
            } else {
                Debug.Log("Successfully diconnected from the server");
            }
        }
    }

    public void OnPlayerDisconnected(NetworkPlayer player) {
        Debug.Log("Clean up after player " + player);
    }

    public void KickPlayer(int playerIndexNr) {
        if (!Network.isServer) {
            Debug.LogWarning("Cant kick player because you're not the host");
            return;
        }

        if(Network.connections.Length == 0) {
            Debug.LogWarning("No players found on server!");
            return;
        }

        Network.CloseConnection(Network.connections[playerIndexNr], true);
    }
    #endregion
}
