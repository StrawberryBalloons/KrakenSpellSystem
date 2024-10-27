using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesTerrain : MonoBehaviour
{
    public int chunkSize = 16;  // Adjust the chunk size as needed
    public float isoLevel = 0.5f;  // Adjust the iso level as needed

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Mesh mesh = MarchingCubesAlgorithm.GenerateMesh(chunkSize, isoLevel);

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));  // Replace with your desired shader
    }
}

public static class MarchingCubesAlgorithm
{
    public static Mesh GenerateMesh(int chunkSize, float isoLevel)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Marching Cubes Mesh";

        Vector3[] vertices = new Vector3[chunkSize * chunkSize * chunkSize];
        int[] triangles = new int[chunkSize * chunkSize * chunkSize * 15];  // Each voxel can contribute up to 15 triangles
        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    float density = CalculateDensity(x, y, z);

                    vertices[vertexIndex] = new Vector3(x, y, z);

                    // Retrieve the triangles for the specified configuration
                    int[] voxelTriangles = MarchingCubesTable.GetTriangles(x, y, z, isoLevel);

                    // Populate the triangles array
                    for (int i = 0; i < voxelTriangles.Length; i++)
                    {
                        triangles[triangleIndex++] = voxelTriangles[i] + vertexIndex;
                    }

                    vertexIndex++;
                }
            }
        }

        // Trim the arrays to match the actual number of vertices and triangles
        Vector3[] trimmedVertices = new Vector3[vertexIndex];
        int[] trimmedTriangles = new int[triangleIndex];

        System.Array.Copy(vertices, trimmedVertices, trimmedVertices.Length);
        System.Array.Copy(triangles, trimmedTriangles, trimmedTriangles.Length);

        mesh.vertices = trimmedVertices;
        mesh.triangles = trimmedTriangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    static float CalculateDensity(float x, float y, float z)
    {
        // Implement your own density calculation based on Perlin noise or other functions
        // For simplicity, we'll use a basic formula here
        float density = GeneratePerlin(x, y, z);
        return density;
    }

    public static float GeneratePerlin(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);

        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        return (xy + xz + yz + yx + zx + zy) / 6f;
    }
}