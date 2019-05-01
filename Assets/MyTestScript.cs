using Microsoft.MixedReality.Toolkit.Input;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class MyTestScript : MonoBehaviour
{
    public int numInteractions = 10000;
    public int numTimings = 10;
    public TextMeshPro text;

    private MixedRealityInteractionMapping[] interactions;
    private MixedRealityInteractionMappingNew[] newInteractions;
    private Vector2[] newValues;
    private double[] timings;

    private int currentIteration;

    static Vector2 randomVector(System.Random rng)
    {
        return new Vector2((float)rng.NextDouble(), (float)rng.NextDouble());
    }

    private void OnEnable()
    {
        currentIteration = 0;
        interactions = new MixedRealityInteractionMapping[numInteractions];
        newInteractions = new MixedRealityInteractionMappingNew[numInteractions];
        newValues = new Vector2[numInteractions];
        timings = new double[numTimings * 2];

        System.Random rng = new System.Random(currentIteration);

        for (int i = 0; i < numInteractions; ++i)
        {
            bool invertXAxis = rng.NextDouble() > 0.5f;
            bool invertYAxis = rng.NextDouble() > 0.5f;
            Vector2 value = randomVector(rng);
            interactions[i] = new MixedRealityInteractionMapping(invertXAxis, invertYAxis, value);
            newInteractions[i] = new MixedRealityInteractionMappingNew(invertXAxis, invertYAxis, value);
        }
    }

    void Update()
    {
        if (currentIteration < numTimings)
        {
            System.Random rng = new System.Random(currentIteration);

            for (int i = 0; i < numInteractions; ++i)
            {
                newValues[i] = randomVector(rng);
            }

            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < numInteractions; ++i)
            {
                interactions[i].Vector2Data = newValues[i];
            }
            timings[2 * currentIteration] = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            for (int i = 0; i < numInteractions; ++i)
            {
                newInteractions[i].Vector2Data = newValues[i];
            }
            timings[2 * currentIteration + 1] = sw.Elapsed.TotalMilliseconds;

            currentIteration++;

            if (currentIteration == numTimings)
            {
                for (int i = 0; i < currentIteration; ++i)
                {
                    text.text = text.text + timings[2 * i].ToString("F3") + " - " + timings[2 * i + 1].ToString("F3") + "\n";
                }
            }
        }
    }
}
