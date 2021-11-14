namespace MiniProjects.MP_TerrainGeneration.Scripts.Game
{
    using UnityEngine;

    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private int width = 256;
        [SerializeField] private int height = 256;
        [SerializeField] private int depth = 20;

        [SerializeField] private float scale = 20f;

        private Terrain terrain;
        private TerrainData terrainData;


        private void Awake()
        {
            terrain = GetComponent<Terrain>();
            terrainData = terrain.terrainData;

            terrainData = GenerateTerrainDate(terrainData);
        }

        private TerrainData GenerateTerrainDate(TerrainData terData)
        {
            terData.heightmapResolution = width + 1;
            
            terData.size = new Vector3(width, depth, height);
            terData.SetHeights(0, 0, GenerateTerrainHeights());

            return terData; 
        }

        private float[,] GenerateTerrainHeights()
        {
            float[,] heights = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heights[x, y] = CalculateHeight(x, y);
                }
            }

            return heights;
        }

        private float CalculateHeight(int x, int y)
        {
            float xCoord = (float) x / width * scale;
            float yCoord = (float) y / height * scale;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }
    }
}
