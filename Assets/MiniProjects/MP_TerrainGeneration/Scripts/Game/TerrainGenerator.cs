namespace MiniProjects.MP_TerrainGeneration.Scripts.Game
{
    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private int width = 256;
        [SerializeField] private int height = 256;
        [SerializeField] private int depth = 20;
        
        [SerializeField] private float scale = 20f;
        [SerializeField] private float offsetX = 100f;
        [SerializeField] private float offsetY = 100f;

        [SerializeField] private bool updateOnce = true;
        
        [SerializeField] private float updateSpeed = 10f;
        [SerializeField] private bool updateX = true;
        [SerializeField] private bool updateY = true;

        private Terrain terrain;
        private TerrainData terrainData;
        

        private void Awake()
        {
            terrain = GetComponent<Terrain>();
            terrainData = terrain.terrainData;

            RandomizeOffsets();

            if (updateOnce)
            {
                terrainData = GenerateTerrainDate(terrainData);
            }
        }

        private void Update()
        {
            if (!updateOnce)
            {
                ScaleOffsets();
                terrainData = GenerateTerrainDate(terrainData);
            }
        }

        private void RandomizeOffsets()
        {
            offsetX = Random.Range(0f, 9999f);
            offsetY = Random.Range(0f, 9999f);
        }

        private void ScaleOffsets()
        {
            if (updateX)
            {
                offsetX += Time.deltaTime * updateSpeed;
            }
            if (updateY)
            {
                offsetY += Time.deltaTime * updateSpeed;
            }
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
            float xCoord = (float) x / width * scale + offsetX;
            float yCoord = (float) y / height * scale + offsetY;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }
    }
}
