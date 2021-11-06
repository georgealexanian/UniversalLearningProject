namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Game.Character.Model
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class ZombieModelAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int StandUpAnimIndexParam = Animator.StringToHash("StandUpAnimIndex");
        private static readonly int StandUpParam = Animator.StringToHash("StandUp");

        private event Action StandUpFinishedEvent;

        
        public void EnableAnimator(bool enable)
        {
            animator.enabled = enable;
            
            ResetTriggers(!enable);
        }

        public void StandUpAnim(bool faceDown, Action standUpFinished)
        {
            var blendIndex = faceDown ? 0 : 1;
            animator.SetFloat(StandUpAnimIndexParam, blendIndex);
            animator.SetTrigger(StandUpParam);

            this.StandUpFinishedEvent = standUpFinished;
        }

        private void ResetTriggers(bool reset)
        {
            if (reset)
            {
                animator.ResetTrigger(StandUpParam);
            }
        }

        [Preserve]
        public void StandUpFinished()
        {
            StandUpFinishedEvent?.Invoke();
        }
    }
}
