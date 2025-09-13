using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMSceneAssign : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayBGM("beranda game");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenuScene":
                AudioManager.instance.PlayBGM("beranda game");
                break;
            case "TutorialScene":
            case "GameplayScene":
                AudioManager.instance.PlayBGM("In Game");
                break;
            default:
                AudioManager.instance.StopBGM();
                break;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
