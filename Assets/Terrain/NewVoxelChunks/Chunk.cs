using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel
{
    public float density;
    public int materialType; // Example: 0 = Air, 1 = Dirt, 2 = Stone, etc.
}

public class Chunk
{
    public Voxel[,,] voxels;
    public Vector3Int position;
    public Biome[] biomes;

    public Chunk(int width, int height, int depth, Vector3Int position, Biome[] biomes)
    {
        this.position = position;
        this.biomes = biomes;
        voxels = new Voxel[width, height, depth];

        // Initialize voxel densities using the terrain generator
        float[,,] densities = TerrainGenerator.GenerateTerrain(this, biomes, width, 2f); // 2f is the blend distance

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    voxels[x, y, z] = new Voxel { density = densities[x, y, z], materialType = 1 }; // Default material type
                }
            }
        }
    }
}
