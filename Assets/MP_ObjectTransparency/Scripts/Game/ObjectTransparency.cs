using UnityEngine;

namespace MP_ObjectTransparency.Scripts.Game
{
    public class ObjectTransparency : MonoBehaviour
    {
        [SerializeField] private Renderer materialRenderer;
        
        [Range(0f, 1f)]
        [SerializeField] private float alpha;


        private void OnValidate()
        {
            ChangeMaterialAlpha();
        }

        private void ChangeMaterialAlpha()
        {
            var mat = materialRenderer.sharedMaterial;
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            mat.color = newColor;
        }
    }
}
