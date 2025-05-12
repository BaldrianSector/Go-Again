using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    public GameObject playerCubePrefab; // Assign in Inspector

    private Collider playerCollider;

    void Awake()
    {
        playerCollider = GetComponent<Collider>();
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

        // Capture velocity from CharacterController
        CharacterController cc = GetComponent<CharacterController>();
        Vector3 deathVelocity = cc != null ? cc.velocity : Vector3.zero;

        // Check if the player is inside a collider tagged "NoRespawn"
        Collider[] overlaps = Physics.OverlapSphere(deathPosition, 0.1f);
        foreach (Collider col in overlaps)
        {
            if (col.CompareTag("NoRespawn"))
            {
                Debug.Log("Death inside NoRespawn zone – ghost will not be spawned.");
                SafeTeleportToStart();
                return;
            }
        }

        // Offset the ghost's position slightly above the ground
        Vector3 offsetDeathPosition = deathPosition + new Vector3(0, 0.48f, 0);

        // Spawn ghost
        GameObject ghost = Instantiate(playerCubePrefab, offsetDeathPosition, deathRotation);
        Collider ghostCollider = ghost.GetComponent<Collider>();
        if (ghostCollider != null) ghostCollider.enabled = false;

        // Apply velocity to ghost's Rigidbody if present
        Rigidbody ghostRb = ghost.GetComponent<Rigidbody>();
        if (ghostRb != null)
        {
            ghostRb.linearVelocity = deathVelocity;
        }

        SafeTeleportToStart();

        if (ghostCollider != null) ghostCollider.enabled = true;
    }

    void SafeTeleportToStart()
    {
        // Disable collider to avoid unwanted collision during teleport
        if (playerCollider != null) playerCollider.enabled = false;

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

        // Re-enable collider
        if (playerCollider != null) playerCollider.enabled = true;
    }
}
