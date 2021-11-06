namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Game.Character.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class ZombieRagDollView : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> ragDollRigidBodies;
        [SerializeField] private List<Collider> ragDollColliders;
        [SerializeField] private Transform rootBone;
        [SerializeField] private ZombieModelAnimator modelAnimator;

        public bool IsRagDoll { get; private set; } = false;

        private List<RagDollBonesInfo> ragDollBonesInfos;
        private bool isStandingUp;
        private float baseBlendSpeed = .2f;
        private float blendSpeed;

        
        private void LateUpdate()
        {
            BlendStandUpAnimWithRagDoll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (IsRagDoll)
                {
                    StandUp();
                }
                else
                {
                    PrepareRagDollState();
                }
            }
        }

        private void BlendStandUpAnimWithRagDoll()
        {
            if (!isStandingUp)
            {
                return;
            }
            
            foreach (var boneInfo in ragDollBonesInfos)
            {
                boneInfo.bone.position = Vector3.Lerp(boneInfo.bone.position, boneInfo.position, blendSpeed);
                boneInfo.bone.rotation = Quaternion.Slerp(boneInfo.bone.rotation, boneInfo.rotation, blendSpeed);
            }

            blendSpeed = Mathf.Clamp(blendSpeed - .002f, 0, 100000);
        }

        private void CacheRagDolledPosition()
        {
            var skeleton = 
                GetComponentsInChildren<Transform>()
                .Where(x => x.TryGetComponent<Rigidbody>(out var bone));

            ragDollBonesInfos = skeleton
                .Select(x => new RagDollBonesInfo(x.transform, x.position, x.rotation))
                .ToList();
        }

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
            CacheRagDolledPosition();

            isStandingUp = true;
            blendSpeed = baseBlendSpeed;
            modelAnimator.StandUpAnim(CheckFaceDown(), () =>
            {
                isStandingUp = false;
            });
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
        
        private class RagDollBonesInfo
        {
            public Transform bone;
            public Vector3 position;
            public Quaternion rotation;

            public RagDollBonesInfo(Transform bone, Vector3 position, Quaternion rotation)
            {
                this.bone = bone;
                this.position = position;
                this.rotation = rotation;
            }
        }
    }
}
