using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDirector : MonoBehaviour 
{

    // **************************
    // Member Variables
    // **************************

    public enum AppState
    {
        kStartScreen   // Start screen
    }
        
    //[SerializeField] private GameObject m_mainMenu;

    private AppState m_appState;

    // **************************
    // Public functions
    // **************************

	public void Start () 
    {
        m_appState = AppState.kStartScreen;
	}
	
	public void Update () 
    {
        // empty
	} 

    // **************************
    // Private/Helper functions
    // **************************
        
    private void Empty()
    {
        //
    }
}