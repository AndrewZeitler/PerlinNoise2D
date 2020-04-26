using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimator : MonoBehaviour {
    int frameCounter = 0;
    Sprite[] frames;
    SpriteRenderer spriteRenderer;
    float frameRate;

    public void SetFrames(Sprite[] frames, float frameRate){
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        this.frames = frames;
        this.frameRate = frameRate;
        StartCoroutine(ChangeSprite());
    }

    IEnumerator ChangeSprite(){
        while(true){
            spriteRenderer.sprite = frames[frameCounter];
            ++frameCounter;
            if(frameCounter >= frames.Length) frameCounter = 0;
            yield return new WaitForSeconds(frameRate);
        }
    }
}