using UnityEngine;

namespace MP_ObjectTransparency.Scripts.Game
{
    using System;
    using UnityEngine.SocialPlatforms;

    public class ObjectTransparency : MonoBehaviour
    {
        [SerializeField] private Renderer materialRenderer;
        
        [Range(0f, 1f)]
        [SerializeField] private float alpha;

        private Material material;
        

        private void Awake()
        {
            material = materialRenderer.material;
        }

        private void OnValidate()
        {
            ChangeMaterialAlpha();
        }

        private void ChangeMaterialAlpha()
        {
            var mat = materialRenderer.material;
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            mat.color = newColor;
        }
    }
}
