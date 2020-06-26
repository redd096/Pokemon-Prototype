using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    /// <summary>
    /// Exit game (works also in editor)
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Reload this scene
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Load new scene by name
    /// </summary>
    public void LoadNewScene(string scene)
    {
        //set timeScale to 1
        Time.timeScale = 1;

        //load new scene
        SceneManager.LoadScene(scene);
    }
}