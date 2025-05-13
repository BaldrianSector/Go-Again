using UnityEngine;

public class newMover : MonoBehaviour
{
    [Header("References")]
    public GameObject player;           // Reference to the player
    public GameObject mover;            // Prefab to mover

    [Header("Spawn Offset")]
    public Vector3 positionOffset = new Vector3(0, 1.625f, 0); // Offset to apply to spawn position

    void Update()
    {
        // Instantiate new mover prefab at player position + offset
        if (Input.GetKeyDown(KeyCode.N) && player != null && mover != null)
        {
            Vector3 spawnPos = player.transform.position + positionOffset;
            Instantiate(mover, spawnPos, Quaternion.identity);
            Debug.Log("Spawned new mover at: " + spawnPos);
        }
    }
}
