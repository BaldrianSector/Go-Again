using UnityEngine;

namespace Antimo_s_private_mappe
{
    public class KillYourself : MonoBehaviour
    {
        private static readonly int PlayAnim = Animator.StringToHash("PlayAnim");
        private Animator _animator;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) // or any key you like
            {
                _animator.SetTrigger(PlayAnim); // Use the name of your animation clip
            }
        }
    }
}
