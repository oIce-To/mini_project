using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusUI : MonoBehaviour
{
    [Header("대상 캐릭터")]
    [SerializeField]
    private BattleCharacter targetCharacter;

    [Header("체력 UI")]
    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private TMP_Text hpText;

    [Header("방어도 UI")]
    [SerializeField]
    private GameObject blockPanel;

    [SerializeField]
    private TMP_Text blockText;

    [Header("설정")]
    [SerializeField]
    private bool hideBlockWhenZero = true;

    private void OnEnable()
    {
        if (targetCharacter == null)
        {
            Debug.LogError($"{gameObject.name}의 Target Character가 비어 있습니다.");
            return;
        }

        targetCharacter.OnStatusChanged += UpdateStatus;

        // UI가 활성화되는 즉시 현재 값으로 초기화
        UpdateStatus(targetCharacter.CurrentHp, targetCharacter.MaxHp, targetCharacter.Block);
    }

    private void OnDisable()
    {
        if (targetCharacter != null)
        {
            targetCharacter.OnStatusChanged -= UpdateStatus;
        }
    }

    private void UpdateStatus(int currentHp, int maxHp, int block)
    {
        hpSlider.minValue = 0;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;

        hpText.text = $"{currentHp} / {maxHp}";

        blockText.text = block.ToString();

        if (blockPanel != null && hideBlockWhenZero)
        {
            blockPanel.SetActive(block > 0);
        }
    }
}