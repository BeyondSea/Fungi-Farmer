using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private float timeToQuit = 1.5f;
    [SerializeField] private float quitTimer = 0f;
    [SerializeField] private GameObject quitCanvas;
     
    public void Update() 
    {
        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            quitCanvas.SetActive(true);
        }

        else if ( Input.GetKey(KeyCode.Escape) )
        {
            quitTimer += Time.deltaTime;
            
            if ( quitTimer > timeToQuit )
            {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }

        else if ( Input.GetKeyUp(KeyCode.Escape) )
        {
            quitTimer = 0f;
            quitCanvas.SetActive(false);
        }
    }
}