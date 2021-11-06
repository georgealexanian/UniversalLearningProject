namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Editor
{
    using System.Linq;
    using System.Reflection;
    using Game.Character.Model;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ZombieRagDollView), false)]
    public class ZombieRagDollViewEditor : Editor
    {
        private const float OffsetSpace = 10f;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetType = (ZombieRagDollView) target;

            GUILayout.Space(OffsetSpace);
            if (GUILayout.Button("Fix RagDoll projections"))
            {
                FixRagDollChildrenProjections(targetType);
                EditorUtility.SetDirty(target);
            }
            
            GUILayout.Space(OffsetSpace);
            if (GUILayout.Button("Fix InBetween collisions"))
            {
                FixInBetweenCollisions(targetType);
                EditorUtility.SetDirty(target);
            }
            
            GUILayout.Space(OffsetSpace);
            if (GUILayout.Button("Cache RigidBodies"))
            {
                CacheRagDollRigidBodies(targetType);
                EditorUtility.SetDirty(target);
            }
            
            GUILayout.Space(OffsetSpace);
            if (GUILayout.Button("Cache Colliders"))
            {
                CacheRagDollColliders(targetType);
                EditorUtility.SetDirty(target);
            }
            
            GUILayout.Space(OffsetSpace);
            if (GUILayout.Button("'''''STAND UP'''''"))
            {
                StandUpCharacter(targetType);
                EditorUtility.SetDirty(target);
            }
            
            GUILayout.Space(OffsetSpace);
            if (GUILayout.Button("''''''BECOME RAGDOLL'''''"))
            {
                BecomeRagDoll(targetType);
                EditorUtility.SetDirty(target);
            }
        }

        private void FixRagDollChildrenProjections(ZombieRagDollView view)
        {
            var joints = view
                .GetComponentsInChildren<CharacterJoint>()
                .ToList();
            joints.ForEach(x =>
            {
                x.enableProjection = true;
            });
        }

        private void FixInBetweenCollisions(ZombieRagDollView view)
        {
            var joints = view
                .GetComponentsInChildren<CharacterJoint>()
                .ToList();
            joints.ForEach(x =>
            {
                x.enableCollision = true;
            });
        }

        private void CacheRagDollRigidBodies(ZombieRagDollView view)
        {
            var rigidBodies = view
                .GetComponentsInChildren<Rigidbody>()
                .ToList();
            
            var type = view.GetType();
            type
                .GetField("ragDollRigidBodies", BindingFlags.Instance | BindingFlags.NonPublic)?
                .SetValue(view, rigidBodies);
        }
        
        private void CacheRagDollColliders(ZombieRagDollView view)
        {
            var colliders = view
                .GetComponentsInChildren<Collider>()
                .ToList();
            
            var type = view.GetType();
            type
                .GetField("ragDollColliders", BindingFlags.Instance | BindingFlags.NonPublic)?
                .SetValue(view, colliders);
        }

        private void StandUpCharacter(ZombieRagDollView view)
        {
            view.StandUp();
        }

        private void BecomeRagDoll(ZombieRagDollView view)
        {
            view.PrepareRagDollState();
        }
    }
}
