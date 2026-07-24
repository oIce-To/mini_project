using System;
using UnityEngine;

public abstract class BattleCharacter : MonoBehaviour
{
    [Header("기본 능력치")]
    [SerializeField] protected int maxHp;

    public int MaxHp => maxHp;
    public int CurrentHp { get; protected set; }
    public int Block { get; protected set; }

    public bool IsDead => CurrentHp <= 0;

    public event Action<int, int, int> OnStatusChanged;

    protected virtual void Awake()
    {
        CurrentHp = maxHp;
        Block = 0;
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0 || IsDead)
            return;

        int blockedDamage = Mathf.Min(Block, damage);

        Block -= blockedDamage;
        damage -= blockedDamage;

        CurrentHp -= damage;
        CurrentHp = Mathf.Max(CurrentHp, 0);

        NotifyStatusChanged();

        Debug.Log($"{gameObject.name} 피해를 받음. 남은 체력: {CurrentHp}, 방어력: {Block}");
    }

    public virtual void AddBlock(int amount)
    {
        if (amount <= 0 || IsDead)
            return;

        Block += amount;

        NotifyStatusChanged();

        Debug.Log($"{gameObject.name} 방어력 {amount} 획득. 현재 방어력: {Block}");
    }

    public virtual void ResetBlock()
    {
        Block = 0;

        NotifyStatusChanged();
    }

    public virtual void Heal(int amount)
    {
        if (amount <= 0 || IsDead)
            return;

        CurrentHp += amount;
        CurrentHp = Mathf.Min(CurrentHp, maxHp);

        NotifyStatusChanged();
    }

    public virtual void ResetCharacter()
    {
        CurrentHp = maxHp;
        Block = 0;

        NotifyStatusChanged();
    }

    protected void NotifyStatusChanged()
    {
        OnStatusChanged?.Invoke(CurrentHp, MaxHp, Block);
    }
}