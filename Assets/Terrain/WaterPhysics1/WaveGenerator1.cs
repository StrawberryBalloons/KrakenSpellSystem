using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator1 : MonoBehaviour
{
    public ComputeShader waveComputeShader;
    public List<Vector4> waveParameters; // (amplitude, wavelength, speed, direction angle)

    private Mesh mesh;
    private Vector3[] vertices;
    private ComputeBuffer vertexBuffer;
    private ComputeBuffer waveParamBuffer;
    private int kernelHandle;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        // Initialize compute buffers
        vertexBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);
        vertexBuffer.SetData(vertices);

        waveParamBuffer = new ComputeBuffer(waveParameters.Count, sizeof(float) * 4);
        waveParamBuffer.SetData(waveParameters.ToArray());

        kernelHandle = waveComputeShader.FindKernel("CSMain");

        waveComputeShader.SetBuffer(kernelHandle, "vertices", vertexBuffer);
        waveComputeShader.SetBuffer(kernelHandle, "waveParameters", waveParamBuffer);
    }

    void Update()
    {
        float time = Time.time;
        waveComputeShader.SetFloat("time", time);
        int threadGroups = Mathf.CeilToInt(Mathf.Sqrt(vertices.Length) / 8.0f);
        waveComputeShader.Dispatch(kernelHandle, threadGroups, threadGroups, 1);

        vertexBuffer.GetData(vertices);
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    void OnDestroy()
    {
        vertexBuffer.Release();
        waveParamBuffer.Release();
    }
}
