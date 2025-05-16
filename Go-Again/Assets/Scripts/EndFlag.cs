using UnityEngine;

public class EndFlag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the end flag!");
            // SceneController.instance.NextLevel();

            // Stop the time and show win screen
            
            GameManager.instance.winText.gameObject.SetActive(true);
            
        }
        else
        {
            Debug.Log("Non-player object triggered the end flag.");
        }
    }
}
