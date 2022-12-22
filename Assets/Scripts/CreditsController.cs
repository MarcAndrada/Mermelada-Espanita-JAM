using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{

    private VideoPlayer videoPlayer;

    [SerializeField]
    private VideoClip paellaClip;
    [SerializeField]
    private VideoClip cryClip;
    [SerializeField]
    private GameObject creditsCanvas;

    [SerializeField]
    private int videoIndex = 0;

    private bool canPlay = true;
    [SerializeField]
    private AudioSource audioSource1;
    [SerializeField]
    private AudioSource audioSource2;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }


    private void Update()
    {
        if (videoIndex >= 3 && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
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
                        creditsCanvas.SetActive(true);
                        audioSource2.Stop();
                        audioSource1.Play();
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
