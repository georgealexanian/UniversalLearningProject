namespace MiniProjects.MP_DrawingUsingGameObjectInstantiation.Scripts.Game
{
    using System;
    using Unity.Mathematics;
    using UnityEngine;

    public class Paintable : MonoBehaviour
    {
        [SerializeField] private GameObject brush;
        [SerializeField] private float brushSize;

        private Camera mainCamera;
        private Vector3 lastDrawPosition;


        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                CalculateDrawPosition(() =>
                {
                    Draw();
                });
            }
        }

        private void CalculateDrawPosition(Action callback)
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                lastDrawPosition = raycastHit.point + Vector3.up * 0.1f;
                
                callback.Invoke();
            }
        }

        private void Draw()
        {
            var drawInstance = Instantiate(
                brush,
                lastDrawPosition, 
                quaternion.identity,
                transform);
            drawInstance.transform.localScale = Vector3.one * brushSize;
        }
    }
}
