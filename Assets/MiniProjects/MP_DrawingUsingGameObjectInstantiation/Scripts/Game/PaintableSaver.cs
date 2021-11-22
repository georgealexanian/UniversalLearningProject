namespace MiniProjects.MP_DrawingUsingGameObjectInstantiation.Scripts.Game
{
    using System.Collections;
    using System.IO;
    using UnityEngine;

    public class PaintableSaver : MonoBehaviour
    {
        [SerializeField] private RenderTexture paintedTexture;
        [SerializeField] private SaveType saveType = SaveType.PNG;

        
        public void SaveDrawing()
        {
            StartCoroutine(Save());
        }

        private IEnumerator Save()
        {
            Debug.Log(Application.dataPath);
            
            yield return new WaitForEndOfFrame();

            RenderTexture.active = paintedTexture;
            EncodeAndSave(SampleTexture2D());
        }

        private Texture2D SampleTexture2D()
        {
            var texture2D = new Texture2D(paintedTexture.width, paintedTexture.height, TextureFormat.RGB24, false);
            texture2D.ReadPixels(new Rect(0, 0, paintedTexture.width, paintedTexture.height), 0, 0);
            texture2D.Apply();
            return texture2D;
        }

        private void EncodeAndSave(Texture2D texture2D)
        {
            var encodedData = saveType == SaveType.PNG 
                ? texture2D.EncodeToPNG() : saveType == SaveType.JPG 
                    ? texture2D.EncodeToJPG() 
                    : texture2D.EncodeToPNG();
            var format = saveType == SaveType.PNG 
                ? "png" : saveType == SaveType.JPG 
                    ? "jpg"
                    : "png";
            File.WriteAllBytes(Application.dataPath + "/Painting." + format, encodedData);
        }
    }
}
