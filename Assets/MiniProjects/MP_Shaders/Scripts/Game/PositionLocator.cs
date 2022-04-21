namespace MiniProjects.MP_Shaders.Scripts.Game
{
    using UnityEngine;

    public class PositionLocator : MonoBehaviour
    {
        [SerializeField] private Material material;
        
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector4 position = new Vector4(hit.point.x, hit.point.y, hit.point.z, Time.time);
                    material.SetVector("_Position", position);
                }
            }
        }
    }
}
