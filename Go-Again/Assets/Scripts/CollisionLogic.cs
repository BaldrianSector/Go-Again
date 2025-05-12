using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    public GameObject playerCubePrefab; // Assign in Inspector

    private Collider playerCollider;
    private Rigidbody rb;

    void Awake()
    {
        playerCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key pressed – triggering death.");
            TriggerDeath();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed – attempting to teleport to start.");
            SafeTeleportToStart();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            Debug.Log("Collided with Lava – triggering death.");
            TriggerDeath();
        }
    }

    void TriggerDeath()
    {
        Vector3 deathPosition = transform.position;
        Quaternion deathRotation = transform.rotation;

        Vector3 offsetDeathPosition = deathPosition + new Vector3(0, 0.48f, 0);
        GameObject ghost = Instantiate(playerCubePrefab, offsetDeathPosition, deathRotation);
        Collider ghostCollider = ghost.GetComponent<Collider>();
        if (ghostCollider != null) ghostCollider.enabled = false;

        SafeTeleportToStart();

        if (ghostCollider != null) ghostCollider.enabled = true;
    }

    void SafeTeleportToStart()
    {
        // Disable physics and collisions
        if (playerCollider != null) playerCollider.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        GameObject respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        if (respawnPoint != null)
        {
            transform.SetPositionAndRotation(
                respawnPoint.transform.position,
                respawnPoint.transform.rotation
            );
            Debug.Log("Player teleported to respawn.");
        }
        else
        {
            Debug.LogWarning("No object with tag 'Respawn' found in the scene!");
        }

        // Re-enable physics and collisions
        if (playerCollider != null) playerCollider.enabled = true;
        if (rb != null) rb.isKinematic = false;
    }
}
