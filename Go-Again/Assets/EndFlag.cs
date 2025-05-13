using UnityEngine;

public class EndFlag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the end flag!");
        }
        else
        {
            Debug.Log("Non-player object triggered the end flag.");
        }
    }
}
