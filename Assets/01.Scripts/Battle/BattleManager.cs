using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("전투 참가자")]
    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy enemy;

    [Header("주사위")]
    [SerializeField]
    private DiceManager diceManager;

    [Header("족보 데이터")]
    [SerializeField]
    private CombinationDatabase combinationDatabase;

    [Header("UI")]
    [SerializeField]
    private EnemyIntentUI enemyIntentUI;

    public BattleState CurrentState { get; private set; }

    private void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        player.ResetCharacter();
        enemy.ResetCharacter();

        Debug.Log("전투 시작");

        UpdateEnemyIntent();
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        if (player.IsDead || enemy.IsDead)
            return;

        CurrentState = BattleState.PlayerRolling;

        player.StartTurn();
        diceManager.StartDiceTurn();

        UpdateEnemyIntent();

        Debug.Log("플레이어 턴 시작");
    }

    public void UseDiceResult(List<int> diceValues)
    {
        if (CurrentState != BattleState.PlayerRolling)
        {
            Debug.LogWarning("현재는 주사위 결과를 사용할 수 없습니다.");
            return;
        }

        CurrentState = BattleState.PlayerAction;

        CombinationResult result = CombinationChecker.Check(diceValues);
        CombinationData data = combinationDatabase.GetData(result.Combination);

        if (data == null)
        {
            Debug.LogError($"{result.Combination}에 해당하는 CombinationData가 없습니다.");

            // 상태가 PlayerAction에 갇히지 않도록 복구
            CurrentState = BattleState.PlayerRolling;
            return;
        }

        int finalDamage = DamageCalculator.CalculateDamage(result, data);

        enemy.TakeDamage(finalDamage);
        player.AddBlock(data.baseBlock);

        Debug.Log(
            $"족보: {result.Combination}\n" +
            $"일치 숫자: {result.MatchedValue}\n" +
            $"주사위 합: {result.TotalValue}\n" +
            $"피해량: {finalDamage}\n" +
            $"획득 방어력: {data.baseBlock}");

        if (enemy.IsDead)
        {
            WinBattle();
            return;
        }

        StartEnemyTurn();
    }

    private void StartEnemyTurn()
    {
        CurrentState = BattleState.EnemyTurn;

        enemy.ResetBlock();

        enemy.ExecuteCurrentAction(player);

        if (player.IsDead)
        {
            LoseBattle();
            return;
        }

        StartPlayerTurn();
    }

    private void UpdateEnemyIntent()
    {
        EnemyAction nextAction = enemy.GetCurrentAction();

        enemyIntentUI.UpdateIntent(nextAction);

        if (nextAction != null)
        {
            Debug.Log(
                $"적의 다음 행동: " +
                $"{nextAction.GetDescription()}");
        }
    }

    private void WinBattle()
    {
        CurrentState = BattleState.Victory;

        enemyIntentUI.HideIntent();

        Debug.Log("전투에서 승리했습니다.");

        // 이후 업그레이드 선택 화면
    }

    private void LoseBattle()
    {
        CurrentState = BattleState.Defeat;

        enemyIntentUI.HideIntent();

        Debug.Log("플레이어가 패배했습니다.");

        // 이후 게임 오버 화면
    }
}