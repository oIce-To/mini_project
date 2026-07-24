using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleCharacter
{
    [Header("행동 패턴")]
    [SerializeField]
    private List<EnemyAction> actionPattern;
    private int currentActionIndex;
    private int attackBonus;

    public int CurrentActionIndex => currentActionIndex;

    protected override void Awake()
    {
        base.Awake();

        currentActionIndex = 0;
        attackBonus = 0;
    }

    public EnemyAction GetCurrentAction()
    {
        if (actionPattern == null || actionPattern.Count == 0)
        {
            Debug.LogWarning($"{gameObject.name}의 행동 패턴이 비어 있습니다.");
            return null;
        }

        return actionPattern[currentActionIndex];
    }

    public void ExecuteCurrentAction(Player player)
    {
        if (IsDead)
            return;

        EnemyAction action = GetCurrentAction();

        if (action == null)
            return;

        switch (action.ActionType)
        {
            case EnemyActionType.Attack:
                ExecuteAttack(player, action.Value);
                break;

            case EnemyActionType.Block:
                ExecuteBlock(action.Value);
                break;

            case EnemyActionType.AttackAndBlock:
                ExecuteAttack(player, action.Value);
                ExecuteBlock(action.SecondaryValue);
                break;

            case EnemyActionType.BuffAttack:
                ExecuteAttackBuff(action.Value);
                break;
        }

        AdvanceAction();
    }

    private void ExecuteAttack(Player player, int baseDamage)
    {
        int finalDamage = baseDamage + attackBonus;

        Debug.Log($"{gameObject.name}이 플레이어에게 {finalDamage}의 피해를 줍니다.");
        player.TakeDamage(finalDamage);
    }

    private void ExecuteBlock(int blockAmount)
    {
        AddBlock(blockAmount);

        Debug.Log($"{gameObject.name}이 방어력 {blockAmount}을 획득합니다.");
    }

    private void ExecuteAttackBuff(int amount)
    {
        attackBonus += amount;

        Debug.Log($"{gameObject.name}의 공격력이 {amount} 증가했습니다. 현재 추가 공격력: {attackBonus}");
    }

    private void AdvanceAction()
    {
        currentActionIndex++;

        if (currentActionIndex >= actionPattern.Count)
        {
            currentActionIndex = 0;
        }
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();

        currentActionIndex = 0;
        attackBonus = 0;
    }
}