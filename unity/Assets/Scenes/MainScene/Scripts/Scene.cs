using System.Linq;
using UnityEngine;

public class Scene : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject gameClear;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject Ui;
    [SerializeField] private Timer timer;
    [SerializeField] private Ranking ranking;
    [SerializeField] private PlayLog playLog;
    [SerializeField] private Pointer pointer;

    public void Title()
    {
        Ui.SetActive(false);
        InactiveTargets();
        title.SetActive(true);
    }

    public void GameClear()
    {
        GameEnd();
        gameClear.SetActive(true);
        ranking.UpdateRanking(timer.GetTime());
    }

    public void GameOver()
    {
        GameEnd();
        gameOver.SetActive(true);
        PlayLog.IsGameOver = true;
    }

    private void GameEnd()
    {
        Ui.SetActive(false);
        InactiveTargets();
        timer.TimerStop();
        playLog.Save();
    }

    private void InactiveTargets()
    {
        var targets = GameObject.FindGameObjectsWithTag("Target");
        targets.ToList().ForEach(target => target.SetActive(false));
    }

    public void OnClickStart()
    {
        title.SetActive(false);
        Ui.SetActive(true);
        gameManager.GameStart();
    }

    public void OnClickEnd()
    {
        gameManager.Reset();
        title.SetActive(true);
        gameClear.SetActive(false);
        gameOver.SetActive(false);
        pointer.ResetPosition();
    }
}
