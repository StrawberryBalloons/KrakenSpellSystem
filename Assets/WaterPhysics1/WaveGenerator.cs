using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics; // For complex numbers

public class WaveGenerator : MonoBehaviour
{
    public int meshSize = 256; // Size of the mesh
    public float waveSpeed = 1.0f; // Speed of the wave
    public float waveHeight = 1.0f; // Height of the wave
    public float waveLength = 10.0f; // Length of the wave
    public List<UnityEngine.Vector4> waveParameters; // (amplitude, wavelength, speed, direction angle)

    private Mesh mesh;
    private UnityEngine.Vector3[] vertices;
    private UnityEngine.Vector3[] originalVertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        vertices = new UnityEngine.Vector3[originalVertices.Length];
        originalVertices.CopyTo(vertices, 0);

        // Example wave parameters (amplitude, wavelength, speed, direction angle)
        waveParameters = new List<UnityEngine.Vector4>
        {
            new UnityEngine.Vector4(3.5f, 5f, .75f, 0.0f),
            new UnityEngine.Vector4(1f, 2f, 5f, 45.0f),
            new UnityEngine.Vector4(0.8f, 8.0f, 0.8f, 90.0f)
        };
    }

    void Update()
    {
        GenerateWaves();
        ApplyFFT();
    }

    void GenerateWaves()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            UnityEngine.Vector3 vertex = originalVertices[i];
            vertex.y = 0;

            foreach (var waveParam in waveParameters)
            {
                vertex.y += GerstnerWave(vertex.x, vertex.z, Time.time, waveParam);
            }

            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    float GerstnerWave(float x, float z, float time, UnityEngine.Vector4 waveParam)
    {
        float amplitude = waveParam.x;
        float wavelength = waveParam.y;
        float speed = waveParam.z;
        float directionAngle = waveParam.w;

        float k = 2 * Mathf.PI / wavelength; // Wave number
        float c = Mathf.Sqrt(9.81f / k); // Wave speed
        UnityEngine.Vector2 waveDirection = new UnityEngine.Vector2(Mathf.Cos(directionAngle * Mathf.Deg2Rad), Mathf.Sin(directionAngle * Mathf.Deg2Rad));
        float phase = k * (waveDirection.x * x + waveDirection.y * z) - c * time * speed; // Wave phase
        float wave = amplitude * Mathf.Sin(phase); // Gerstner wave formula
        return wave;
    }

    void ApplyFFT()
    {
        // Example FFT application
        int N = vertices.Length;
        Complex[] complexVertices = new Complex[N];
        for (int i = 0; i < N; i++)
        {
            complexVertices[i] = new Complex(vertices[i].y, 0);
        }

        // Perform FFT
        Complex[] fftResult = FFT(complexVertices);

        // Apply the FFT result back to vertices
        for (int i = 0; i < N; i++)
        {
            vertices[i].y = (float)fftResult[i].Magnitude;
        }
    }

    public Complex[] FFT(Complex[] x)
    {
        int N = x.Length;
        if (N <= 1)
            return x;

        // Divide
        Complex[] even = new Complex[N / 2];
        Complex[] odd = new Complex[N / 2];
        for (int i = 0; i < N / 2; i++)
        {
            even[i] = x[i * 2];
            odd[i] = x[i * 2 + 1];
        }

        // Conquer
        Complex[] fftEven = FFT(even);
        Complex[] fftOdd = FFT(odd);

        // Combine
        Complex[] result = new Complex[N];
        for (int k = 0; k < N / 2; k++)
        {
            Complex t = Complex.Exp(-2.0 * System.Math.PI * Complex.ImaginaryOne * k / N) * fftOdd[k];
            result[k] = fftEven[k] + t;
            result[k + N / 2] = fftEven[k] - t;
        }
        return result;
    }
}
