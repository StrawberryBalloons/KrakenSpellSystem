using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    #region Functions

    // Generates 3D simplex noise
    public static float Generate(float x, float y, float z)
    {
        // Skew the input space to determine which simplex cell we're in
        float s = (x + y + z) * .33f; // Very nice an simple skew factor for 3
        int i = FastFloor(x + s);
        int j = FastFloor(y + s);
        int k = FastFloor(z + s);

        float t = (i + j + k) * (1.0f / 6.0f);
        float _x0 = i - t; // Unskew the cell origin back to (x,y,z) space
        float _y0 = j - t;
        float _z0 = k - t;
        float x0 = x - _x0; // The x,y,z istances from the cell origin
        float y0 = y - _y0;
        float z0 = z - _z0;

        // For the 3 case, the simplex shape is a slightly irregular tetraheron.
        // Determine which simplex we are in.
        int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
        int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

        if (x0 >= y0)
        {
            if (y0 >= z0) // X Y Z order
            {
                i1 = 1; j1 = 0; k1 = 0;
                i2 = 1; j2 = 1; k2 = 0;
            }
            else if (x0 >= z0) // X Z Y order
            {
                i1 = 1; j1 = 0; k1 = 0;
                i2 = 1; j2 = 0; k2 = 1;
            }
            else // Z X Y order
            {
                i1 = 0; j1 = 0; k1 = 1;
                i2 = 1; j2 = 0; k2 = 1;
            }
        }
        else // x0<y0
        {
            if (y0 < z0) // Z Y X order
            {
                i1 = 0; j1 = 0; k1 = 1;
                i2 = 0; j2 = 1; k2 = 1;
            }
            else if (x0 < z0) // Y Z X order
            {
                i1 = 0; j1 = 1; k1 = 0;
                i2 = 0; j2 = 1; k2 = 1;
            }
            else // Y X Z order
            {
                i1 = 0; j1 = 1; k1 = 0;
                i2 = 1; j2 = 1; k2 = 0;
            }
        }

        // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
        // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
        // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
        // c = 1/6.

        float x1 = x0 - i1 + (1.0f / 6.0f); // Offsets for secon corner in (x,y,z) coords
        float y1 = y0 - j1 + (1.0f / 6.0f);
        float z1 = z0 - k1 + (1.0f / 6.0f);

        float x2 = x0 - i2 + 2.0f * (1.0f / 6.0f); // Offsets for thir corner in (x,y,z) coords
        float y2 = y0 - j2 + 2.0f * (1.0f / 6.0f);
        float z2 = z0 - k2 + 2.0f * (1.0f / 6.0f);

        float x3 = x0 - 1.0f + 3.0f * (1.0f / 6.0f); // Offsets for last corner in (x,y,z) coords
        float y3 = y0 - 1.0f + 3.0f * (1.0f / 6.0f);
        float z3 = z0 - 1.0f + 3.0f * (1.0f / 6.0f);

        // Wrap the integer indices at 256, to avoi indexing perm[] out of bounds
        int ii = i & 255;
        int jj = j & 255;
        int kk = k & 255;

        int gi0 = permMod12[ii + perm[jj + perm[kk]]];
        int gi1 = permMod12[ii + i1 + perm[jj + j1 + perm[kk + k1]]];
        int gi2 = permMod12[ii + i2 + perm[jj + j2 + perm[kk + k2]]];
        int gi3 = permMod12[ii + 1 + perm[jj + 1 + perm[kk + 1]]];

        float n0, n1, n2, n3;

        // Calculate the contribution from the four corners
        float t0 = .6f - x0 * x0 - y0 * y0 - z0 * z0;
        if (t0 < 0) n0 = 0.0f;
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * gradDot(grad3[gi0], x0, y0, z0);
        }
        float t1 = .6f - x1 * x1 - y1 * y1 - z1 * z1;
        if (t1 < 0) n1 = 0.0f;
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * gradDot(grad3[gi1], x1, y1, z1);
        }
        float t2 = .6f - x2 * x2 - y2 * y2 - z2 * z2;
        if (t2 < 0) n2 = 0.0f;
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * gradDot(grad3[gi2], x2, y2, z2);
        }
        float t3 = .6f - x3 * x3 - y3 * y3 - z3 * z3;
        if (t3 < 0) n3 = 0.0f;
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * gradDot(grad3[gi3], x3, y3, z3);
        }
        // A contributions from each corner to get the final noise value.
        // The result is scale to stay just insie [-1,1]
        return 32.0f * (n0 + n1 + n2 + n3); // TODO: The scale factor is preliminary!
    }

    public class Grad
    {
        public float x, y, z, w;

        public Grad(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public Grad(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
    }

    static Grad[] grad3 =
    {
        new Grad (1, 1, 0),
        new Grad (-1, 1, 0),
        new Grad (1, -1, 0),
        new Grad (-1, -1, 0),
        new Grad (1, 0, 1),
        new Grad (-1, 0, 1),
        new Grad (1, 0, -1),
        new Grad (-1, 0, -1),
        new Grad (0, 1, 1),
        new Grad (0, -1, 1),
        new Grad (0, 1, -1),
        new Grad (0, -1, -1)
    };

    // This is a Pseuorandom see, shuffle to see the alogrithm
    // NO DUPLICATE VALUES
    static short[] p =
    {
        180,160,137,91,90,15,
        131,13,201,95,96,53,194,
        233,7,225,140,36,103,30,
        69,142,8,99,37,240,21,10,
        23,190, 6,148,247,120,234,
        75,0,26,197,62,94,252,219,
        203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,
        136,171,168, 68,175,74,165,71,
        134,139,48,27,166,77,146,158,
        231,83,111,229,122,60,211,133,
        230,220,105,92,41,55,46,245,40,
        244,102,143,54, 65,25,63,161,
        1,216,80,73,209,76,132,187,208,
        89,18,169,200,196,135,130,116,
        188,159,86,164,100,109,198,173,
        186,3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,
        212,207,206,59,227,47,16,58,17,
        182,189,28,42,223,183,170,213,
        119,248,152, 2,44,154,163,70,
        221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,
        113,224,232,178,185, 112,104,218,
        246,97,228,251,34,242,193,238,
        210,144,12,191,179,162,241,81,
        51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,
        157,184, 84,204,176,115,121,
        50,45,127, 4,150,254,138,236,
        205,93,222,114,67,29,24,72,243,
        141,128,195,78,66,215,61,156,151
    };

    private static int FastFloor(float x)
    {
        return (x > 0) ? ((int)x) : (((int)x) - 1);
    }

    static short[] perm = new short[512];

    static short[] permMod12 = new short[512];

    // this function generates perm an permMod12 from the psuoradnom array of p
    void permGen()
    {
        for (int i = 0; i < 512; i++)
        {
            perm[i] = p[i & 255];
            permMod12[i] = (short)(perm[i] % 12);
        }
        Debug.Log("DEBUG: Finished generating permuations");
    }

    // Dot product using the Grad class
    static float gradDot(Grad g, float x, float y)
    {
        return g.x * x + g.y * y;
    }

    static float gradDot(Grad g, float x, float y, float z)
    {
        return g.x * x + g.y * y + g.z * z;
    }

    #endregion
}

