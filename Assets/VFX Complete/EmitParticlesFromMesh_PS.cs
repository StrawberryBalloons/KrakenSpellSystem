using UnityEngine;

public class EmitParticlesFromParentMesh_PS : MonoBehaviour
{
    private ParticleSystem particleSystem;  // The Particle System component
    private ParticleSystem.ShapeModule shapeModule; // Shape module for controlling particle emission

    private void Start()
    {
        // Automatically get the ParticleSystem component attached to this GameObject
        particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem == null)
        {
            Debug.LogError("No Particle System component found on this GameObject.");
            return;
        }

        // Access the Shape module of the Particle System
        shapeModule = particleSystem.shape;

        // Try to get the mesh from the parent's MeshFilter or SkinnedMeshRenderer
        Mesh parentMesh = GetParentMesh();

        if (parentMesh != null)
        {
            // Assign the parent's mesh to the Particle System's shape mesh
            shapeModule.shapeType = ParticleSystemShapeType.Mesh;
            shapeModule.mesh = parentMesh;
        }
        else
        {
            Debug.LogError("Parent mesh not found.");
        }
    }

    // Function to get the parent's mesh
    private Mesh GetParentMesh()
    {
        MeshFilter meshFilter = GetComponentInParent<MeshFilter>();
        if (meshFilter != null)
        {
            return meshFilter.sharedMesh;
        }

        SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInParent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            return skinnedMeshRenderer.sharedMesh;
        }

        return null; // No mesh found on the parent
    }
}
