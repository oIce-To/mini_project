using System;
using UnityEngine;

[Serializable]
public class EnemyAction
{
    [SerializeField] private EnemyActionType actionType;
    [SerializeField] private int value;
    [SerializeField] private int secondaryValue;

    public EnemyActionType ActionType => actionType;
    public int Value => value;
    public int SecondaryValue => secondaryValue;

    public string GetDescription()
    {
        switch (actionType)
        {
            case EnemyActionType.AttackAndBlock:
                return $"{value} / {secondaryValue}";

            case EnemyActionType.Attack or EnemyActionType.Block or EnemyActionType.BuffAttack:
                return $"{value}";

            default:
                return "행동 없음";
        }
    }
}

public enum EnemyActionType
{
    Attack,
    Block,
    AttackAndBlock,
    BuffAttack
}
