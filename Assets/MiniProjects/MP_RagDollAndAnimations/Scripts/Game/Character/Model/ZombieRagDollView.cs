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

        
        private void LateUpdate()
        {
            Debug.Log(Vector3.Dot(rootBone.up, Vector3.up));
        }

        private bool CheckFaceDown()
        {
            return Vector3.Dot(rootBone.up, Vector3.up) > 0;
        }

        public void StandUp()
        {
            if (CheckFaceDown())
            {
                
            }
            else
            {
                
            }
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
        }

        
    }
}
