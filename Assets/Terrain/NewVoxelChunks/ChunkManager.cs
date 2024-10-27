using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public int chunkWidth = 64;
    public int chunkHeight = 8;
    public int chunksInX = 4;
    public int chunksInY = 1; // Added for vertical chunks
    public int chunksInZ = 4;
    public Material[] materials;

    private GameObject[,,] chunks;

    void Start()
    {
        chunks = new GameObject[chunksInX, chunksInY, chunksInZ];
        GenerateChunks();
    }

    void GenerateChunks()
    {
        for (int x = 0; x < chunksInX; x++)
        {
            for (int y = 0; y < chunksInY; y++)
            {
                for (int z = 0; z < chunksInZ; z++)
                {
                    Vector3Int chunkPosition = new Vector3Int(x * chunkWidth, y * chunkHeight, z * chunkWidth);
                    GameObject chunk = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity);
                    chunk.transform.SetParent(transform);

                    ChunkRenderer chunkRenderer = chunk.GetComponent<ChunkRenderer>();
                    if (chunkRenderer != null)
                    {
                        chunkRenderer.chunkWidth = chunkWidth;
                        chunkRenderer.chunkHeight = chunkHeight;
                        chunkRenderer.Initialize(chunkPosition, this);
                    }

                    chunks[x, y, z] = chunk;
                }
            }
        }
        //Configure borders

        //Render
    }

    public GameObject GetChunk(Vector3Int chunkPosition)
    {
        Vector3Int gridPosition = ChunkToGrid(chunkPosition);
        if (IsInGridBounds(gridPosition))
        {
            return chunks[gridPosition.x, gridPosition.y, gridPosition.z];
        }

        return null;
    }

    public Material GetMaterial(Vector3Int location)
    {
        // Implement logic to return the correct material based on location.
        // This is a placeholder example.
        int materialIndex = 0; // Determine material index based on location
        return materials[materialIndex];
    }

    public void ModifyVoxelValues(Vector3 position, float strength, float distance)
    {
        List<Vector3Int> positions = GetPositionsInRadius(position, distance);

        HashSet<Vector3Int> affectedChunks = new HashSet<Vector3Int>();
        foreach (var pos in positions)
        {
            Vector3Int chunkPosition = WorldToChunk(pos);
            GameObject chunkObj = GetChunk(chunkPosition);
            if (chunkObj != null)
            {
                ChunkRenderer chunkRenderer = chunkObj.GetComponent<ChunkRenderer>();
                if (chunkRenderer != null)
                {
                    Vector3Int voxelPos = WorldToLocal(pos);

                    var voxelData = chunkRenderer.GetVoxelData();
                    if (IsInBounds(voxelPos, voxelData))
                    {
                        voxelData[voxelPos.x, voxelPos.y, voxelPos.z].value = Mathf.Max(0, voxelData[voxelPos.x, voxelPos.y, voxelPos.z].value - strength);
                        affectedChunks.Add(chunkPosition);
                    }
                }
            }
        }

        foreach (var chunkPos in affectedChunks)
        {
            GameObject chunkObj = GetChunk(chunkPos);
            if (chunkObj != null)
            {
                ChunkRenderer chunkRenderer = chunkObj.GetComponent<ChunkRenderer>();
                if (chunkRenderer != null)
                {
                    chunkRenderer.UpdateVoxelData(chunkRenderer.GetVoxelData());
                }
            }
        }
    }

    private Vector3Int WorldToChunk(Vector3Int position)
    {
        return new Vector3Int(
            Mathf.FloorToInt(position.x / (float)chunkWidth) * chunkWidth,
            Mathf.FloorToInt(position.y / (float)chunkHeight) * chunkHeight,
            Mathf.FloorToInt(position.z / (float)chunkWidth) * chunkWidth
        );
    }

    private Vector3Int ChunkToGrid(Vector3Int chunkPosition)
    {
        return new Vector3Int(
            chunkPosition.x / chunkWidth,
            chunkPosition.y / chunkHeight,
            chunkPosition.z / chunkWidth
        );
    }

    private Vector3Int WorldToLocal(Vector3Int point)
    {
        return new Vector3Int(
            point.x % chunkWidth,
            point.y % chunkHeight,
            point.z % chunkWidth
        );
    }

    private bool IsInBounds(Vector3Int pos, VoxelData[,,] voxelData)
    {
        return pos.x >= 0 && pos.x < voxelData.GetLength(0) &&
               pos.y >= 0 && pos.y < voxelData.GetLength(1) &&
               pos.z >= 0 && pos.z < voxelData.GetLength(2);
    }

    private bool IsInGridBounds(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < chunksInX &&
               pos.y >= 0 && pos.y < chunksInY &&
               pos.z >= 0 && pos.z < chunksInZ;
    }

    public List<Vector3Int> GetPositionsInRadius(Vector3 center, float radius)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        int minX = Mathf.FloorToInt(center.x - radius);
        int maxX = Mathf.CeilToInt(center.x + radius);
        int minY = Mathf.FloorToInt(center.y - radius);
        int maxY = Mathf.CeilToInt(center.y + radius);
        int minZ = Mathf.FloorToInt(center.z - radius);
        int maxZ = Mathf.CeilToInt(center.z + radius);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int z = minZ; z <= maxZ; z++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);
                    if (Vector3.Distance(center, position) <= radius)
                    {
                        positions.Add(position);
                    }
                }
            }
        }

        return positions;
    }

    public int[,,] GetChunkMaterialIndices(Vector3Int chunkPosition)
    {
        GameObject chunkObj = GetChunk(chunkPosition);
        if (chunkObj != null)
        {
            ChunkRenderer chunkRenderer = chunkObj.GetComponent<ChunkRenderer>();
            if (chunkRenderer != null)
            {
                VoxelData[,,] voxelData = chunkRenderer.GetVoxelData();
                int width = voxelData.GetLength(0);
                int height = voxelData.GetLength(1);
                int depth = voxelData.GetLength(2);

                int[,,] materialIndices = new int[width, height, depth];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            materialIndices[x, y, z] = voxelData[x, y, z].material;
                        }
                    }
                }

                return materialIndices;
            }
        }

        Debug.LogError($"Chunk at position {chunkPosition} not found.");
        return null;
    }
}
