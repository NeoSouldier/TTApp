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
    [SerializeField] private YoutubePlayer m_youtubePlayer;
    [SerializeField] private VideoPlayer m_videoPlayer;

    private AppState m_appState;
    private GameObject[] screens;
    private int m_currVideoIndex;

    // **************************
    // Public functions
    // **************************

	public void Start () 
    {
        Init();
        SetState((int)AppState.kMainMenu);
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
        m_videoPlayer.targetCameraAlpha = 1;
    }

    public void VideoFinished()
    {
        m_canvasTopLevel.SetActive(true);
        m_videoPlayer.targetCameraAlpha = 0;
        ShowScreen((int)m_appState);
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
    }

    private void PlayVideoInternal(int videoIndex)
    {
        bool isNewVideo = m_currVideoIndex != videoIndex;
        if (isNewVideo) // load new video
        {
            string videoURL = "https://www.youtube.com/watch?v=y8Kyi0WNg40";
            m_youtubePlayer.LoadYoutubeVideo(videoURL);
        }
        else
        {
            m_youtubePlayer.PlayButton();
        }
    }
}