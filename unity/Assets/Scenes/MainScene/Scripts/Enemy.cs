using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Particle particle;
    [SerializeField] private Image slimeAppear;

    private IList<GameObject> enemies;
    private GameObject enemy;

    public List<string> elements;
    private string enemyElement;

    private void Start()
    {
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

    public void SetElements()
    {
        elements = new List<string> { "Fire", "Aqua", "Wind" };
    }

    public static string GetName()
    {
        return GameManager.Round <= 3 ? "Slime" : "BlackKnight";
    }

    public string GetElement()
    {
        return enemyElement;
    }

    public void Summon(string PlayerElement)
    {
        switch (PlayerElement)
        {
            case "Fire":
                enemyElement = "Wind";
                Slime(enemyElement);
                break;
            case "Aqua":
                enemyElement = "Fire";
                Slime(enemyElement);
                break;
            case "Wind":
                enemyElement = "Aqua";
                Slime(enemyElement);
                break;
            case "Free":
                var random = Random.Range(0, elements.Count);
                enemyElement = elements[random];
                elements.Remove(enemyElement);
                BlackKnight();
                break;
            case "Lightning":
                enemyElement = "Dark";
                BlackKnight();
                break;
        }

        particle.GenerateParticle(enemyElement);
    }

    private void Slime(string enemyElement)
    {
        enemyElement += "SlimePrefab";
        const float size = 3.0f;
        var slimePrefab = enemies.FirstOrDefault(enemy => enemy.name == enemyElement);
        var slime = Instantiate(slimePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        slime.transform.Rotate(new Vector3(0, 180, 0));
        slime.transform.localScale = new Vector3(size, size, size);
        enemy = slime;
    }

    private void BlackKnight()
    {
        const float size = 2.5f;
        var blackKnightPrefab = enemies[1];
        var blackKnight = Instantiate(blackKnightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        blackKnight.transform.localScale = new Vector3(size, size, size);
        blackKnight.transform.Rotate(new Vector3(0, 180, 0));
        enemy = blackKnight;
    }

    public void DestroyEnemy()
    {
        if (enemy == null) return;
        Destroy(enemy);
    }

    public void BlackKnightAttackAnimation()
    {
        BlackKnightAnimator.Attack();
    }
}
