using UnityEngine;

public class Player : BattleCharacter
{
    public void StartTurn()
    {
        ResetBlock();
        Debug.Log("플레이어 턴 시작");
    }
}
