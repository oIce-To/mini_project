using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [Header("주사위")]
    [SerializeField] private List<Dice> diceList;

    [Header("전투")]
    [SerializeField] private BattleManager battleManager;

    [Header("UI")]
    [SerializeField] private Button rerollButton;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TMP_Text rollCountText;
    [SerializeField] private int maxRollCount = 3;

    [Header("족보 미리보기")]
    [SerializeField]
    private CombinationDatabase combinationDatabase;

    [SerializeField]
    private CombinationPreviewUI combinationPreviewUI;

    public int RollCount { get; private set; }

    private bool resultSubmitted;

    private void Awake()
    {
        rerollButton.onClick.AddListener(RollDice);
        endTurnButton.onClick.AddListener(ConfirmDiceResult);
    }

    public void StartDiceTurn()
    {
        RollCount = 0;
        resultSubmitted = false;

        foreach (Dice dice in diceList)
        {
            dice.ResetDice();
        }

        rerollButton.interactable = true;
        endTurnButton.interactable = false;

        UpdateUI();

        combinationPreviewUI.ClearPreview();
    }

    public void RollDice()
    {
        if (resultSubmitted)
            return;

        if (RollCount >= maxRollCount)
        {
            Debug.LogWarning("더 이상 주사위를 굴릴 수 없습니다.");
            return;
        }

        if (RollCount > 0 && !HasUnlockedDice())
        {
            Debug.LogWarning("모든 주사위가 잠겨 있습니다. 잠금을 해제하거나 턴을 종료하세요.");
            return;
        }

        foreach (Dice dice in diceList)
        {
            dice.Roll();
        }

        RollCount++;

        // 첫 굴림이 끝난 뒤부터 주사위 잠금 가능
        SetDiceToggleInteractable(true);

        // 한 번 이상 굴렸다면 턴 종료 가능
        endTurnButton.interactable = true;

        // 총 굴림 횟수를 모두 사용했다면 리롤 불가능
        if (RollCount >= maxRollCount)
        {
            rerollButton.interactable = false;
        }

        UpdateUI();
        UpdateCombinationPreview();
    }

    private bool HasUnlockedDice()
    {
        foreach (Dice dice in diceList)
        {
            if (!dice.IsLocked)
                return true;
        }

        return false;
    }

    private void UpdateCombinationPreview()
    {
        if (RollCount == 0)
        {
            combinationPreviewUI.ClearPreview();
            return;
        }

        List<int> values = GetDiceValues();

        CombinationResult result = CombinationChecker.Check(values);
        CombinationData data = combinationDatabase.GetData(result.Combination);

        if (data == null)
        {
            Debug.LogWarning($"{result.Combination}에 해당하는 CombinationData가 없습니다.");

            combinationPreviewUI.ClearPreview();
            return;
        }

        int finalDamage = DamageCalculator.CalculateDamage(result, data);
        int finalBlock = data.baseBlock;

        combinationPreviewUI.ShowPreview(result, data, finalDamage, finalBlock);
    }

    public void ConfirmDiceResult()
    {
        if (resultSubmitted)
            return;

        if (RollCount == 0)
        {
            Debug.LogWarning("주사위를 먼저 굴려야 합니다.");
            return;
        }

        resultSubmitted = true;

        rerollButton.interactable = false;
        endTurnButton.interactable = false;

        SetDiceToggleInteractable(false);

        List<int> values = GetDiceValues();

        battleManager.UseDiceResult(values);
    }

    public List<int> GetDiceValues()
    {
        List<int> values = new List<int>();

        foreach (Dice dice in diceList)
        {
            values.Add(dice.Value);
        }

        return values;
    }

    private void SetDiceToggleInteractable(bool interactable)
    {
        foreach (Dice dice in diceList)
        {
            dice.SetToggleInteractable(interactable);
        }
    }

    private void UpdateUI()
    {
        if (rollCountText != null)
        {
            int remainingRolls = maxRollCount - RollCount;
            if (remainingRolls == maxRollCount)
                rollCountText.text = $"주사위 굴리기";
            else rollCountText.text = $"남은 리롤 횟수 : {remainingRolls}";
        }
    }
}