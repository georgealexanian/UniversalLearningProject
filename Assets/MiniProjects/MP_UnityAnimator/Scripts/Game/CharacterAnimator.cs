namespace MiniProjects.MP_UnityAnimator.Scripts.Game
{
    using UnityEngine;

    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private void Update()
        {
            ProcessKeys();
        }

        private void ProcessKeys()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StandUp();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.SetTrigger("Walk");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                animator.SetTrigger("Run");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetTrigger("Kick");
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                animator.SetTrigger("Idle");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                animator.SetTrigger("Dance");
            }
        }

        private void StandUp()
        {
            animator.SetTrigger("GetUpFaceUp");
        }
    }
}
