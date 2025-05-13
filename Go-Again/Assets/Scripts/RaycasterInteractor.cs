using UnityEngine;
using System.Collections.Generic;

public class RaycastInteractor : MonoBehaviour
{
    [Header("Global Toggle")]
    public bool interactionsEnabled = true;

    [Header("Interaction Settings")]
    public float rayDistance = 3f;
    public LayerMask interactableLayer;
    public Vector3 rayOriginOffset = new Vector3(0, 0.6f, 0);
    public Vector2 rayAngleOffset = Vector2.zero; // x = pitch (up/down), y = yaw (left/right)
    public bool disableGhostMaterialOnHit = false;
    public Material ghostHighlightMaterial;

    private Camera mainCamera;

    private GameObject currentGhostHit;
    private Renderer lastGhostRenderer;
    private Material originalGhostMaterial;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!interactionsEnabled)
        {
            RestoreGhostMaterial();
            return;
        }

        Vector3 origin = transform.position + rayOriginOffset;

        // Correctly apply yaw and pitch offsets relative to the camera's local orientation
        Vector3 direction = mainCamera.transform.forward;
        direction = Quaternion.AngleAxis(rayAngleOffset.y, Vector3.up) * direction; // Yaw (left/right)
        direction = Quaternion.AngleAxis(rayAngleOffset.x, mainCamera.transform.right) * direction; // Pitch (up/down)

        Color rayColor = Color.red;

        Ray ray = new Ray(origin, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, interactableLayer);

        bool ghostWasHit = false;

        foreach (var hit in hits)
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.CompareTag("Ghost"))
            {
                ghostWasHit = true;
                rayColor = Color.green;

                Renderer renderer = hitObj.GetComponent<Renderer>();
                if (disableGhostMaterialOnHit && renderer != null)
                {
                    if (currentGhostHit != hitObj)
                    {
                        RestoreGhostMaterial();

                        originalGhostMaterial = renderer.material;
                        renderer.material = ghostHighlightMaterial;

                        currentGhostHit = hitObj;
                        lastGhostRenderer = renderer;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Destroyed Ghost: " + hitObj.name);
                    Destroy(hitObj);
                    currentGhostHit = null;
                    lastGhostRenderer = null;
                    originalGhostMaterial = null;
                    break;
                }

                break; // only interact with the first Ghost
            }
        }

        if (!ghostWasHit && currentGhostHit != null)
        {
            RestoreGhostMaterial();
        }

        Debug.DrawRay(origin, direction * rayDistance, rayColor);
    }

    private void RestoreGhostMaterial()
    {
        if (lastGhostRenderer != null && originalGhostMaterial != null)
        {
            lastGhostRenderer.material = originalGhostMaterial;
        }

        currentGhostHit = null;
        lastGhostRenderer = null;
        originalGhostMaterial = null;
    }
}
