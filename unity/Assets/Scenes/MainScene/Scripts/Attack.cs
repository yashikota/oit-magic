using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Attack : MonoBehaviour
{
    private IList<GameObject> magics;

    private void Start()
    {
        StartCoroutine(LoadAsset());
    }

    private IEnumerator LoadAsset()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>("Magic", null);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            magics = handle.Result;
        }
        magics = magics.OrderBy(magic => magic.name).ToList();
    }

    public void Type(string element)
    {
        switch (element)
        {
            case "Fire":
                Fire();
                break;
            case "Aqua":
                Aqua();
                break;
            case "Wind":
                Wind();
                break;
            case "Lightning":
                Lightning();
                break;
        }
    }

    private void Fire()
    {
        var explosionPrefab = magics[0];
        var explosion = Instantiate(explosionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        explosion.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);

        if (explosion.TryGetComponent<ParticleSystem>(out var explosionParticle))
        {
            explosionParticle.Play();
            var duration = explosionParticle.main.duration;
            Destroy(explosion, duration);
        }
        else
        {
            Destroy(explosion);
        }
    }

    private void Aqua()
    {
        var waterTornadoPrefab = magics[3];
        var waterTornado = Instantiate(waterTornadoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        waterTornado.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        if (waterTornado.TryGetComponent<ParticleSystem>(out var waterTornadoParticle))
        {
            waterTornadoParticle.Play();
            var duration = waterTornadoParticle.main.duration;
            Destroy(waterTornado, duration);
        }
        else
        {
            Destroy(waterTornado);
        }
    }

    private void Wind()
    {
        var tornadoLoopPrefab = magics[2];
        var tornadoLoop = Instantiate(tornadoLoopPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        tornadoLoop.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);

        if (tornadoLoop.TryGetComponent<ParticleSystem>(out var tornadoLoopParticle))
        {
            tornadoLoopParticle.Play();
            var duration = tornadoLoopParticle.main.duration;
            Destroy(tornadoLoop, duration);
        }
        else
        {
            Destroy(tornadoLoop);
        }
    }

    private void Lightning()
    {
        var lightningBoltPrefab = magics[1];
        var lightningBolt = Instantiate(lightningBoltPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        lightningBolt.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        if (lightningBolt.TryGetComponent<ParticleSystem>(out var lightningBoltParticle))
        {
            lightningBoltParticle.Play();
            var duration = lightningBoltParticle.main.duration;
            Destroy(lightningBolt, duration);
        }
        else
        {
            Destroy(lightningBolt);
        }
    }
}
