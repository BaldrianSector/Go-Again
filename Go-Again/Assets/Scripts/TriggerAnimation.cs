using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    private Animator _animator;
    private float currentTime = 0f;
    private float speed = 1f;
    private bool hasReachedEnd = false;
    private bool hasSpawnedParticles = false;

    [Tooltip("Name of the state with the animation")]
    public string animationStateName = "MyAnimState"; // <-- change this
    [Tooltip("Layer index where the animation is")]
    public int layerIndex = 0;

    [Header("Particle Settings")]
    public GameObject particlePrefab;

    public GameObject decalPrefab;
    public Transform particleSpawnPoint;
    public float particleTriggerTime = 0.8f; // normalized time

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator not found on GameObject: " + gameObject.name);
        }

        _animator.Play(animationStateName, layerIndex, 0f);
        _animator.Update(0f);
        _animator.speed = 0f;
    }

    private void Update()
    {
        bool isHolding = Input.GetKey(KeyCode.T);
        float delta = Time.deltaTime;

        if (isHolding)
        {
            currentTime += delta * speed;

            if (currentTime >= 1f)
            {
                currentTime = 1f;

                if (!hasReachedEnd)
                {
                    Debug.Log("Animation reached the end");
                    hasReachedEnd = true;
                }
            }
            else
            {
                hasReachedEnd = false;
            }

            // 🎆 Spawn particle once when reaching the trigger time
            if (!hasSpawnedParticles && currentTime >= particleTriggerTime)
            {
                SpawnParticles();
                hasSpawnedParticles = true;
                
                SpawnDecal();
            }
        }
        else
        {
            currentTime -= delta * speed;

            if (currentTime < particleTriggerTime)
            {
                hasSpawnedParticles = false; // reset so it can play again
            }

            if (currentTime < 0f)
            {
                currentTime = 0f;
            }

            hasReachedEnd = false;
        }

        _animator.Play(animationStateName, layerIndex, currentTime);
        _animator.Update(0f);
    }

    private void SpawnDecal()
    {
        Debug.Log("Spawned decal at: " + transform.position);

        if (decalPrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 0.20f; // slight upward offset
            Instantiate(decalPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Missing decal prefab.");
        }
    }


    private void SpawnParticles()
    {
        if (particlePrefab != null && particleSpawnPoint != null)
        {
            Instantiate(particlePrefab, particleSpawnPoint.position, particleSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Missing particle prefab or spawn point.");
        }
    }
}
