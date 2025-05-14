using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    private static readonly int PlayAnim = Animator.StringToHash("PlayAnim");
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator not found on GameObject: " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T key pressed — triggering animation");
            _animator.SetTrigger(PlayAnim);
        }
    }
}
