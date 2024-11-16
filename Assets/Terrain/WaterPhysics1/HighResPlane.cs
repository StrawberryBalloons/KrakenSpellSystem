using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class HighResPlane : MonoBehaviour
{
    public int baseResolution = 256; // Base resolution of the mesh
    public int lodLevels = 3; // Number of LOD levels
    public float lodDistance = 50f; // Distance at which the LOD level changes

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        Generate(baseResolution);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, mainCamera.transform.position);
        int resolution = CalculateLODResolution(distance);
        if (resolution != mesh.vertexCount)
        {
            Generate(resolution);
        }
    }

    int CalculateLODResolution(float distance)
    {
        for (int i = 0; i < lodLevels; i++)
        {
            if (distance < (i + 1) * lodDistance)
            {
                return baseResolution >> i; // Divide resolution by 2^i
            }
        }
        return baseResolution >> lodLevels; // Lowest resolution
    }

    void Generate(int resolution)
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        triangles = new int[resolution * resolution * 6];

        float stepSize = 1.0f / resolution;

        for (int i = 0, z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++, i++)
            {
                vertices[i] = new Vector3(x * stepSize, 0, z * stepSize);
                uv[i] = new Vector2(x * stepSize, z * stepSize);
            }
        }

        for (int ti = 0, vi = 0, z = 0; z < resolution; z++, vi++)
        {
            for (int x = 0; x < resolution; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + resolution + 1;
                triangles[ti + 5] = vi + resolution + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
