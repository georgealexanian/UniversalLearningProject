namespace MiniProjects.MP_Shaders.Scripts.Game
{
    using UnityEngine;

    [ExecuteInEditMode]
    public class DepthReplacementShaderEffect : MonoBehaviour
    {
        [SerializeField] private Shader replacementShader;

        private void OnEnable()
        {
            GetComponent<Camera>().SetReplacementShader(replacementShader, "");
        }

        private void OnDisable()
        {
            GetComponent<Camera>().ResetReplacementShader();
        }
    }
}
