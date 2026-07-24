using UnityEngine;

[CreateAssetMenu(
    fileName = "NewCombinationData",
    menuName = "Dice Game/Combination Data")]
public class CombinationData : ScriptableObject
{
    [Header("족보 정보")]
    public DiceCombination combination;
    public string displayName;

    [Header("피해 계산")]
    public DamageCalculationType damageCalculationType;
    public int baseDamage;
    public int damageMultiplier;

    [Header("방어")]
    public int baseBlock;

    [Header("표시 정보")]
    [TextArea]
    public string description;

    public Sprite icon;
}