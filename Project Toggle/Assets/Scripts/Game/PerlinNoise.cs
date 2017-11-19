using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PerlinNoise
{
    public long seed { get; set; }

    public PerlinNoise(long seed)
    {
        this.seed = seed;
    }

    private int GetRandom(int x, int range)
    {
        return (int)((x + seed ^ 5) % range);
    }

    public int GetNoise(int x, int range)
    {
        int chunkSize = 16;
        float noise = 0;

        range /= 2;
        
        while (chunkSize > 0)
        {
            int chunkIndex = x / chunkSize;
            float progress = (x % chunkSize) / ((float)chunkSize);
            float leftRandom = GetRandom(chunkIndex, range);
            float rightRandom = GetRandom(chunkIndex + 1, range);

            noise += (1 - progress) * leftRandom + progress * rightRandom;
            chunkSize /= 2;
            range /= 2;

            range = Mathf.Max(1, range);
        }
        return (int)Mathf.Round(noise);
    }
}

	
