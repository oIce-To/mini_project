public class CombinationResult
{
    public DiceCombination Combination { get; private set; }
    public int MatchedValue { get; private set; } // 족보 달성 숫자
    public int TotalValue { get; private set; } // 숫자 총 합

    public CombinationResult(
        DiceCombination combination,
        int matchedValue,
        int totalValue)
    {
        Combination = combination;
        MatchedValue = matchedValue;
        TotalValue = totalValue;
    }
}