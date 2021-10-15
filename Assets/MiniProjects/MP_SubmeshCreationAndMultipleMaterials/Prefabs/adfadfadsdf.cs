using System.Collections.Generic;
using UnityEngine;

public class adfadfadsdf
{
    public void AdvancedMerge(out CombineInstance[] combineInstances, GameObject gameObject)
    {
        MeshFilter[] filters = gameObject.GetComponentsInChildren<MeshFilter>(false);

        List<Material> materials = new List<Material>();
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>(false);
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.transform == gameObject.transform)
            {
                continue;
            } 
            Material[] localMats = renderer.sharedMaterials;
            foreach (Material localMat in localMats)
            {
                if (!materials.Contains(localMat))
                {
                    materials.Add(localMat);
                }
            }
        }

        List<Mesh> submeshes = new List<Mesh>();
        foreach (Material material in materials)
        {
            List<CombineInstance> combiners = new List<CombineInstance>();
            foreach (MeshFilter filter in filters)
            {
                if (filter.transform == gameObject.transform)
                {
                    continue;
                }
                MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
                if (renderer == null)
                {
                    continue;
                }

                Material[] localMaterials = renderer.sharedMaterials;
                for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++)
                {
                    if (localMaterials[materialIndex] != material)
                    {
                        continue;
                    };
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = filter.sharedMesh;
                    ci.subMeshIndex = materialIndex;
                    ci.transform = Matrix4x4.identity;
                    combiners.Add(ci);
                }
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiners.ToArray(), true);
            submeshes.Add(mesh);
        }

        List<CombineInstance> finalCombiners = new List<CombineInstance>();
        foreach (Mesh mesh in submeshes)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiners.Add(ci);
        }

        combineInstances = finalCombiners.ToArray();
        // Mesh finalMesh = new Mesh();
        // finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
        // Debug.Log("Final mesh has " + submeshes.Count + " materials.");
    }
}