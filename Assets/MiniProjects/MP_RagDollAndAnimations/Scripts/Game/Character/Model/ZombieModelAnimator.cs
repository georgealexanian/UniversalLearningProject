namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Game.Character.Model
{
    using UnityEngine;

    public class ZombieModelAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        public void EnableAnimator(bool enable)
        {
            animator.enabled = enable;
        }
    }
}
