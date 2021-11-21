namespace MiniProjects.MP_ProceduralTerrainGeneration.Scripts.Game
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class ProceduralTerrainGeneration : MonoBehaviour
    {
        [SerializeField] private int horizontalSize = 20;
        [SerializeField] private int forwardSize = 20;
        
        [SerializeField] private float generationAwaitTime = 0.03f;

        private Mesh mesh;
        private MeshFilter meshFilter;
        
        private int[] triangles;
        private Vector3[] vertices;


        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        private void Start()
        {
            mesh = new Mesh();
            meshFilter.mesh = mesh;
            
            StartCoroutine(CreateShape());
        }

        private void Update()
        {
            UpdateMesh();
        }

        private IEnumerator CreateShape()
        {
            vertices = new Vector3[(horizontalSize + 1) * (forwardSize + 1)];

            int i = 0;
            for (int z = 0; z <= forwardSize; z++)
            {
                for (int x = 0; x <= horizontalSize; x++)
                {
                    vertices[i] = new Vector3(x, 0, z);

                    i++;
                }
            }

            triangles = new int[horizontalSize * forwardSize * 6];
            int vert = 0;
            int tris = 0;
            for (int z = 0; z < forwardSize; z++)
            {
                for (int x = 0; x < horizontalSize; x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + horizontalSize + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + horizontalSize + 1;
                    triangles[tris + 5] = vert + horizontalSize + 2;
                
                    vert++;
                    tris += 6;

                    yield return new WaitForSeconds(generationAwaitTime);
                }

                vert++;
            }
        }

        private void UpdateMesh()
        {
            mesh.Clear();

            mesh.vertices = vertices; 
            mesh.triangles = triangles;
            
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
        }

        private void OnDrawGizmos()
        {
            if (vertices == null)
            {
                return;
            }

            DrawVertexGizmos();
        }

        private void DrawVertexGizmos()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
}
