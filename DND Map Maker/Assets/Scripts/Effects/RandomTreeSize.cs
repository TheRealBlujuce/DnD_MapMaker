using UnityEngine;

public class RandomTreeSize : MonoBehaviour
{
    public float randomTreeSizeMin = 0.75f;
    public float randomTreeSizeMax = 1f;
    public float currentTreeSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTreeSize = Random.Range(randomTreeSizeMin, randomTreeSizeMax);
        currentTreeSize = Mathf.Clamp(currentTreeSize, randomTreeSizeMin, randomTreeSizeMax);

        transform.localScale = new Vector3(currentTreeSize, currentTreeSize, currentTreeSize);
    }


}
