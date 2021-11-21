namespace MiniProjects.MP_ProceduralTerrainGeneration.Scripts.Game
{
    using System.Collections;
    using UnityEngine;

    public class ProceduralTerrainGeneration : MonoBehaviour
    {
        [SerializeField] private int horizontalSize = 20;
        [SerializeField] private int forwardSize = 20;
        [Range(-5, 5)]
        [SerializeField] private float heightSize = 2f;
        
        [SerializeField] private float horizontalTiling = 0.3f;
        [SerializeField] private float forwardTiling = 0.3f;

        [SerializeField] private float generationAwaitTime = 0.03f;
        [SerializeField] private bool updateRuntime = false;
        [SerializeField] private bool drawGizmos = true;
        
        private Mesh mesh;
        private MeshFilter meshFilter;
        
        private int[] triangles;
        private Vector3[] vertices;

        private Coroutine coroutine;


        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        private void Start()
        {
            mesh = new Mesh();
            meshFilter.mesh = mesh;
            
            coroutine = StartCoroutine(CreateShape());
        }

        private void Update()
        {
            if (updateRuntime)
            {
                coroutine = StartCoroutine(CreateShape(true));
            }
            UpdateMesh();
        }

        private IEnumerator CreateShape(bool overrideDelay = false)
        {
            vertices = new Vector3[(horizontalSize + 1) * (forwardSize + 1)];

            int i = 0;
            for (int z = 0; z <= forwardSize; z++)
            {
                for (int x = 0; x <= horizontalSize; x++)
                {
                    float height = Mathf.PerlinNoise(x * horizontalTiling, z * forwardTiling) * heightSize;
                    vertices[i] = new Vector3(x, height, z);
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

                    if (!overrideDelay)
                    {
                        yield return new WaitForSeconds(generationAwaitTime);
                    }
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
            if (!drawGizmos)
            {
                return;
            }
            
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
}
