namespace MiniProjects.MP_Shaders.Scripts.Game
{
    using System;
    using UnityEngine;

    public class AssignTexture : MonoBehaviour
    {
        public ComputeShader computeShader;
        public int textureResolution = 256;
        public string kernelName;
        
        private Renderer rend;
        private RenderTexture outputTexture;
        private int kernelHandle;


        private void Start()
        {
            outputTexture = new RenderTexture(textureResolution, textureResolution, 0);
            outputTexture.enableRandomWrite = true;
            outputTexture.Create();

            rend = GetComponent<Renderer>();
            rend.enabled = true; 
            
            InitShader();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.U))
            {
                DispatchShader(textureResolution / 8, textureResolution / 8);
            }
        }

        private void InitShader()
        {
            kernelHandle = computeShader.FindKernel(kernelName);
            computeShader.SetInt("textureResolution", textureResolution);
            
            computeShader.SetTexture(kernelHandle, "Result", outputTexture);
            rend.material.SetTexture("_MainTex", outputTexture);
            
            DispatchShader(textureResolution / 16, textureResolution / 16);
        }

        private void DispatchShader(int x, int y)
        {
            computeShader.Dispatch(kernelHandle, x, y, 1);
        }
    }
}
