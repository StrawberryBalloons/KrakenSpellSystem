using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGeneratorGPU : MonoBehaviour
{
    public ComputeShader waveComputeShader;
    public int resolution = 256;
    public float waveSpeed = 1.0f;
    public float waveHeight = 1.0f;
    public float waveLength = 10.0f;

    private ComputeBuffer waveBuffer;
    private Vector3[] vertices;
    private Mesh mesh;
    private int kernelHandle;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = new Vector3[mesh.vertexCount];
        waveBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);

        kernelHandle = waveComputeShader.FindKernel("CSMain");

        waveComputeShader.SetFloat("waveSpeed", waveSpeed);
        waveComputeShader.SetFloat("waveHeight", waveHeight);
        waveComputeShader.SetFloat("waveLength", waveLength);
        waveComputeShader.SetBuffer(kernelHandle, "waveBuffer", waveBuffer);
    }

    void Update()
    {
        waveComputeShader.SetFloat("time", Time.time);
        waveComputeShader.Dispatch(kernelHandle, resolution / 8, resolution / 8, 1);

        waveBuffer.GetData(vertices);
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    void OnDestroy()
    {
        waveBuffer.Release();
    }
}
