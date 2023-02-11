using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSnack : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false); 
    }
}
