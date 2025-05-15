using UnityEngine;

public class MoverSwapLogic : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.Play("portalAmbient");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject respawnObject = GameObject.FindGameObjectWithTag("Respawn");

            if (respawnObject != null && respawnObject != gameObject)
            {
                GameManager.instance.ResetLives();

                Vector3 tempPosition = transform.position;
                transform.position = respawnObject.transform.position;
                respawnObject.transform.position = tempPosition;

                Debug.Log("Swapped Mover with Respawn.");
            }
            else
            {
                Debug.LogWarning("No suitable object with tag 'Respawn' found.");
            }
        }
    }
}
