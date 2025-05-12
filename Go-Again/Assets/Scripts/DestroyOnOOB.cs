using UnityEngine;

public class DestroyOnOOB : MonoBehaviour
{
    public GameObject oobRootObject; // Assign the parent of the OOB colliders in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name}");

        if (oobRootObject == null)
        {
            Debug.LogWarning("OOB root not assigned!");
            return;
        }

        // Check if the trigger belongs to the assigned OOB root or one of its children
        if (other.transform.IsChildOf(oobRootObject.transform) || other.gameObject == oobRootObject)
        {
            Debug.Log($"{gameObject.name} entered OOB trigger zone — destroying.");
            Destroy(gameObject);
        }
    }
}
