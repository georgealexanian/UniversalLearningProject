namespace MiniProjects.MP_SubmeshCreationAndMultipleMaterials.Scripts.Editor
{
    using Game;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(MeshCombiner), true)]
    public class MeshCombinerEditor : Editor
    {
        private const float SpaceBetweenButtons = 10;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetType = (MeshCombiner) target;
            
            GUILayout.Space(SpaceBetweenButtons);
            if (GUILayout.Button("Combine child meshes into 1 mesh"))
            {
                CombineChildMeshesIntoOne(targetType);
            }
            
            EditorUtility.SetDirty(target);
        }

        
        private void CombineChildMeshesIntoOne(MeshCombiner meshCombiner)
        {
            var transform = meshCombiner.transform;
            
            Quaternion oldRotation = transform.rotation;
            Vector3 oldPosition = transform.position;
            
            transform.rotation = quaternion.identity;
            transform.position = Vector3.zero;

            MeshFilter[] filters = meshCombiner.GetComponentsInChildren<MeshFilter>();

            Mesh finalMesh;
            if (meshCombiner.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                finalMesh = meshFilter.sharedMesh;
            }
            else
            {
                finalMesh = new Mesh {name = meshCombiner.gameObject.name};
                
                meshCombiner.gameObject.AddComponent<MeshFilter>();
                meshCombiner.gameObject.AddComponent<MeshRenderer>();
            }

            CombineInstance[] combineInstances = new CombineInstance[filters.Length];
            for (int i = 0; i < filters.Length; i++)
            {
                if (filters[i].transform == meshCombiner.transform)
                {
                    continue;
                }

                combineInstances[i].subMeshIndex = 0;
                combineInstances[i].mesh = filters[i].sharedMesh;
                combineInstances[i].transform = filters[i].transform.localToWorldMatrix;
            }
            
            finalMesh.CombineMeshes(combineInstances);
            meshCombiner.GetComponent<MeshFilter>().sharedMesh = finalMesh;

            transform.rotation = oldRotation;
            transform.position = oldPosition;

            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
