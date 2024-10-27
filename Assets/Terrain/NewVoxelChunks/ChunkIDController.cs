using UnityEngine;

public class ChunkIDController : MonoBehaviour
{
    public Vector3 chunkPosition;
    public int[,,] idData; // This should be assigned with your chunk's ID data

    private Renderer rend;
    private Texture3D idMap;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("Renderer not found!");
            return;
        }

        if (idData != null)
        {
            int width = idData.GetLength(0);
            int height = idData.GetLength(1);
            int depth = idData.GetLength(2);

            idMap = new Texture3D(width, height, depth, TextureFormat.R8, false);
            Color[] ids = new Color[width * height * depth];

            int index = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        ids[index] = new Color(idData[x, y, z] / 255.0f, 0, 0, 0); // Store ID as a normalized float in the red channel
                        index++;
                    }
                }
            }

            idMap.SetPixels(ids);
            idMap.Apply();

            rend.material.SetTexture("_IDMap", idMap);
            rend.material.SetVector("_ChunkPosition", chunkPosition);
        }
        else
        {
            Debug.LogError("ID data is not assigned!");
        }
    }

    void OnUpdate()
    {
        if (rend != null && idMap != null)
        {
            rend.material.SetVector("_ChunkPosition", chunkPosition);
        }
    }
}
