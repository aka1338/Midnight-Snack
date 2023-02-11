using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // gameFirstBooted
    public bool isGameOver = false; 
    public bool playerHasSnack = false;
    public CanvasGroup gameOverCanvas; 

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

    // Should track mama's state here. 
    private MAMA_STATE mamaState = MAMA_STATE.IDLE; 

    public void SetMamaState(MAMA_STATE state)
    {
        mamaState = state;
    }

    private void Start()
    {
        gameOverCanvas.alpha = 0; 
        current = this;
        GameEvents.current.onSnackObtained += OnSnackObtained;
        GameEvents.current.onGameOver += SetGameOver; 
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
        gameOverCanvas.alpha = 1; 

        if (state)
        {
            Debug.Log("You won!");
        }
        else
        {
            Debug.Log("You lost!");
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
