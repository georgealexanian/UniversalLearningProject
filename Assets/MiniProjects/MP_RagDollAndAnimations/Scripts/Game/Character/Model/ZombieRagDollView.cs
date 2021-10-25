namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Game.Character.Model
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ZombieRagDollView : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> ragDollRigidBodies;
        [SerializeField] private List<Collider> ragDollColliders;

        [SerializeField] private Transform rootBone;
        [SerializeField] private ZombieModelAnimator modelAnimator;

        public bool IsRagDoll { get; private set; } = false;
        

        private bool CheckFaceDown()
        {
            return Vector3.Dot(rootBone.up, Vector3.up) < 0;
        }

        public void StandUp()
        {
            if (!IsRagDoll)
            {
                return;
            }
            
            PreparePositions();

            modelAnimator.StandUpAnim(CheckFaceDown());
            PrepareAnimatedState();
        }

        public void PrepareRagDollState()
        {
            ragDollRigidBodies.ForEach(x =>
            {
                x.isKinematic = false;
                x.useGravity = true;
            });
            ragDollColliders.ForEach(x => x.enabled = true);

            modelAnimator.EnableAnimator(false);

            IsRagDoll = true;
        }

        public void PrepareAnimatedState()
        {
            ragDollRigidBodies.ForEach(x =>
            {
                x.isKinematic = true;
                x.useGravity = false;
            });
            ragDollColliders.ForEach(x => x.enabled = false);
            
            modelAnimator.EnableAnimator(true);
            
            IsRagDoll = false;
        }

        private void PreparePositions()
        {
            var localPosition = rootBone.localPosition;
            transform.localPosition += new Vector3(localPosition.x, 0, localPosition.z);
            localPosition = new Vector3(0, 0, 0);
            rootBone.localPosition = localPosition;
        }
    }
}
