using UnityEngine;

public class Death_trigger : MonoBehaviour
{
    // Called when another collider enters this trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone!");
        }
    }
}