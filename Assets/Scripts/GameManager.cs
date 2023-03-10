using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public CinemachineVirtualCamera[] cameras;

    public bool isGameOver = false;
    public bool playerHasSnack = false;

    [Header("Canvas Groups")]
    public CanvasGroup gameLostCanvasGroup;
    public CanvasGroup gameWonCanvasGroup;
    public CanvasGroup gameStartCanvasGroup;

    [Header("Canvas")]
    public Canvas gameLostCanvas;
    public Canvas gameWonCanvas;
    public Canvas gameStartCanvas;

    public static GameManager current;
    public GameObject snackLocation;
    public GameObject player;

    [SerializeField]
    private AK.Wwise.Event UIButtonClick;
    [SerializeField]
    private AK.Wwise.Event UIButtonHover;

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        cameras[0].gameObject.SetActive(true);
        cameras[0].Priority = 11;

        gameLostCanvasGroup.alpha = 0;
        gameLostCanvas.gameObject.SetActive(false);

        gameWonCanvasGroup.alpha = 0;
        gameWonCanvas.gameObject.SetActive(false);

        gameStartCanvasGroup.alpha = 1;
        gameStartCanvas.gameObject.SetActive(true);

        current = this;

        GameEvents.current.onSnackObtained += OnSnackObtained;
        GameEvents.current.onGameOver += SetGameOver;
    }
    // Should track mama's state here. 
    private MAMA_STATE mamaState = MAMA_STATE.IDLE;

    public void SetMamaState(MAMA_STATE state)
    {
        mamaState = state;
    }

    public void CloseOpeningScreen()
    {
        gameStartCanvasGroup.DOFade(0, 1f).SetEase(Ease.InOutQuad);
        gameStartCanvasGroup.interactable = false;
    }

    private void OnDisable()
    {

        GameEvents.current.onSnackObtained -= OnSnackObtained;
    }

    void OnSnackObtained()
    {
        playerHasSnack = true;
    }

    public void SetGameOver(bool state)
    {
        isGameOver = true;

        if (state)
        {
            gameWonCanvasGroup.interactable = true;
            gameWonCanvas.gameObject.SetActive(true);
            gameWonCanvasGroup.DOFade(1, 1f).SetEase(Ease.InOutQuad);
        }
        else
        {
            StartCoroutine(PanCameraToMomAndShake());
        }
    }

    public void RestartGame()
    {
        AkSoundEngine.StopAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void UIButtonClicked()
    {
        AkSoundEngine.PostEvent(UIButtonClick.Id, player);
    }

    IEnumerator PanCameraToMomAndShake()
    {
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        cameras[1].Priority = 11;
        yield return new WaitForSecondsRealtime(6f);
        GameEvents.current.MamaTurnLightOn(PlayerController.current.currentRoom);
        // Disable player controls 
        gameLostCanvasGroup.interactable = true;
        gameLostCanvas.gameObject.SetActive(true);
        gameLostCanvasGroup.DOFade(1, 1f).SetEase(Ease.InOutQuad);
    }

}
