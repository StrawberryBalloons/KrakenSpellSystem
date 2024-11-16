using UnityEngine;

public class MeshGrassCover : MonoBehaviour
{
    public Sprite grassSprite; // Sprite for the grass particle
    public int maxParticles = 1000; // Maximum number of particles
    public Color[] grassColors; // Array of colors to randomly choose from
    [Range(0f, 1f)]
    public float coveragePercentage = 0.5f; // Percentage of particles to modify
    public float scaleMultiplier = 1f; // Scale multiplier for the grass particles

    private GameObject[] grassParticles; // Array to store instantiated grass particles

    void Start()
    {
        if (grassSprite == null)
        {
            Debug.LogError("Grass Sprite not assigned!");
            enabled = false;
            return;
        }

        // Initialize array for grass particles
        grassParticles = new GameObject[maxParticles];

        // Get mesh from the object this script is attached to
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        if (mesh == null)
        {
            Debug.LogError("Mesh not found on the object!");
            enabled = false;
            return;
        }

        // Generate grass particles on mesh vertices
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < maxParticles; i++)
        {
            Vector3 position = transform.TransformPoint(vertices[i % vertices.Length]);
            GameObject grass = new GameObject("GrassParticle");
            grass.transform.position = position;
            grass.transform.parent = transform;
            SpriteRenderer renderer = grass.AddComponent<SpriteRenderer>();
            renderer.sprite = grassSprite;
            renderer.color = grassColors[Random.Range(0, grassColors.Length)];
            grass.transform.localScale *= scaleMultiplier;
            grassParticles[i] = grass;
        }
    }

    void Update()
    {
        // Calculate how many particles to modify based on coverage percentage
        int particlesToModify = Mathf.RoundToInt(maxParticles * coveragePercentage);

        for (int i = 0; i < particlesToModify; i++)
        {
            // Randomly select a grass particle
            GameObject particle = grassParticles[Random.Range(0, maxParticles)];

            // Change color randomly
            // SpriteRenderer spriteRenderer = particle.GetComponent<SpriteRenderer>();
            // if (spriteRenderer != null)
            // {
            //     spriteRenderer.color = grassColors[Random.Range(0, grassColors.Length)];
            // }
        }
    }
}
