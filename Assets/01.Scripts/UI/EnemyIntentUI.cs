using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntentUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TMP_Text intentText;

    [SerializeField]
    private Image intentIcon;

    [Header("행동 아이콘")]
    [SerializeField]
    private Sprite attackIcon;

    [SerializeField]
    private Sprite blockIcon;

    [SerializeField]
    private Sprite attackAndBlockIcon;

    [SerializeField]
    private Sprite buffIcon;

    public void UpdateIntent(EnemyAction action)
    {
        if (action == null)
        {
            intentText.text = "행동 없음";

            if (intentIcon != null)
                intentIcon.enabled = false;

            return;
        }

        intentText.text = action.GetDescription();

        if (intentIcon == null)
            return;

        intentIcon.enabled = true;

        switch (action.ActionType)
        {
            case EnemyActionType.Attack:
                intentIcon.sprite = attackIcon;
                break;

            case EnemyActionType.Block:
                intentIcon.sprite = blockIcon;
                break;

            case EnemyActionType.AttackAndBlock:
                intentIcon.sprite = attackAndBlockIcon;
                break;

            case EnemyActionType.BuffAttack:
                intentIcon.sprite = buffIcon;
                break;
        }
    }

    public void HideIntent()
    {
        intentText.text = string.Empty;

        if (intentIcon != null)
            intentIcon.enabled = false;
    }
}