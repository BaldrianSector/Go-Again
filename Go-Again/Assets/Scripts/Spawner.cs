using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject player; // Reference to the player object

    [Header("Movement Settings")]
    public Vector3 positionOffset = new Vector3(0, 1.625f, 0); // Optional offset (e.g. for height)

    void Start()
    {
        AudioManager.Instance.Play("portalAmbient");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && player != null)
        {
            transform.position = player.transform.position + positionOffset;
            Debug.Log("Spawner moved to: " + transform.position);
        }
    }
}
