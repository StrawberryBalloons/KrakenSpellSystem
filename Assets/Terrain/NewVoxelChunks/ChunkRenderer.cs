using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VoxelData
{
    public float value;
    public int material;
}

public class ChunkRenderer : MonoBehaviour
{
    [System.Serializable]
    public struct BiomeSettings
    {
        public string biomeName;
        public float adjust;
        public SimplexNoiseDetails[] simplexNoise;
        public int material;
        public int minHeight; // Minimum height for the biome
        public int maxHeight; // Maximum height for the biome
    }

    public int chunkWidth = 64;
    public int chunkHeight = 8;
    public Material customMaterial;
    public BiomeSettings[] biomes;

    private VoxelData[,,] voxelData;
    private ChunkMarching chunkMarching;
    private ChunkManager chunkManager;
    public GameObject cubePrefab; // Assign the cube prefab in the Unity Editor

    public void Initialize(Vector3Int chunkPosition, ChunkManager manager)
    {
        voxelData = new VoxelData[chunkWidth + 2, chunkHeight + 2, chunkWidth + 2];
        chunkMarching = GetComponent<ChunkMarching>();
        chunkMarching.width = chunkWidth;
        chunkMarching.height = chunkHeight;
        chunkManager = manager;

        if (chunkMarching == null)
        {
            Debug.LogError("ChunkMarching component is missing!");
            return;
        }

        GenerateVoxelData(chunkPosition);

        chunkMarching.SetChunkArray(voxelData);
    }

    private void GenerateVoxelData(Vector3Int chunkPosition)
    {
        for (int x = 0; x < chunkWidth + 2; x++)
        {
            for (int y = 0; y < chunkHeight + 2; y++)
            {
                for (int z = 0; z < chunkWidth + 2; z++)
                {
                    float voxelValue = 0f;

                    // Determine biome based on voxel position
                    Vector3 voxelPosition = new Vector3(x + chunkPosition.x, y + chunkPosition.y, z + chunkPosition.z);
                    BiomeSettings biome = GetBiome(voxelPosition);

                    foreach (var noiseDetails in biome.simplexNoise)
                    {
                        voxelValue += chunkMarching.GenerateSimplexNoise(
                            voxelPosition.x,
                            voxelPosition.y,
                            voxelPosition.z,
                            noiseDetails
                        ) + biome.adjust;

                        voxelValue = Mathf.Clamp(voxelValue, 0f, 1f);
                    }

                    voxelData[x, y, z].value = voxelValue;
                    voxelData[x, y, z].material = biome.material;
                }
            }
        }
    }

    private BiomeSettings GetBiome(Vector3 voxelPosition)
    {
        foreach (var biome in biomes)
        {
            if (voxelPosition.y >= biome.minHeight && voxelPosition.y < biome.maxHeight)
            {
                return biome;
            }
        }

        // Fallback to the first biome if none match
        Debug.LogWarning("No biome found for the given position. Using default biome.");
        return biomes[0];
    }

    public VoxelData[,,] GetVoxelData()
    {
        return voxelData;
    }

    public VoxelData SampleNeighbor(Vector3Int neighborChunkPosition, Vector3Int localPosition)
    {
        GameObject neighborChunk = chunkManager.GetChunk(neighborChunkPosition);
        if (neighborChunk != null)
        {
            ChunkRenderer neighborRenderer = neighborChunk.GetComponent<ChunkRenderer>();
            if (neighborRenderer != null)
            {
                return neighborRenderer.GetVoxelData()[localPosition.x, localPosition.y, localPosition.z];
            }
        }
        return new VoxelData { value = 0 }; // Default value if neighbor does not exist
    }

    public void UpdateVoxelData(VoxelData[,,] newVoxelData)
    {
        voxelData = newVoxelData;
        chunkMarching.SetChunkArray(voxelData);
    }

    public Vector3 GetVoxelPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z);
    }

    public void SetVoxelData(VoxelData[,,] data)
    {
        voxelData = data;
    }

    public void UpdateMesh()
    {
        GetComponent<ChunkMarching>().SetChunkArray(voxelData);
        GetComponent<ChunkMarching>().ApplyMeshUpdate(); // Ensure this method exists or equivalent
    }
}
