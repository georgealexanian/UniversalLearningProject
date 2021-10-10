namespace MP_JellyMesh.Scripts.Game
{
    using UnityEngine;

    public class JellyVertex
    {
        private int vertexIndex;
        private readonly Vector3 initialVertexPosition;
        public Vector3 CurrentVertexPosition;
        public Vector3 CurrentVelocity;


        public JellyVertex(int vertexIndex, Vector3 initialVertexPosition, Vector3 currentVertexPosition, Vector3 currentVelocity)
        {
            this.vertexIndex = vertexIndex;
            this.initialVertexPosition = initialVertexPosition;
            CurrentVertexPosition = currentVertexPosition;
            CurrentVelocity = currentVelocity;
        }

        private Vector3 GetCurrentDisplacement()
        {
            return CurrentVertexPosition - initialVertexPosition;
        }

        public void UpdateVelocity(float bounceSpeed)
        {
            CurrentVelocity -= GetCurrentDisplacement() * (bounceSpeed * Time.deltaTime);
        }

        public void CalmVelocity(float stiffness)
        {
            CurrentVelocity *= 1f - stiffness * Time.deltaTime;
        }

        public void ApplyPressureToVertex(Transform tr, Vector3 position, float pressure)
        {
            Vector3 distanceToHitPoint = CurrentVertexPosition - tr.InverseTransformPoint(position);
            float adaptedPressure = pressure / (1f * distanceToHitPoint.sqrMagnitude);
            float velocity = adaptedPressure * Time.deltaTime;
            CurrentVelocity += distanceToHitPoint.normalized * velocity;
        }
    }
}
