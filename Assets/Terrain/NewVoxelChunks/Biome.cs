using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome
{
    public string name;
    public float frequency;
    public float amplitude;
    public int seed;

    public Biome(string name, float frequency, float amplitude, int seed)
    {
        this.name = name;
        this.frequency = frequency;
        this.amplitude = amplitude;
        this.seed = seed;
    }
}
