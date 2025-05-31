using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkConnectUI : MonoBehaviour
{
    public void StartHost() => BeginHost();
    public void StartClient() => NetworkManager.Singleton.StartClient();

    private void BeginHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Yunhai Port", LoadSceneMode.Single);
    }

}