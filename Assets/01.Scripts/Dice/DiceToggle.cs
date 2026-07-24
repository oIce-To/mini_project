using UnityEngine;
using UnityEngine.UI;

public class DiceToggleUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image diceImage;
    [SerializeField] private GameObject grayOverlay;

    [Header("주사위 눈 이미지")]
    [SerializeField] private Sprite[] diceSprites;

    [Header("현재 주사위 값")]
    [SerializeField] private int diceValue = 1;

    public int DiceValue => diceValue;
    public bool IsSelected => toggle.isOn;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(UpdateToggleVisual);

        SetDiceValue(diceValue);
        UpdateToggleVisual(toggle.isOn);
    }

    public void SetDiceValue(int value)
    {
        if (value < 1 || value > 6)
        {
            Debug.LogWarning($"잘못된 주사위 값입니다: {value}");
            return;
        }

        if (diceSprites == null || diceSprites.Length < 6)
        {
            Debug.LogWarning("주사위 Sprite 6개를 Inspector에 등록해야 합니다.");
            return;
        }

        diceValue = value;
        diceImage.sprite = diceSprites[diceValue - 1];
    }

    private void UpdateToggleVisual(bool isOn)
    {
        grayOverlay.SetActive(!isOn);
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(UpdateToggleVisual);
    }
}