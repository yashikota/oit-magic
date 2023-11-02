using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Particle : MonoBehaviour
{
    private IList<GameObject> particles;
    private GameObject particle;

    private void Start()
    {
        StartCoroutine(LoadAsset());
    }

    private IEnumerator LoadAsset()
    {
        // 1: Aqua, 2: Fire, 4: Wind, 5: Dark
        var handle = Addressables.LoadAssetsAsync<GameObject>("Ring", null);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            particles = handle.Result;
        }
        particles = particles.OrderBy(enemy => enemy.name).ToList();

    }

    public void GenerateParticle(string element)
    {
        string particleName = "";

        switch (element)
        {
            case "Fire":
                particleName = "magic_ring_02";
                break;
            case "Aqua":
                particleName = "magic_ring_01";
                break;
            case "Wind":
                particleName = "magic_ring_04";
                break;
            case "Lightning":
                particleName = "magic_ring_05";
                break;
        }

        var size = 3.0f;
        var particlePrefab = particles.FirstOrDefault(particle => particle.name == particleName);
        particle = Instantiate(particlePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
        particle.transform.localScale = new Vector3(size, size, size);
    }

    public void DestroyParticle()
    {
        Destroy(particle);
    }
}
