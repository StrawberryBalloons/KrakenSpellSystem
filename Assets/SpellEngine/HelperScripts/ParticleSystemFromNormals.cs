using UnityEngine;

public class ParticleSystemFromNormals : MonoBehaviour
{
    public GameObject particleSystemPrefab;

    void Start()
    {
        if (particleSystemPrefab == null)
        {
            Debug.LogError("Particle system prefab is not assigned!");
            Destroy(this); // Remove this script if the prefab is not assigned
            return;
        }

        // Instantiate the particle system prefab
        GameObject psObject = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);

        // Parent the particle system to the current GameObject
        psObject.transform.parent = transform;

        // Ensure the particle system plays
        ParticleSystem ps = psObject.GetComponent<ParticleSystem>();
        ps.Play();

        // Emit particles from the normals of the mesh
        var emission = ps.emission;
        emission.rateOverTime = 100; // Set the emission rate

        // Set the shape to emit from mesh normals
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Mesh;
        shape.meshRenderer = GetComponent<MeshRenderer>();
        shape.meshShapeType = ParticleSystemMeshShapeType.Vertex;
    }
}
