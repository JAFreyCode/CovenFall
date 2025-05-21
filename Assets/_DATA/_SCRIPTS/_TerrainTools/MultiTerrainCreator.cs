using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSG
{
    public class MultiTerrainCreator : MonoBehaviour
    {
        [Header("Generate Terrain")]
        public bool generateTerrain;
        public bool clearTerrainList;
        public int terrainGridSize;

        [Header("Terrain Settings")]
        static public Vector3 terrainSize = new Vector3(1000, 10000, 1000);
        [Tooltip("can be 16, 32, 64, 128, 256, 512, 1024, 2048, 4096")]
        static public int terrainHeightMapResolution = 4096;
        [Tooltip("can be 16, 32, 64, 128, 256, 512, 1024, 2048, 4096")]
        static public int terrainBaseTextureResolution = 2048;

        private Coroutine terrainCreationCoroutine;

        private void OnValidate()
        {
            if (generateTerrain)
            {
                generateTerrain = false;

                // Only works in Play mode!
                if (Application.isPlaying)
                {
                    if (terrainCreationCoroutine != null)
                        StopCoroutine(terrainCreationCoroutine);

                    terrainCreationCoroutine = StartCoroutine(CreateTerrainChunksCoroutine());
                }
                else
                {
                    Debug.LogWarning("Terrain generation coroutine only works in Play mode.");
                }
            }

            if (clearTerrainList)
            {
                clearTerrainList = false;
            }
        }

        public IEnumerator CreateTerrainChunksCoroutine()
        {

            TerrainData baseData = new TerrainData
            {
                heightmapResolution = terrainHeightMapResolution,
                baseMapResolution = terrainBaseTextureResolution,
                size = terrainSize
            };

            for (int z = 0; z < terrainGridSize; z++)
            {
                for (int x = 0; x < terrainGridSize; x++)
                {
                    TerrainData instanceData = ScriptableObject.Instantiate(baseData);
                    GameObject terrain = Terrain.CreateTerrainGameObject(instanceData);
                    terrain.transform.SetParent(transform);

                    // Position the terrain immediately
                    float xPos = terrainSize.x * x;
                    float zPos = terrainSize.z * z;
                    terrain.transform.position = new Vector3(xPos, 0, zPos);

                    // Yield after each terrain to spread the load
                    yield return null;
                }
            }

            Debug.Log($"Created {terrainGridSize * terrainGridSize} terrain chunks ({terrainGridSize} x {terrainGridSize})");
        }

    }
}