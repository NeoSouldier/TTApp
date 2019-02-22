using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AppDirector : MonoBehaviour 
{

    // **************************
    // Member Variables
    // **************************

    public enum AppState
    {
        kStartScreen = 0,   // Start screen
        kMainMenu = 1,      // Main Menu
        kBeginner = 2,      // Beginner
        kIntermediate = 3,  // Intermediate
        kAdvanced = 4,      // Advanced
        kMax = 5
    }

    [SerializeField] private GameObject m_canvasTopLevel;
    [SerializeField] private GameObject m_startScreen;
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_beginner;
    [SerializeField] private GameObject m_intermediate;
    [SerializeField] private GameObject m_advanced;
    //[SerializeField] private YoutubePlayer m_youtubePlayer;
    [SerializeField] private Vimeo.Player.VimeoPlayer m_vimeoPlayer;
    [SerializeField] private VideoPlayer m_vimeoVideoPlayer;
    [SerializeField] private VideoPlayer m_clipVideoPlayer; // Currently separating youtube Video Player from Embedded videos...
    [SerializeField] private UnityEngine.Video.VideoClip m_introVideoClip;
    [SerializeField] private UnityEngine.Video.VideoClip m_beginnerVideoClip;
    [SerializeField] private UnityEngine.Video.VideoClip m_intermediateVideoClip;
    [SerializeField] private UnityEngine.Video.VideoClip m_advancedVideoClip;

    private AppState m_appState;
    private GameObject[] screens;
    private int m_currVideoIndex;

    // **************************
    // Public functions
    // **************************

	public void Start () 
    {
        Init();

        if (!PlayerPrefs.HasKey("IntroVideoPlayed")) 
        {            
            SetState((int)AppState.kStartScreen);
            m_clipVideoPlayer.clip = m_introVideoClip;
            m_clipVideoPlayer.Play();
            m_clipVideoPlayer.loopPointReached += IntroVideoFinished;
            m_clipVideoPlayer.targetCameraAlpha = 1; 
            PlayerPrefs.SetInt("IntroVideoPlayed", 1);
        }
        else
        {
            SetState((int)AppState.kMainMenu);
        }
	}

    public void SetState(int state)
    {
        SetStateInternal((AppState)state);
    }

    public void PlayVideo(int videoIndex)
    {
        HideScreen();
        PlayVideoInternal(videoIndex);
    }

    public void VideoStarted()
    {
        m_canvasTopLevel.SetActive(false);
        m_vimeoVideoPlayer.targetCameraAlpha = 1;
        Debug.Log("[AppDirector] VideoStarted()");
    }

    public void VideoFinished(VideoPlayer videoPlayer)
    {
        m_canvasTopLevel.SetActive(true);
        m_vimeoVideoPlayer.targetCameraAlpha = 0;
        ShowScreen((int)m_appState);
        Debug.Log("[AppDirector] VideoFinished()");
    }

    // **************************
    // Private/Helper functions
    // **************************

    private void Init()
    {
        screens = new GameObject[(int)AppState.kMax];
        screens[(int)AppState.kStartScreen] = m_startScreen;
        screens[(int)AppState.kMainMenu] = m_mainMenu;
        screens[(int)AppState.kBeginner] = m_beginner;
        screens[(int)AppState.kIntermediate] = m_intermediate;
        screens[(int)AppState.kAdvanced] = m_advanced;

        m_currVideoIndex = -1;

        m_vimeoPlayer.OnVideoStart += VideoStarted;
        m_vimeoPlayer.OnPlay += VideoStarted;
        m_vimeoVideoPlayer.loopPointReached += VideoFinished;
    }

    private void ShowScreen(int screenNumber)
    {
        for (int i = 0; i < (int)AppState.kMax; i++)
        {
            screens[i].SetActive(i == screenNumber);
        }
    }

    private void HideScreen()
    {
        ShowScreen(-1);
    }

    private void SetStateInternal(AppState appState)
    {
        m_appState = appState;
        ShowScreen((int)m_appState);

        if (m_appState == AppState.kBeginner && !PlayerPrefs.HasKey("BeginnerVideoPlayed"))
        {
            m_clipVideoPlayer.clip = m_beginnerVideoClip;
            m_clipVideoPlayer.Play();
            m_clipVideoPlayer.loopPointReached += NormalVideoFinished;
            m_clipVideoPlayer.targetCameraAlpha = 1; 
            m_canvasTopLevel.SetActive(false);
            PlayerPrefs.SetInt("BeginnerVideoPlayed", 1);
        }
        else if (m_appState == AppState.kIntermediate && !PlayerPrefs.HasKey("IntermediateVideoPlayed"))
        {
            m_clipVideoPlayer.clip = m_intermediateVideoClip;
            m_clipVideoPlayer.Play();
            m_clipVideoPlayer.loopPointReached += NormalVideoFinished;
            m_clipVideoPlayer.targetCameraAlpha = 1; 
            m_canvasTopLevel.SetActive(false);
            PlayerPrefs.SetInt("IntermediateVideoPlayed", 1);
        }
        else if (m_appState == AppState.kAdvanced && !PlayerPrefs.HasKey("AdvancedVideoPlayed"))
        {
            m_clipVideoPlayer.clip = m_advancedVideoClip;
            m_clipVideoPlayer.Play();
            m_clipVideoPlayer.loopPointReached += NormalVideoFinished;
            m_clipVideoPlayer.targetCameraAlpha = 1;
            m_canvasTopLevel.SetActive(false);
            PlayerPrefs.SetInt("AdvancedVideoPlayed", 1);
        }
    }

    private void PlayVideoInternal(int videoIndex)
    {
        bool isNewVideo = m_currVideoIndex != videoIndex;
        if (isNewVideo) // load new video
        {
            string videoURL = "https://vimeo.com/318340152";            
            m_vimeoPlayer.PlayVideo(videoURL);
            m_currVideoIndex = videoIndex;            
        }
        else
        {
            m_vimeoPlayer.Play();
        }
    }
    private void IntroVideoFinished(UnityEngine.Video.VideoPlayer videoPlayer)
    {
        videoPlayer.targetCameraAlpha = 0;
        SetState((int)AppState.kMainMenu);
    }

    private void NormalVideoFinished(UnityEngine.Video.VideoPlayer videoPlayer)
    {        
        m_canvasTopLevel.SetActive(true);
        videoPlayer.targetCameraAlpha = 0;        
    }
}