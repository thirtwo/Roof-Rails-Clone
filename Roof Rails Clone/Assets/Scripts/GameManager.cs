using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStarted;
    public static event Action<bool> OnGameFinished;
    public static bool isGameStarted = false;
    public static bool isGameFinished = false;
    public static int scoreMultiplier = 0;
    private static int money = 0;
    public static int Money
    {
        get { return money; }
        set
        {
            UIManager.Instance.UpdateDiamonds(value);
            money = value;
        }
    }
    private void Awake()
    {
        isGameStarted = false;
        isGameFinished = false;
    }
    public static void StartGame()
    {
        if (isGameStarted) return;
        isGameStarted = true;
        OnGameStarted?.Invoke();
    }
    public static void FinishGame(bool isWin)
    {
        if (isGameFinished) return;
        isGameFinished = true;
        OnGameFinished?.Invoke(isWin);
    }

    public void NextLevel()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);//only have one scene
    }
    public void Retry()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
}
