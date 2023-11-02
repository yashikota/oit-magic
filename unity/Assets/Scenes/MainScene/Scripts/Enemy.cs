using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Enemy : MonoBehaviour
{
    private IList<GameObject> enemies;

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

        foreach (var enemy in enemies)
        {
            Debug.Log(enemy.name);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Slime();
        } else if (Input.GetKeyDown(KeyCode.B))
        {
            BlackKnight();
        }
    }

    private void Slime()
    {
        var slimePrefab = enemies[1];
        var slime = Instantiate(slimePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        slime.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
    }

    private void BlackKnight()
    {
        var blackKnightPrefab = enemies[0];
        var blackKnight = Instantiate(blackKnightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        blackKnight.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
    }
}
