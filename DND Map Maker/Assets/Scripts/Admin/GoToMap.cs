using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMap : MonoBehaviour
{

    public string levelName = "";
	private InitiativeUIManager initiativeManager;
    
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
		initiativeManager = FindFirstObjectByType<InitiativeUIManager>();	
    }
	
	public void SwitchToMap()
    {
		initiativeManager.ClearAndRefreshInitiative();
        NetworkManager.Singleton.SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

}
