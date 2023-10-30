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
        Debug.Log("Fire");

        var flameshrowerPrefab = magics[0];
        var flameshrower = Instantiate(flameshrowerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var flameshrowerParticle = flameshrower.GetComponent<ParticleSystem>();

        if (flameshrowerParticle != null)
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
        Debug.Log("Aqua");
    }

    private void Wind()
    {
        Debug.Log("Wind");
    }

    private void Lightning()
    {
        Debug.Log("Lightning");
    }
}
