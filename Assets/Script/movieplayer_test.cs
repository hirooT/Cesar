using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class movieplayer_test : MonoBehaviour {

    public VideoPlayer videoplayer_1;
    public static bool fadeout = false;
    //public VideoPlayer videoplayer_2;

    double videolength;

	// Use this for initialization
	void Start () {
        //videoplayer_1.Pause();
        //videoplayer_2.Pause();

    }
	
	// Update is called once per frame
	void Update () {

        videolength = videoplayer_1.time;
        //Debug.Log(videolength);
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (videoplayer_1.isPlaying)
            {
                videoplayer_1.Pause();
            }
            else videoplayer_1.Play();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //videoplayer_1.Stop();
            videoplayer_1.targetCameraAlpha = 0;
            //videoplayer_2.Play();

        }
        if (!fadeout)
        {
            if (videoplayer_1.targetCameraAlpha < 1)
            {
                StartCoroutine(fadein());
            }
        }

        if(videolength > 60f && fadeout == true)
        {

                if (videoplayer_1.targetCameraAlpha > 0)
                {
                    StartCoroutine(FadeOut());
                }
        }
        
	}
    IEnumerator fadein()
    {
        videoplayer_1.targetCameraAlpha += Time.deltaTime * 0.6f;
        if (videoplayer_1.targetCameraAlpha > 1) {
            videoplayer_1.targetCameraAlpha = 1;
            fadeout = true;
        }     
        yield return null;
    }
    IEnumerator FadeOut()
    {
        videoplayer_1.targetCameraAlpha -= Time.deltaTime * 0.6f;
        if (videoplayer_1.targetCameraAlpha < 0) {
            videoplayer_1.targetCameraAlpha = 0;
            yield return new WaitForSeconds(1.0f);
            fadeout = false;
        }
        
        yield return null;
        
    }
}
