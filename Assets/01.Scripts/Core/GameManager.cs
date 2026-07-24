using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Battle:
                StartBattle();
                break;

            case GameState.UpgradeSelection:
                OpenUpgradeSelection();
                break;

            case GameState.GameOver:
                OpenGameOver();
                break;
        }
    }

    private void StartBattle()
    {
    }

    private void OpenUpgradeSelection()
    {
    }

    private void OpenGameOver()
    {
    }
}