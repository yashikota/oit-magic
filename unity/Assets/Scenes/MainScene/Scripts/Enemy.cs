using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Countdown countdown;
    [SerializeField] private Battle battle;

    private IList<GameObject> enemies;
    private GameObject enemy;
    private bool isEnemyNull;

    private void Start()
    {
        isEnemyNull = enemy == null;
        StartCoroutine(LoadAsset());
    }

    private IEnumerator LoadAsset()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>("Enemy", null);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            enemies = handle.Result;
        }
        enemies = enemies.OrderBy(enemy => enemy.name).ToList();
    }

    public static string GetName()
    {
        return GameManager.Round <= 3 ? "Slime" : "BlackKnight";
    }

    public void Summon(string element)
    {
        switch (element)
        {
            case "Fire":
                Slime("WindSlimePrefab");
                break;
            case "Aqua":
                DestroyEnemy();
                Slime("FireSlimePrefab");
                break;
            case "Wind":
                DestroyEnemy();
                Slime("WaterSlimePrefab");
                break;
            case "Free":
            case "Lightning":
                DestroyEnemy();
                BlackKnight(element);
                break;
        }

        battle.BattleStart();
    }

    private void Slime(string type)
    {
        const float size = 3.0f;
        var slimePrefab = enemies.FirstOrDefault(enemy => enemy.name == type);
        var slime = Instantiate(slimePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        slime.transform.Rotate(new Vector3(0, 180, 0));
        slime.transform.localScale = new Vector3(size, size, size);
        enemy = slime;
    }

    private void BlackKnight(string element)
    {
        const float size = 2.0f;
        var blackKnightPrefab = enemies[0];
        var blackKnight = Instantiate(blackKnightPrefab, new Vector3(0, 0, -5.5f), Quaternion.identity);
        blackKnight.transform.localScale = new Vector3(size, size, size);
        blackKnight.transform.Rotate(new Vector3(0, 180, 0));
        enemy = blackKnight;
    }

    public void DestroyEnemy()
    {
        if (isEnemyNull) return;
        Destroy(enemy);
    }
}
