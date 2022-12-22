using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class CreditsController : MonoBehaviour
{

    private VideoPlayer videoPlayer;

    [SerializeField]
    private VideoClip paellaClip;
    [SerializeField]
    private VideoClip cryClip;
    [SerializeField]
    private VideoClip creditsClip;

    [SerializeField]
    private int videoIndex = 0;

    private bool canPlay = true;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void FixedUpdate()
    {
        CheckIfNextVideo();
    }

    private void CheckIfNextVideo() 
    {
        if (!videoPlayer.isPlaying)
        {
            if (canPlay)
            {


                switch (videoIndex)
                {
                    case 0:
                        videoPlayer.clip = paellaClip;
                        break;
                    case 1:
                        videoPlayer.clip = cryClip;
                        break;
                    case 2:
                        //videoPlayer.clip = creditsClip;
                        break;
                    case 4:
                        //Ir al menu
                        Debug.Log("acabo");
                        Application.Quit();
                        break;
                    default:
                        break;
                }

                videoPlayer.Play();

                videoIndex++;
                canPlay = false;
            }
        }
        else
        {
            canPlay = true;
        }
    }
}
