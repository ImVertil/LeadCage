using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    public void LoadGameplay()
    {
        StartCoroutine(LoadGameplayAsync());
    }

    public void LoadLoadingScreen()
    {
        SceneManager.LoadScene(Scenes.LOADING_SCREEN);
    }
    public IEnumerator LoadGameplayAsync()
    {
        yield return new WaitForSeconds(1);

        var asyncLoad = SceneManager.LoadSceneAsync(Scenes.GAME);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(Scenes.MAIN_MENU);
    }
}
