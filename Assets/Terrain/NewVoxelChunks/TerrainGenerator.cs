using UnityEngine;

public static class TerrainGenerator
{
    public static float[,,] GenerateTerrain(Chunk chunk, Biome[] biomes, int chunkSize, float blendDistance)
    {
        float[,,] densities = new float[chunkSize, chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    Vector3Int globalPosition = chunk.position + new Vector3Int(x, y, z);
                    densities[x, y, z] = CalculateDensity(globalPosition, biomes, blendDistance);
                }
            }
        }

        return densities;
    }

    private static float CalculateDensity(Vector3Int position, Biome[] biomes, float blendDistance)
    {
        float totalDensity = 0f;
        float totalWeight = 0f;

        foreach (Biome biome in biomes)
        {
            float noise = Mathf.PerlinNoise((position.x + biome.seed) * biome.frequency, (position.z + biome.seed) * biome.frequency);
            float height = noise * biome.amplitude;
            float distance = Mathf.Abs(position.y - height);
            float weight = Mathf.Max(0, blendDistance - distance) / blendDistance;

            totalDensity += noise * weight;
            totalWeight += weight;
        }

        return totalDensity / totalWeight;
    }
}
