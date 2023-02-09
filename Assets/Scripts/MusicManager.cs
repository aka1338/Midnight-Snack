using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Mama States")]
    public AK.Wwise.Event idleStateEvent;
    public AK.Wwise.Event alertedStateEvent;
    public AK.Wwise.Event seekingPlayerEvent;
    public AK.Wwise.Event returningToBedEvent;

    [Header("Game States")]
    public AK.Wwise.Event snackObtainedEvent;
    public AK.Wwise.Event gameOverState;

    private void Start()
    {
        AkSoundEngine.PostEvent(idleStateEvent.Id, gameObject);  
        AkSoundEngine.PostEvent(alertedStateEvent.Id, gameObject);
        AkSoundEngine.PostEvent(seekingPlayerEvent.Id, gameObject);
        AkSoundEngine.PostEvent(returningToBedEvent.Id, gameObject);

    }

    private void Update()
    {
        switch (MamaController.current.mamaState)
        { 
            case MAMA_STATE.IDLE:
                AkSoundEngine.SetRTPCValue("MAMA_VOLUME", 1f);
                break;
            case MAMA_STATE.ALERTED:
                AkSoundEngine.SetRTPCValue("MAMA_VOLUME", 0.5f);
                break;
            case MAMA_STATE.SEEKING_PLAYER:
                AkSoundEngine.SetRTPCValue("MAMA_VOLUME", 0f);
                break;
            case MAMA_STATE.RETURNING_TO_BED:
                AkSoundEngine.SetRTPCValue("MAMA_VOLUME", 1f);
                break;
            default:
                break;
        }
    }
}
