using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using System.Collections;

[CustomEditor(typeof(NSG.MultiTerrainCreator))]
public class MultiTerrainCreatorEditor : Editor
{
    private EditorCoroutine terrainCoroutine;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NSG.MultiTerrainCreator creator = (NSG.MultiTerrainCreator)target;

        if (GUILayout.Button("Generate Terrains (Editor Coroutine)"))
        {
            if (terrainCoroutine != null)
                EditorCoroutineUtility.StopCoroutine(terrainCoroutine);

            terrainCoroutine = EditorCoroutineUtility.StartCoroutineOwnerless(CreateTerrainChunksCoroutine(creator));
        }
    }

    private IEnumerator CreateTerrainChunksCoroutine(NSG.MultiTerrainCreator creator)
    {

        TerrainData baseData = new TerrainData
        {
            heightmapResolution = NSG.MultiTerrainCreator.terrainHeightMapResolution,
            baseMapResolution = NSG.MultiTerrainCreator.terrainBaseTextureResolution,
            size = NSG.MultiTerrainCreator.terrainSize
        };

        for (int z = 0; z < creator.terrainGridSize; z++)
        {
            for (int x = 0; x < creator.terrainGridSize; x++)
            {
                TerrainData instanceData = Instantiate(baseData);
                GameObject terrain = Terrain.CreateTerrainGameObject(instanceData);
                Undo.RegisterCreatedObjectUndo(terrain, "Create Terrain Chunk");
                terrain.transform.SetParent(creator.transform);

                float xPos = NSG.MultiTerrainCreator.terrainSize.x * x;
                float zPos = NSG.MultiTerrainCreator.terrainSize.z * z;
                terrain.transform.position = new Vector3(xPos, 0, zPos);

                yield return null; // Spread over frames
            }
        }

        Debug.Log($"Created {creator.terrainGridSize * creator.terrainGridSize} terrain chunks ({creator.terrainGridSize} x {creator.terrainGridSize})");
    }
}