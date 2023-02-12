using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHidingSpotSFX : MonoBehaviour
{
    [Header("Wwise")]
    [SerializeField]
    private AK.Wwise.Event hidingSpotEnteredSFXEvent;
    [SerializeField]
    private AK.Wwise.Event hidingSpotExitedSFXEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AkSoundEngine.PostEvent(hidingSpotEnteredSFXEvent.Id, this.gameObject);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        AkSoundEngine.PostEvent(hidingSpotExitedSFXEvent.Id, this.gameObject);

    }
}
