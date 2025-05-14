using UnityEngine;

public class DestroyOnOOB : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name}");

        // Walk up the hierarchy to check for a parent with the "OOB" tag
        Transform current = other.transform;
        while (current != null)
        {
            if (current.CompareTag("OOB"))
            {
                Debug.Log($"{gameObject.name} entered OOB trigger zone — destroying.");
                Destroy(gameObject);
                return;
            }

            current = current.parent;
        }
    }
}
