using System.Collections.Generic;
using System.Linq;

public static class CombinationChecker
{
    public static CombinationResult Check(List<int> diceValues)
    {
        int totalValue = diceValues.Sum();

        Dictionary<int, int> counts = diceValues
            .GroupBy(value => value)
            .ToDictionary(
                group => group.Key,
                group => group.Count());

        // 요트
        int yachtValue = GetValueWithCount(counts, 5);

        if (yachtValue != 0)
        {
            return new CombinationResult(
                DiceCombination.Yacht,
                yachtValue,
                totalValue);
        }

        // 라지 스트레이트
        if (IsLargeStraight(diceValues))
        {
            return new CombinationResult(
                DiceCombination.LargeStraight,
                0,
                totalValue);
        }

        // 스몰 스트레이트
        if (IsSmallStraight(diceValues))
        {
            return new CombinationResult(
                DiceCombination.SmallStraight,
                0,
                totalValue);
        }

        // 포 오브 어 카인드
        int fourValue = GetValueWithCount(counts, 4);

        if (fourValue != 0)
        {
            return new CombinationResult(
                DiceCombination.FourOfAKind,
                fourValue,
                totalValue);
        }

        // 풀 하우스
        int tripleValue = GetValueWithCount(counts, 3);
        int pairValue = GetValueWithCount(counts, 2);

        if (tripleValue != 0 && pairValue != 0)
        {
            return new CombinationResult(
                DiceCombination.FullHouse,
                tripleValue,
                totalValue);
        }

        // 트리플
        if (tripleValue != 0)
        {
            return new CombinationResult(
                DiceCombination.Triple,
                tripleValue,
                totalValue);
        }

        List<int> pairValues = counts
            .Where(pair => pair.Value == 2)
            .Select(pair => pair.Key)
            .OrderByDescending(value => value)
            .ToList();

        // 투 페어
        if (pairValues.Count == 2)
        {
            return new CombinationResult(
                DiceCombination.TwoPair,
                pairValues[0],
                totalValue);
        }

        // 원 페어
        if (pairValues.Count == 1)
        {
            return new CombinationResult(
                DiceCombination.OnePair,
                pairValues[0],
                totalValue);
        }

        // 아무런 족보도 없음
        return new CombinationResult(
            DiceCombination.None,
            0,
            totalValue);
    }

    private static int GetValueWithCount(
        Dictionary<int, int> counts,
        int targetCount)
    {
        foreach (KeyValuePair<int, int> pair in counts)
        {
            if (pair.Value == targetCount)
            {
                return pair.Key;
            }
        }

        return 0;
    }

    private static bool IsLargeStraight(List<int> values)
    {
        List<int> sorted = values
            .Distinct()
            .OrderBy(value => value)
            .ToList();

        if (sorted.Count != 5)
            return false;

        bool lowStraight = sorted.SequenceEqual(
            new List<int> { 1, 2, 3, 4, 5 });

        bool highStraight = sorted.SequenceEqual(
            new List<int> { 2, 3, 4, 5, 6 });

        return lowStraight || highStraight;
    }

    private static bool IsSmallStraight(List<int> values)
    {
        List<int> sorted = values
            .Distinct()
            .OrderBy(value => value)
            .ToList();

        bool has1234 =
            sorted.Contains(1) &&
            sorted.Contains(2) &&
            sorted.Contains(3) &&
            sorted.Contains(4);

        bool has2345 =
            sorted.Contains(2) &&
            sorted.Contains(3) &&
            sorted.Contains(4) &&
            sorted.Contains(5);

        bool has3456 =
            sorted.Contains(3) &&
            sorted.Contains(4) &&
            sorted.Contains(5) &&
            sorted.Contains(6);

        return has1234 || has2345 || has3456;
    }
}