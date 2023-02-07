using UnityEngine;

public class GameManager : MonoBehaviour
{
    // gameFirstBooted
    public bool isGameOver = false; 
    public bool playerHasSnack = false;

    public static GameManager current;

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
        current = this;
        GameEvents.current.onSnackObtained += OnSnackObtained;
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
        isGameOver = state; 
    }

}
