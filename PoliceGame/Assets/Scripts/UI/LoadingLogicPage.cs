using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingLogicPage : MonoBehaviour
{

    public static LoadingLogicPage LLPage;
    public GameObject LoadinScreen; 
    public float FakeLoadTime = 1f;
    public Slider ProgressBarSlider;

    private void Awake()
    {
        LoadinScreen.SetActive(false);
        MakeThisTheOnlyLoadingLogicPage();
    }

    // Destroy copy of LoadingLogic
    private void MakeThisTheOnlyLoadingLogicPage()
    {
        if (LLPage == null)
        {
            DontDestroyOnLoad(gameObject);
            LLPage = this;
        }
        else
        {
            if (LLPage != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void LoadScene(string sceneName, GameObject[] elementsToHideWhenLoading)
    {
        StartCoroutine(LoadGameSceneCor(sceneName, elementsToHideWhenLoading));
    }

    private IEnumerator LoadGameSceneCor(string sceneName, GameObject[] elementsToHideWhenLoading)
    {
        Time.timeScale = 1f;

        foreach (var element in elementsToHideWhenLoading)
        {
            element.SetActive(false);
        }

        LoadinScreen.SetActive(true);

        var asyncLoading = SceneManager.LoadSceneAsync(sceneName);
        asyncLoading.allowSceneActivation = false;

        float timer = 0;

        while (timer < FakeLoadTime || asyncLoading.progress < 0.9f)
        {
            timer += Time.deltaTime;
            SetProgressBarProgress(timer / FakeLoadTime);
            yield return null;
        }

        LoadinScreen.SetActive(false);

        asyncLoading.allowSceneActivation = true;
    }

    private void SetProgressBarProgress(float progress)
    {
        ProgressBarSlider.value = progress;
        ProgressBarSlider.GetComponentInChildren<TextMeshProUGUI>().text = (progress * 100).ToString("0");
    }

}
