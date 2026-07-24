using UnityEngine;
[System.Serializable]

public class DiceProbability
{
    public int[] weights = { 1, 1, 1, 1, 1, 1 };

    public int Roll()
    {
        int totalWeight = 0;

        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        int randomValue = Random.Range(0, totalWeight);

        int accumulatedWeight = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            accumulatedWeight += weights[i];

            if (randomValue < accumulatedWeight)
            {
                return i + 1;
            }
        }

        return 1;
    }
}