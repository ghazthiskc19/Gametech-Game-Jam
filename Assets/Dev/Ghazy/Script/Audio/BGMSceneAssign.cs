using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMSceneAssign : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayBGM("Main_Menu");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                AudioManager.instance.PlayBGM("Main_Menu");
                break;
            case "TutorialScene":
            case "EPS_1_GamePlay":
            case "EPS_2_GamePlay":
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
