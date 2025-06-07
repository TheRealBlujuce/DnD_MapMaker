using UnityEngine;

public class TreeHandler : MonoBehaviour
{

	private ParticleSystem leafSystem;
	private Transform leafPartTransform;
	[SerializeField] private float currentWindDirection;
	[SerializeField] private bool canSpawnLeaves = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leafSystem = GetComponentInChildren<ParticleSystem>();
		leafPartTransform = leafSystem.transform;
		currentWindDirection = GameManager.Instance.currentWorldWindDirection;
		leafPartTransform.localRotation = Quaternion.Euler(currentWindDirection, 90f, 90f);
    }

    // Update is called once per frame
    void Update()
    {
		currentWindDirection = GameManager.Instance.currentWorldWindDirection;
        leafPartTransform.localRotation = Quaternion.Euler(currentWindDirection, 90f, 90f);
    }

	public void SetLeafPartSystemState(bool enabled)
	{
		canSpawnLeaves = enabled;
		UpdateLeafPartSystemState();
	}

	private void UpdateLeafPartSystemState()
	{
		switch(canSpawnLeaves)
		{
			case true:
				leafSystem.Play();
			break;
			case false:
				leafSystem.Stop();
			break;
		}
	}

}
