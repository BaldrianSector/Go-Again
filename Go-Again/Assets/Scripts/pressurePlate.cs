using UnityEngine;

public class pressurePlate : MonoBehaviour
{
    [Header("Animation Target")]
    public GameObject targetObject; // Assign in Inspector
    public string boolParameterName = "isActive"; // Animator bool parameter to toggle

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ghost"))
        {
            Debug.Log($"Pressure plate triggered by {other.tag}.");
            SetTargetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ghost"))
        {
            Debug.Log($"{other.tag} left the pressure plate.");
            SetTargetActive(false);
        }
    }

    private void SetTargetActive(bool state)
    {
        if (targetObject != null)
        {
            Animator animator = targetObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool(boolParameterName, state);
                Debug.Log($"Set '{boolParameterName}' to {state} on {targetObject.name}.");
            }
            else
            {
                Debug.LogWarning($"No Animator found on {targetObject.name}.");
            }
        }
        else
        {
            Debug.LogWarning("Target object is not assigned.");
        }
    }
}
