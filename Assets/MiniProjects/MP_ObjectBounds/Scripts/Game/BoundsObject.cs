namespace MiniProjects.MP_ObjectBounds.Scripts.Game
{
    using UnityEngine;

    public class BoundsObject : MonoBehaviour
    {
        [SerializeField] BoxCollider boxCollider;

        private void OnDrawGizmosSelected()
        {
            var bounds = boxCollider.bounds;

            Gizmos.color = new Color(0.1f, 0.2f, 0.8f, 0.5f);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
    }
}
