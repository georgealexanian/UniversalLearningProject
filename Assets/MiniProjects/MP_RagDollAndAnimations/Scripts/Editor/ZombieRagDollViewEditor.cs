namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Editor
{
    using System.Linq;
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
            }
            
            GUILayout.Space(OffsetSpace);
            if (GUILayout.Button("Fix InBetween collisions"))
            {
                FixInBetweenCollisions(targetType);
            }
            
            EditorUtility.SetDirty(target);
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
    }
}
