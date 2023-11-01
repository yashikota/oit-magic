using System.Collections;
using System.Collections.Generic;
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
        var ExplosionPrefab = magics[0];
        var Explosion = Instantiate(ExplosionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Explosion.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);

        if (Explosion.TryGetComponent<ParticleSystem>(out var ExplosionParticle))
        {
            ExplosionParticle.Play();
            var duration = ExplosionParticle.main.duration;
            Destroy(Explosion, duration);
        }
        else
        {
            Destroy(Explosion);
        }
    }

    private void Aqua()
    {
        var waterTornadoPrefab = magics[1];
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
        var lightningBoltPrefab = magics[3];
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
