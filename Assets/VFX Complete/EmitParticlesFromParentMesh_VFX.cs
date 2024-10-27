using UnityEngine;
using UnityEngine.VFX;

public class EmitParticlesFromParentMesh_VFX : MonoBehaviour
{
    private VisualEffect visualEffect;   // The VFX Graph component

    private void Start()
    {
        // Automatically get the VisualEffect component attached to this GameObject
        visualEffect = GetComponent<VisualEffect>();

        if (visualEffect == null)
        {
            Debug.LogError("No VisualEffect component found on this GameObject.");
            return;
        }

        // Try to get the mesh from the parent's MeshFilter or SkinnedMeshRenderer
        Mesh parentMesh = GetParentMesh();

        if (parentMesh != null)
        {
            // Assign the parent mesh to the VFX Graph's "Mesh" property
            visualEffect.SetMesh("Mesh", parentMesh);
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
