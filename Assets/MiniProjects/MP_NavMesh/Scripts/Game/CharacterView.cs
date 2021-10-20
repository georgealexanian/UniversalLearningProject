namespace MiniProjects.MP_NavMesh.Scripts.Game
{
    using System;
    using UnityEngine;
    using UnityEngine.AI;

    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;

        private Camera mainCamera;


        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            MovePlayer(GetPositionOnArea());
        }

        private Vector3? GetPositionOnArea()
        {
            Vector3? point = null;
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    point = hit.point;
                }
            }
            
            return point;
        }

        private void MovePlayer(Vector3? point)
        {
            if (point.HasValue)
            {
                navMeshAgent.SetDestination(point.GetValueOrDefault());
            }
        }
    }
}
