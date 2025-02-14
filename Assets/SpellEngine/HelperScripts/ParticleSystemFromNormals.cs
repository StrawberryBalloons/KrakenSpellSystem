using UnityEngine;

public class ParticleSystemFromNormals : MonoBehaviour
{
    public GameObject particleSystemPrefab;

    void Start()
    {
        if (particleSystemPrefab == null)
        {
            Debug.LogError("Particle system prefab is not assigned!");
            Destroy(this);
            return;
        }

        // Instantiate and attach particle system
        GameObject psObject = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);

        // Parent the particle system to the current GameObject
        psObject.transform.parent = transform;

        // Get the particle system and play it
        ParticleSystem ps = psObject.GetComponent<ParticleSystem>();
        if (ps == null)
        {
            Debug.LogError("Particle system prefab does not contain a ParticleSystem component!");
            return;
        }

        ps.Play();

        // Ensure there's a MeshFilter and assign it to the shape module
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.Log("No MeshFilter or mesh found on the object.");
            return;
        }

        var shape = ps.shape;
        shape.enabled = true; // Ensure shape module is enabled
        shape.shapeType = ParticleSystemShapeType.Mesh;
        shape.mesh = meshFilter.sharedMesh; // Use mesh from MeshFilter
        shape.meshShapeType = ParticleSystemMeshShapeType.Vertex;
    }
}
