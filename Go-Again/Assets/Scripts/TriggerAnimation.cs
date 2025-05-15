using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    private Animator _animator;
    private float currentTime = 0f;
    private float speed = 1f;
    private bool hasReachedEnd = false;
    private bool hasSpawnedParticles = false;

    [Tooltip("Name of the state with the animation")]
    public string animationStateName = "MyAnimState";
    [Tooltip("Layer index where the animation is")]
    public int layerIndex = 0;

    [Header("Particle Settings")]
    public GameObject particlePrefab;
    public Transform particleSpawnPoint;

    public GameObject decalPrefab;

    public Vector3 offset = new Vector3(0f, 0.02f, 0f); // Adjustable in Inspector

    public float particleTriggerTime = 0.8f; // normalized time

    private CollisionLogic collisionLogic;

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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            collisionLogic = player.GetComponent<CollisionLogic>();
            if (collisionLogic == null)
            {
                Debug.LogWarning("CollisionLogic script not found on Player.");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not found in scene.");
        }
    }

    private void Update()
    {
        bool isHolding = Input.GetKey(KeyCode.E);
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

            if (!hasSpawnedParticles && currentTime >= particleTriggerTime)
            {
                TriggerVFX();
                AudioManager.Instance.Play("stabHit");

                if (collisionLogic != null)
                {
                    collisionLogic.TriggerDeath();
                }
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

    public void TriggerVFX()
    {
        if (!hasSpawnedParticles)
        {
            SpawnParticles();
            hasSpawnedParticles = true;
            SpawnDecal();
        }
    }

    private void SpawnDecal()
    {
        Debug.Log("Spawned decal at: " + transform.position);

        if (decalPrefab != null)
        {
            Vector3 spawnPosition = transform.position + offset;

            float randomY = Random.Range(0f, 360f);
            Quaternion randomYRotation = Quaternion.Euler(0f, randomY, 0f);

            Quaternion spawnRotation = randomYRotation * decalPrefab.transform.rotation;

            Instantiate(decalPrefab, spawnPosition, spawnRotation);
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
