public static class DamageCalculator
{
    public static int CalculateDamage(
        CombinationResult result,
        CombinationData data)
    {
        switch (data.damageCalculationType)
        {
            case DamageCalculationType.Fixed:
                return data.baseDamage;

            case DamageCalculationType.MatchedValueMultiplier:
                return result.MatchedValue * data.damageMultiplier;

            case DamageCalculationType.TotalValueMultiplier:
                return result.TotalValue * data.damageMultiplier;

            default:
                return 0;
        }
    }
}