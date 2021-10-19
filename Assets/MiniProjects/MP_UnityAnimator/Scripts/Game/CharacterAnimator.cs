namespace MiniProjects.MP_UnityAnimator.Scripts.Game
{
    using UnityEngine;
    using UnityEngine.Scripting;

    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Animator animator;
        
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
            if (Input.GetKeyDown(KeyCode.B))
            {
                animator.SetTrigger("WalkAndRunBTMix");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                var floatValue = animator.GetFloat("WalkAndRunBTVertical");
                floatValue -= 0.1f;
                animator.SetFloat("WalkAndRunBTVertical", floatValue);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var floatValue = animator.GetFloat("WalkAndRunBTVertical");
                floatValue += 0.1f;
                animator.SetFloat("WalkAndRunBTVertical", floatValue);
            }
        }

        private void StandUp()
        {
            animator.SetTrigger("GetUpFaceUp");
        }
        
        public void StartedAnimClip(string clipName)
        {
            Debug.Log(clipName);
        }
        
        public void FinishedAnimClip(string clipName)
        {
            Debug.Log(clipName);
        }
        
        public void PlayingAnimClip(string clipName)
        {
            
        }

        [Preserve]
        public void KickFinished()
        {
            Debug.Log("Kick Finished Event");
        }
    }
}
