using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    [SerializeField] private Canvas _bgDimCanvas;
    private Image _bgDimImage;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_bgDimCanvas);
            _bgDimImage = _bgDimCanvas.GetComponentInChildren<Image>();
        }
    }

    public void LoadNextScene() => StartCoroutine(LoadNextSceneAsync());

    // this is a mess but looks cool
    public IEnumerator LoadNextSceneAsync()
    {
        bool isFadeFinished = false;
        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currSceneIndex + 1;
        if (SceneManager.GetSceneByBuildIndex(nextSceneIndex).name != Scenes.LOADING_SCREEN)
        {
            LeanTween.alpha(_bgDimImage.rectTransform, 255, 3f)
                .setEase(LeanTweenType.easeInCubic)
                .setOnComplete(() => { isFadeFinished = true; });

            yield return new WaitUntil(() => isFadeFinished);
            isFadeFinished = false;
            var asyncLoad = SceneManager.LoadSceneAsync(Scenes.LOADING_SCREEN);

            LeanTween.alpha(_bgDimImage.rectTransform, 0, 2f)
                .setEase(LeanTweenType.easeOutCubic)
                .setOnComplete(() => { isFadeFinished = true; });

            yield return new WaitUntil(() => isFadeFinished);

            asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);
            asyncLoad.allowSceneActivation = false;

            yield return new WaitUntil(() => asyncLoad.progress >= 0.95f);

            LeanTween.alpha(_bgDimImage.rectTransform, 255, 3f)
                    .setEase(LeanTweenType.easeInCubic)
                    .setOnComplete(() => { asyncLoad.allowSceneActivation = true; });

            yield return new WaitUntil(() => asyncLoad.isDone);

            LeanTween.alpha(_bgDimImage.rectTransform, 0, 2f)
                    .setEase(LeanTweenType.easeOutCubic);
        }
    }

    public void LoadGameplay()
    {
        StartCoroutine(LoadGameplayAsync());
    }

    public void LoadEndScene()
    {
        StartCoroutine(LoadEndGameScreenAsync());
    }

    /*public void LoadLoadingScreen()
    {
        SceneManager.LoadScene(Scenes.LOADING_SCREEN);
    }*/

    public IEnumerator LoadGameplayAsync()
    {
        SceneManager.LoadSceneAsync(Scenes.LOADING_SCREEN);

        yield return new WaitForSeconds(1);

        var asyncLoad = SceneManager.LoadSceneAsync(Scenes.GAME);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        //SceneManager.UnloadSceneAsync(Scenes.MAIN_MENU);
        //SceneManager.UnloadSceneAsync(Scenes.LOADING_SCREEN);
    }

    public IEnumerator LoadEndGameScreenAsync()
    {
        SceneManager.LoadSceneAsync(Scenes.LOADING_SCREEN);

        yield return new WaitForSeconds(1);

        var asyncLoad = SceneManager.LoadSceneAsync(Scenes.MAIN_MENU); // test

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
