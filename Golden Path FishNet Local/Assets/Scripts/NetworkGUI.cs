using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGUI : MonoBehaviour
{
    void OnGUI()
    {
        if (GUILayout.Button("Host"))
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        }
        if (GUILayout.Button("Client"))
        {
            InstanceFinder.ClientManager.StartConnection();
        }
        if (GUILayout.Button("Server"))
        {
            InstanceFinder.ServerManager.StartConnection();
        }
    }
}
