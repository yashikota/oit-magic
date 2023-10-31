using UnityEngine;

public class Scene : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject gameClear;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject UI;

    void Update()
    {
        if (gameManager.IsTitle())
        {
            UI.SetActive(false);
            title.SetActive(true);
        }
        else if (gameManager.IsGameClear())
        {
            UI.SetActive(false);
            gameClear.SetActive(true);
        }
        else if (gameManager.IsGameOver())
        {
            UI.SetActive(false);
            gameOver.SetActive(true);
        }
    }

    public void OnClickStart()
    {
        title.SetActive(false);
        UI.SetActive(true);
        gameManager.SetTitle(false);
    }

    public void OnClickEnd()
    {
        gameManager.Reset();
        title.SetActive(true);
        gameClear.SetActive(false);
        gameOver.SetActive(false);
        gameManager.SetTitle(true);
    }
}
