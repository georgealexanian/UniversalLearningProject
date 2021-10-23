namespace MiniProjects.MP_MeshDeformations.Scripts.Game
{
    using UnityEngine;
    
    [RequireComponent(typeof(MeshFilter))]
    public class MeshDeformer : MonoBehaviour
    {
        [SerializeField] private float springForce = 20f;
        [SerializeField] private float damping = 5f;

        private Mesh deformingMesh;
        private Vector3[] originalVertices;
        private Vector3[] displacedVertices;
        private Vector3[] vertexVelocities;

        private Vector3 currentDispacement;

        private Camera mainCamera;


        private void Start()
        {
            CacheVerticesAndMesh();
            
            mainCamera = Camera.main;
        }
        
        private void Update () 
        {
            DeformMesh();
        }

        private void DeformMesh()
        {
            for (int i = 0; i < displacedVertices.Length; i++) 
            {
                UpdateVertex(i);
            }
            deformingMesh.vertices = displacedVertices;
            deformingMesh.RecalculateNormals();

            RecalculateMeshData();
        }
        
        private void UpdateVertex (int i) 
        {
            currentDispacement = displacedVertices[i] - originalVertices[i];
            vertexVelocities[i] -= currentDispacement * springForce * Time.deltaTime;
            displacedVertices[i] *= 1f - damping * Time.deltaTime;
            displacedVertices[i] += vertexVelocities[i] * Time.deltaTime;
        }

        private void RecalculateMeshData()
        {
            deformingMesh.RecalculateBounds();
            deformingMesh.RecalculateNormals();
            deformingMesh.RecalculateTangents();
        }

        private void CacheVerticesAndMesh()
        {
            deformingMesh = GetComponent<MeshFilter>().mesh;
            originalVertices = deformingMesh.vertices;
            displacedVertices = new Vector3[originalVertices.Length];
            for (var i = 0; i < originalVertices.Length; i++) 
            {
                displacedVertices[i] = originalVertices[i];
            }
            vertexVelocities = new Vector3[originalVertices.Length];
        }
        
        public void AddDeformingForce (Vector3 point, float force) 
        {
            Debug.DrawLine(mainCamera.transform.position, point, Color.green);
            
            CalculateDispacements(point, force);
        }

        private void CalculateDispacements(Vector3 point, float force)
        {
            for (int i = 0; i < displacedVertices.Length; i++) 
            {
                AddForceToVertex(i, point, force);
            }
        }
        
        private void AddForceToVertex (int i, Vector3 point, float force) 
        {
            Vector3 pointToVertex = displacedVertices[i] - point;
            float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
            float velocity = attenuatedForce * Time.deltaTime;
            vertexVelocities[i] += pointToVertex.normalized * velocity;
        }
    }
}