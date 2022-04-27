namespace MiniProjects.MP_RayTracing.Scripts.Game
{
    using UnityEngine;

    public class RayTracer : MonoBehaviour
    {
        [SerializeField] private ComputeShader rayTracingShader;

        private RenderTexture target;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Render(destination);
        }

        private void Render(RenderTexture destination)
        {
            Init();
            
            rayTracingShader.SetTexture(0, "Result", target);
            int threadGroupX = Mathf.CeilToInt(Screen.width / 8.0f);
            int threadGroupY = Mathf.CeilToInt(Screen.height / 8.0f);
            rayTracingShader.Dispatch(0, threadGroupX, threadGroupY, 1);
            
            Graphics.Blit(target, destination);
        }

        private void Init()
        {
            if (target == null || target.width != Screen.width || target.height != Screen.height)
            {
                target.Release();
                target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat);
                target.enableRandomWrite = true;
                target.Create();
            }
        }
    }
}