using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Particle particle;
    [SerializeField] private RoundManager roundManager;

    public bool IsPlayerElementWin(string playerElement, string enemyElement)
    {
        if (Enemy.GetName() == "Slime") return true;
        if (enemyElement == "Dark") return true;
        else if (playerElement == "Fire" && enemyElement == "Wind") return true;
        else if (playerElement == "Aqua" && enemyElement == "Fire") return true;
        else if (playerElement == "Wind" && enemyElement == "Aqua") return true;
        else return false;
    }

    public async void PlayerWin()
    {
        enemy.DestroyEnemy();
        particle.DestroyParticle();
        await Task.Delay(1000);
        gameManager.Rounds();
    }

    public async void PlayerLose()
    {
        enemy.BlackKnightAttackAnimation();
        await Task.Delay(2000);
        gameManager.DecreaseHp();
        roundManager.Reset();

        // Top, Bottom, Left, Rightのターゲットを白に戻す
        foreach (var key in GameManager.Magics["Free"].Cast<string>()
            .Where((_, i) => i % 2 == 0).Select(v => v))
        {
            Target.ChangeColor(key, Color.white);
        }
    }
}
