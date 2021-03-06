namespace MP_JellyMesh.Scripts.Editor
{
    using System.Linq;
    using System.Reflection;
    using Game;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(JellyMeshContainer), true)]
    public class JellyMeshContainerEditor : Editor
    {
        private const float SpaceBetweenButtons = 10;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetType = (JellyMeshContainer) target;
            
            GUILayout.Space(SpaceBetweenButtons);
            if (GUILayout.Button("Add colliders to all Jelly Mesh components"))
            {
                AddColliders(targetType);
            }
            
            GUILayout.Space(SpaceBetweenButtons);
            if (GUILayout.Button("Set All Child JellyMesh values"))
            {
                SetAllChildJellyMeshValues(targetType);
            }
            
            EditorUtility.SetDirty(target);
        }

        private void AddColliders(JellyMeshContainer jellyMeshContainer)
        {
            var jellyMeshes = jellyMeshContainer
                .GetComponentsInChildren<JellyMesh>()
                .Where(x => !x.TryGetComponent<Collider>(out var collider))
                .ToList();
            
            jellyMeshes.ForEach(x =>
            {
                x.gameObject.AddComponent<MeshCollider>();
            });
        }

        private void SetAllChildJellyMeshValues(JellyMeshContainer jellyMeshContainer)
        {
            var jellyMeshes = jellyMeshContainer
                .GetComponentsInChildren<JellyMesh>()
                .ToList();
            
            jellyMeshes.ForEach(x =>
            {
                var type = x.GetType();
                type
                    .GetField(nameof(jellyMeshContainer.bounceSpeed), BindingFlags.Instance | BindingFlags.NonPublic)?
                    .SetValue(x, jellyMeshContainer.bounceSpeed);
                type
                    .GetField(nameof(jellyMeshContainer.fallForce), BindingFlags.Instance | BindingFlags.NonPublic)?
                    .SetValue(x, jellyMeshContainer.fallForce);
                type
                    .GetField(nameof(jellyMeshContainer.stiffness), BindingFlags.Instance | BindingFlags.NonPublic)?
                    .SetValue(x, jellyMeshContainer.stiffness);
            });
        }
    }
}
