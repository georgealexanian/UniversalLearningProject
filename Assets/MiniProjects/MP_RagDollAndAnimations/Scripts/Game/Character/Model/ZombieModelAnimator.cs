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
        }

        public void StandUpAnim(bool faceDown)
        {
            if (faceDown)
            {
                animator.SetFloat(StandUpAnimIndexParam, 0);
                animator.SetTrigger(StandUpParam);
            }
            else
            {
                animator.SetFloat(StandUpAnimIndexParam, 1);
                animator.SetTrigger(StandUpParam);
            }
        }

    }
}
