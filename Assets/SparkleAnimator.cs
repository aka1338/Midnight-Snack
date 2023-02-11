using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleAnimator : MonoBehaviour
{
    public List<Sprite> sparkleSprites;
    public SpriteRenderer spriteRenderer;
    public float frameRate = 10;
    private float currentTime = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTime = 0;
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        int frameIndex = (int)(currentTime * frameRate) % sparkleSprites.Count;
        spriteRenderer.sprite = sparkleSprites[frameIndex];
    }

    // If the player stays within this sprite's zone, we do not animate it. 

}
