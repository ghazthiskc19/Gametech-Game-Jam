using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManagerNaya : MonoBehaviour
{
    public static GameManagerNaya instance;
    public InputActionReference ESCMain;
    public GameObject mainMenu;
    public GameObject leveManager;
    public GameObject CrossFade;
    public bool mainMenuAppear = false;
    public void Play(String SceneName)
    {
        LevelManager.Instance.LoadScene(SceneName, "CrossFade");
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        EventManager.instance.OnPlayerDied += ChangeDiedScene;
    }
    private void OnDisable() {
        EventManager.instance.OnPlayerDied -= ChangeDiedScene;  
    }

    public void ChangeDiedScene()
    {
        leveManager.SetActive(true);
        CrossFade.SetActive(true);
        LevelManager.Instance.LoadScene("Dead", "CrossFade");
    }
    public void ChangeWinSceneEps1()
    {
        leveManager.SetActive(true);
        CrossFade.SetActive(true);
        LevelManager.Instance.LoadScene("EPS_1_EndCutScene", "CrossFade");
    }
    public void ChangeWinSceneEps2()
    {
        leveManager.SetActive(true);
        CrossFade.SetActive(true);
        LevelManager.Instance.LoadScene("EPS_2_EndCutScene", "CrossFade");
    }
    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            mainMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            mainMenu.SetActive(false);
        }
    }
    public void ESCHandler()
    {
        Debug.Log("ESC Pressed");
        if (UIManager.Instance.CurrentUI == UIManager.UIType.GAMEPLAY) UIManager.Instance.ChangeUI(UIManager.UIType.PAUSEMENU);
        else if (UIManager.Instance.CurrentUI == UIManager.UIType.PAUSEMENU) UIManager.Instance.ChangeUI(UIManager.UIType.GAMEPLAY);
        else UIManager.Instance.ChangeUI(UIManager.Instance.PreviousUI);
    }

}
