using UnityEngine;
using System.Collections;

public interface INetwork {

    void CreateServer(string serverCode);
    void ConnectToServer(string ipAddress);
    void Disconnect();

    void OnQuit();
    void OnServerInitialized();
    void OnConnectedToServer();
    void KickPlayer(int playerIndexNr);
    void OnDisconnectedFromServer(NetworkDisconnection info);
    void OnPlayerDisconnected(NetworkPlayer player);
    void OnFailedToConnect(NetworkConnectionError error);
    void OnDestroyEvent();
}
