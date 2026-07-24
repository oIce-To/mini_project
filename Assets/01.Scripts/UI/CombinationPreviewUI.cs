using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombinationPreviewUI : MonoBehaviour
{
    [Header("족보 표시")]
    [SerializeField] private TMP_Text combinationNameText;

    [Header("효과 표시")]
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text blockText;
    [SerializeField] private TMP_Text descriptionText;

    public void ShowPreview(CombinationResult result, CombinationData data, int finalDamage, int finalBlock)
    {
        if (result == null || data == null)
        {
            ClearPreview();
            return;
        }

        combinationNameText.text = $"현재 족보: {data.displayName}";
        damageText.text = $"{finalDamage}";
        blockText.text = $"{finalBlock}";
        descriptionText.text = CreateDescription(result, data, finalDamage, finalBlock);
    }

    public void ClearPreview()
    {
        combinationNameText.text = "현재 족보: 없음";
        damageText.text = "-";
        blockText.text = "-";
        descriptionText.text = "주사위를 굴리면 효과가 표시됩니다.";
    }

    private string CreateDescription(CombinationResult result, CombinationData data, int finalDamage, int finalBlock)
    {
        switch (data.damageCalculationType)
        {
            case DamageCalculationType.Fixed:
                return $"{data.baseDamage}의 피해를 줍니다.";
            case DamageCalculationType.MatchedValueMultiplier:
                return $"족보를 이룬 눈금({result.MatchedValue}) * {data.damageMultiplier} 만큼의 피해를 줍니다.";
            case DamageCalculationType.TotalValueMultiplier:
                return $"전체 눈금의 합({result.TotalValue}) * {data.damageMultiplier} 만큼의 피해를 줍니다.";

            default:
                return "-";
        }
    }
}
