using System.Collections.Generic;
using UnityEngine;

public class CombinationDatabase : MonoBehaviour
{
    [SerializeField]
    private List<CombinationData> combinationDataList;

    public CombinationData GetData(
        DiceCombination combination)
    {
        foreach (CombinationData data in combinationDataList)
        {
            if (data.combination == combination)
                return data;
        }

        return null;
    }
}

