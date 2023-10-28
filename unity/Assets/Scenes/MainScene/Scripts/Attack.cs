using UnityEngine;

public class Attack : MonoBehaviour
{
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
