using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuPage : MonoBehaviour
{

    public GameObject[] ElementsToHideWhenLoading;

    public string[] SceneNames;

    public Button[] PressButtons;

    private LoadingLogicPage LoadingLogic;


    private void Awake()
    {
        LoadingLogic = GameObject.Find("LoadingLogic").GetComponent<LoadingLogicPage>();

        for (int i=0; i<PressButtons.Length; i++)
        {
            var sceneName = SceneNames[i];
            PressButtons[i].onClick.AddListener(() => { PlayGame(sceneName); });
        }
        
    }

    private void PlayGame(string sceneName)
    {
        LoadingLogic.LoadScene(sceneName, ElementsToHideWhenLoading);
    }

    

}
