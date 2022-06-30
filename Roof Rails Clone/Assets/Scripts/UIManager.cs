using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;
    [Header("UI Settings")]
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject gameInCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    [Header("Diamond Settings")]
    [SerializeField] private GameObject diamondPrefab;
    [SerializeField] private RectTransform moneyTransform;
    [SerializeField]private RectTransform gameInCanvasTransform;
    [SerializeField] private TextMeshProUGUI moneyText;

    private Vector2 uiOffset;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        uiOffset = new Vector2(gameInCanvasTransform.sizeDelta.x / 2f, gameInCanvasTransform.sizeDelta.y / 2f);
        gameInCanvas.SetActive(false);
        GameManager.OnGameStarted += GameManager_OnGameStarted;
        GameManager.OnGameFinished += GameManager_OnGameFinished;
    }
    private void OnDisable()
    {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
        GameManager.OnGameFinished -= GameManager_OnGameFinished;
    }
    private void GameManager_OnGameFinished(bool isWin)
    {
        gameInCanvas.SetActive(false);
        if (isWin)
        {
            winCanvas.SetActive(true);
            //Set win canvas
        }
        else
        {
            loseCanvas.SetActive(true);
        }
    }

    private void GameManager_OnGameStarted()
    {
        startCanvas.SetActive(false);
        gameInCanvas.SetActive(true);
    }

    public void UpdateDiamonds(int diamondCount)
    {
        moneyText.text = "<sprite index=23>" + diamondCount;
    }
    public void SpawnDiamond(Vector3 objPosition)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objPosition);
        var proportionalPosition = new Vector2(ViewportPosition.x * gameInCanvasTransform.sizeDelta.x, ViewportPosition.y * gameInCanvasTransform.sizeDelta.y);
        var diamond = Instantiate(diamondPrefab, gameInCanvasTransform);
        var rect = diamond.GetComponent<RectTransform>();
        rect.localPosition = proportionalPosition - uiOffset;
        rect.DOMove(moneyTransform.position, 1);
        StartCoroutine(KillAfterTween(diamond));
    }
    private IEnumerator KillAfterTween(GameObject obj)
    {
        yield return new WaitForSeconds(1);
        obj.SetActive(false);
    }
}
