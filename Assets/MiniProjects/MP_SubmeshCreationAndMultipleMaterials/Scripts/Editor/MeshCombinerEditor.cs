namespace MiniProjects.MP_SubmeshCreationAndMultipleMaterials.Scripts.Editor
{
    using System.Collections.Generic;
    using System.Linq;
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

            InitializePrefabAndCreateNewMesh(ref meshCombiner, out Mesh finalMesh);

            // CombineInstance[] combineInstances = new CombineInstance[filters.Length];
            ConfigureCombineInstances(out CombineInstance[] combineInstances, filters, meshCombiner);
            // var adfdsda = new adfadfadsdf();
            // adfdsda.AdvancedMerge(out var combineInstances, meshCombiner.gameObject);
            
            CombineAndSetMeshes(ref finalMesh, combineInstances, meshCombiner);

            transform.rotation = oldRotation;
            transform.position = oldPosition;

            MarkChildrenToBeRemoved(transform);
            SaveMeshToFolder(ref finalMesh);
        }

        private void InitializePrefabAndCreateNewMesh(ref MeshCombiner meshCombiner, out Mesh finalMesh)
        {
            if (meshCombiner.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                var sharedMesh = meshFilter.sharedMesh;
                finalMesh = sharedMesh == null 
                    ? new Mesh {name = meshCombiner.gameObject.name}
                    : sharedMesh;
            }
            else
            {
                finalMesh = new Mesh {name = meshCombiner.gameObject.name};
                
                meshCombiner.gameObject.AddComponent<MeshFilter>();
                meshCombiner.gameObject.AddComponent<MeshRenderer>();
            }
        }

        private void ConfigureCombineInstances(out CombineInstance[] combineInstances, MeshFilter[] filters, MeshCombiner meshCombiner)
        {
            var allRenderers = meshCombiner.GetComponentsInChildren<MeshRenderer>();
            List<Material> materials =
                new HashSet<Material>(allRenderers
                    .Where(x => x.sharedMaterial != null)
                    .Select(x => x.sharedMaterial))
                    .ToList();

            List<Mesh> subMeshes = new List<Mesh>();
            for (int matIndex = 0; matIndex < materials.Count; matIndex++)
            {
                var sharingThisMat = allRenderers
                    .ToList()
                    .FindAll(x => x.sharedMaterial != null && x.sharedMaterial == materials[matIndex])
                    .Select(x => x.GetComponent<MeshFilter>())
                    .ToList();

                CombineInstance[] localCombineInstances = new CombineInstance[sharingThisMat.Count];
                for (int j = 0; j < sharingThisMat.Count; j++)
                {
                    if (sharingThisMat[j].transform == meshCombiner.transform)
                    {
                        continue;
                    }
                    
                    localCombineInstances[j].subMeshIndex = matIndex;
                    localCombineInstances[j].mesh = sharingThisMat[j].sharedMesh;
                    localCombineInstances[j].transform = Matrix4x4.identity;
                }

                var subMesh = new Mesh();
                subMesh.CombineMeshes(localCombineInstances, true);
                
                subMeshes.Add(subMesh);
            }

            List<CombineInstance> finalCombineInstances = new List<CombineInstance>();
            foreach (var subMesh in subMeshes)
            {
                CombineInstance finalCombineInstance = new CombineInstance
                {
                    subMeshIndex = 0, 
                    mesh = subMesh, 
                    transform = Matrix4x4.identity
                };

                finalCombineInstances.Add(finalCombineInstance);
            }

            combineInstances = finalCombineInstances.ToArray();

            
            //OLD LOGIC!!! THE CODE BELOW WORKS PERFECTLY EXCEPT IT DOES NOT TAKE INTO ACCOUNT DIFFERENT MATERIALS.
            // for (int i = 0; i < filters.Length; i++)
            // {
            //     if (filters[i].transform == meshCombiner.transform)
            //     {
            //         continue;
            //     }
            //
            //     combineInstances[i].subMeshIndex = 0;
            //     combineInstances[i].mesh = filters[i].sharedMesh;
            //     combineInstances[i].transform = filters[i].transform.localToWorldMatrix;
            // }
        }

        private void CombineAndSetMeshes(ref Mesh finalMesh, CombineInstance[] combineInstances, MeshCombiner meshCombiner)
        {
            // finalMesh.CombineMeshes(combineInstances, false);
            // meshCombiner.GetComponent<MeshFilter>().sharedMesh = finalMesh;
        }

        private void MarkChildrenToBeRemoved(Transform transform)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
                child.name = "TO BE REMOVED MANUALLY";
            }
        }

        private void SaveMeshToFolder(ref Mesh mesh)
        {
            AssetDatabase.CreateAsset(mesh, $"Assets/MiniProjects/MP_SubmeshCreationAndMultipleMaterials/Resources/Meshes/{mesh.name}.mesh");
            AssetDatabase.SaveAssets();
        }
    }
}
