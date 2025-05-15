using UnityEngine;

public class DestroyOnOOB : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        Debug.Log($"Trigger entered by: {other.gameObject.name}");

        // Walk up the hierarchy to check for a parent with the "OOB" tag
        Transform current = other.transform;
        while (current != null)
        {
            if (current.CompareTag("OOB"))
            {
                Debug.Log($"{gameObject.name} entered OOB trigger zone — destroying.");

                hasTriggered = true; // Ensure it only happens once
                GameManager.instance.IncreaseLife();

                Destroy(gameObject);
                return;
            }

            current = current.parent;
        }
    }
}
