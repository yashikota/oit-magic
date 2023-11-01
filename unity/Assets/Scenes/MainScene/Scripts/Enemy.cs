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
        var SlimePrefab = enemies[1];
        var Slime = Instantiate(SlimePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Slime.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);

        // if (Slime.TryGetComponent<ParticleSystem>(out var SlimeParticle))
        // {
        //     SlimeParticle.Play();
        //     var duration = SlimeParticle.main.duration;
        //     Destroy(Slime, duration);
        // }
        // else
        // {
        //     Destroy(Slime);
        // }
    }

    private void BlackKnight()
    {
        var DragonPrefab = enemies[0];
        var Dragon = Instantiate(DragonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Dragon.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);

        // if (Dragon.TryGetComponent<ParticleSystem>(out var DragonParticle))
        // {
        //     DragonParticle.Play();
        //     var duration = DragonParticle.main.duration;
        //     Destroy(Dragon, duration);
        // }
        // else
        // {
        //     Destroy(Dragon);
        // }
    }
}
