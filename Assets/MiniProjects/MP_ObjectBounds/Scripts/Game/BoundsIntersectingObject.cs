namespace MiniProjects.MP_ObjectBounds.Scripts.Game
{
    using UnityEngine;

    public class BoundsIntersectingObject : MonoBehaviour
    {
        [SerializeField] BoxCollider boxCollider;
        
        private void OnDrawGizmosSelected()
        {
            //the commented code below works incorrectly since the bounds do not update according to changes of rotation.
            // var position = transform.position;
            // bool isPointInside = boxCollider.bounds.Contains(position);
            
            //this works
            Vector3 pointBoxSpace = boxCollider.transform.InverseTransformPoint(transform.position);
            Bounds correctBounds = new Bounds(boxCollider.center, boxCollider.size);
            bool isPointInside = correctBounds.Contains(pointBoxSpace);
            //
            
            Gizmos.color = isPointInside ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, transform.localScale.x / 2);
        }
    }
}
