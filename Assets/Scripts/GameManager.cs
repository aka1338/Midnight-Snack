using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // gameFirstBooted
    public bool isGameOver = false; 
    public bool playerHasSnack = false;

    public static GameManager current;

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
        // And then spawn the ghost. 
    }


}
