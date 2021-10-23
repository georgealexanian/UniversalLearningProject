namespace MiniProjects.MP_MeshDeformations.Scripts.Game
{
    using System;
    using UnityEngine;

    public class MeshDeformerInput : MonoBehaviour {

        [SerializeField] private float force = 10f;
        [SerializeField] private float forceOffset = 0.1f;

        private Camera mainCamera;


        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update () 
        {
            if (Input.GetMouseButton(0)) {
                HandleInput();
            }
        }
        
        private void HandleInput () 
        {
            Ray inputRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(inputRay, out hit)) {
                MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
                
                if (deformer) {
                    Vector3 point = hit.point;
                    point += hit.normal * forceOffset;
                    deformer.AddDeformingForce(point, force);
                }
            }
        }
    }
}