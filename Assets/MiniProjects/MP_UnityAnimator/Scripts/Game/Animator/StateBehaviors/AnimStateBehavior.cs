namespace MiniProjects.MP_UnityAnimator.Scripts.Game.Animator.StateBehaviors
{
    using UnityEngine;

    public class AnimStateBehavior : StateMachineBehaviour
    {
        [SerializeField] private CharacterAnimator characterAnimator;
        
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            characterAnimator.StartedAnimClip(GetCurrentClipName(animator));
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            characterAnimator.FinishedAnimClip(GetCurrentClipName(animator));
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            characterAnimator.PlayingAnimClip(GetCurrentClipName(animator));
        }

        
        private string GetCurrentClipName(Animator animator)
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            return clipInfo[0].clip.name;
        }
    }
}
