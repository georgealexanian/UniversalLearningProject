namespace MP_JellyMesh.Scripts.Game
{
    using UnityEngine;

    public class JellyMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        
        [SerializeField] private float bounceSpeed = 1000;
        [SerializeField] private float fallForce = 1000;
        [SerializeField] private float stiffness = 100;

        private Mesh mesh;
        private JellyVertex[] jellyVertices;
        private Vector3[] currentMeshVertices;

        
        private void Awake()
        {
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }
            
            mesh = meshFilter.mesh;

            GetVertices();
        }

        private void Update()
        {
            UpdateVertices();
        }

        private void GetVertices()
        {
            jellyVertices = new JellyVertex[mesh.vertices.Length];
            currentMeshVertices = new Vector3[mesh.vertices.Length];

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                jellyVertices[i] = new JellyVertex(
                    i,
                    mesh.vertices[i],
                    mesh.vertices[i],
                    Vector3.zero);
                
                currentMeshVertices[i] = mesh.vertices[i];
            }
        }

        private void UpdateVertices()
        {
            for (int i = 0; i < jellyVertices.Length; i++)
            {
                jellyVertices[i].UpdateVelocity(bounceSpeed);
                jellyVertices[i].CalmVelocity(stiffness);

                jellyVertices[i].CurrentVertexPosition += jellyVertices[i].CurrentVelocity * Time.deltaTime;
                currentMeshVertices[i] = jellyVertices[i].CurrentVertexPosition;
            }

            mesh.vertices = currentMeshVertices;
            
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }

        private void ApplyPressureToPoint(Vector3 point, float pressure)
        {
            for (int i = 0; i < jellyVertices.Length; i++)
            {
                jellyVertices[i].ApplyPressureToVertex(transform, point, pressure);
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            ContactPoint[] collisionPoints = other.contacts;
            for (int i = 0; i < collisionPoints.Length; i++)
            {
                Vector3 inputPoint = collisionPoints[i].point + (collisionPoints[i].point * 0.1f);
                ApplyPressureToPoint(inputPoint, fallForce);
            }
        }

        private void OnMouseDown()
        {
            if (Camera.main is { })
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
                RaycastHit hit = new RaycastHit();
 
                if (Physics.Raycast (ray, out hit))
                {
                    Vector3 inputPoint = hit.point + (hit.point * 0.1f);
                    ApplyPressureToPoint(inputPoint, fallForce);
                }
            }
        }
    }
}
