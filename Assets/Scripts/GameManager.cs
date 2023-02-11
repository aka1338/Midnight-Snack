using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // gameFirstBooted
    public bool isGameOver = false; 
    public bool playerHasSnack = false;
    public CanvasGroup gameLostCanvas;
    public CanvasGroup gameWonCanvas;
    public CanvasGroup gameStartCanvas;


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
        gameLostCanvas.alpha = 0;
        gameLostCanvas.interactable = false;

        gameWonCanvas.alpha = 0;
        gameWonCanvas.interactable = false;

        gameStartCanvas.alpha = 1;
        gameStartCanvas.interactable = true;

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
        Debug.Log("Trying to close window here!"); 
        gameStartCanvas.DOFade(0, 1f).SetEase(Ease.InOutQuad);
    }

    private void OnDisable()
    {

        GameEvents.current.onSnackObtained -= OnSnackObtained; 
    }

    void OnSnackObtained()
    {
        playerHasSnack = true;
    }

    public void SetGameOver(bool state) {

        isGameOver = true;

        if (state)
        {
            Debug.Log("You won!");
            gameWonCanvas.DOFade(1, 1f).SetEase(Ease.InOutQuad);
        }
        else
        {
            Debug.Log("You lost!");
            gameLostCanvas.DOFade(1, 1f).SetEase(Ease.InOutQuad);
        }
        // Display a Game Over UI that allows the player to restart the game 
    }
    public void RestartGame() {
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


}
