using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Countdown countdown;
    [SerializeField] private Enemy enemy;

    private void Update()
    {
        if (Enemy.GetName() != "Slime" || !countdown.Is15Seconds()) return;
        countdown.StartTimer();
        gameManager.Rounds();
    }

    public void BattleStart()
    {
        countdown.StartTimer();
    }
}
