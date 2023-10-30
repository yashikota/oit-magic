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
        var flameShowerPrefab = magics[0];
        var flameShower = Instantiate(flameShowerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        if (flameShower.TryGetComponent<ParticleSystem>(out var flameShowerParticle))
        {
            flameShowerParticle.Play();
            var duration = flameShowerParticle.main.duration;
            Destroy(flameShower, duration / 2);
        }
        else
        {
            Destroy(flameShower);
        }
    }

    private void Aqua()
    {
        var waterTornadoPrefab = magics[2];
        var waterTornado = Instantiate(waterTornadoPrefab, new Vector3(0, 0, 0), Quaternion.identity);

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
        var tornadoLoopPrefab = magics[3];
        var tornadoLoop = Instantiate(tornadoLoopPrefab, new Vector3(0, 0, 0), Quaternion.identity);

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
