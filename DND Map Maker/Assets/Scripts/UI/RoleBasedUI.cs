using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleBasedUI : NetworkBehaviour
{
    public GameObject adminUI;
    public GameObject clientUI;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        clientUI = GameObject.Find("ClientUI");

        if (IsHost)
        {
            adminUI.SetActive(true);
            clientUI.SetActive(false);
        }
        else if (IsClient)
        {
            if (clientUI != null)
            {
                adminUI.SetActive(false);
                clientUI.SetActive(true);
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            adminUI.SetActive(true);
            if (clientUI != null)
            {
                clientUI.SetActive(false);
            }
        }
        else if (IsClient)
        {
            if (clientUI != null)
            {
                adminUI.SetActive(false);
                clientUI.SetActive(true);
            }
        }
    }
}
