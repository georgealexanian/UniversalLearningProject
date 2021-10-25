namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Game.Character.Model
{
    using UnityEngine;

    public class ZombieModelAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int StandUpAnimIndexParam = Animator.StringToHash("StandUpAnimIndex");
        private static readonly int StandUpParam = Animator.StringToHash("StandUp");

        
        public void EnableAnimator(bool enable)
        {
            animator.enabled = enable;
            
            ResetTriggers(!enable);
        }

        public void StandUpAnim(bool faceDown)
        {
            var blendIndex = faceDown ? 0 : 1;
            animator.SetFloat(StandUpAnimIndexParam, blendIndex);
            animator.SetTrigger(StandUpParam);
        }

        private void ResetTriggers(bool reset)
        {
            if (reset)
            {
                animator.ResetTrigger(StandUpParam);
            }
        }
    }
}
