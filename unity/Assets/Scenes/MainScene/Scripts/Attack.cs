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
        var flameshrowerPrefab = magics[0];
        var flameshrower = Instantiate(flameshrowerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        if (flameshrower.TryGetComponent<ParticleSystem>(out var flameshrowerParticle))
        {
            flameshrowerParticle.Play();
            var duration = flameshrowerParticle.main.duration;
            Destroy(flameshrower, duration / 2);
        }
        else
        {
            Destroy(flameshrower);
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
        var LightningBoltPrefab = magics[1];
        var LightningBolt = Instantiate(LightningBoltPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        if (LightningBolt.TryGetComponent<ParticleSystem>(out var LightningBoltParticle))
        {
            LightningBoltParticle.Play();
            var duration = LightningBoltParticle.main.duration;
            Destroy(LightningBolt, duration);
        }
        else
        {
            Destroy(LightningBolt);
        }
    }
}
