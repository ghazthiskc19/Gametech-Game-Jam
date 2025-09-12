using UnityEngine;

public class GameManager : MonoBehaviour
{

    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }

    public void ESCHandler()
    {
        Debug.Log("ESC Pressed");
        if (UIManager.Instance.CurrentUI == UIManager.UIType.GAMEPLAY) UIManager.Instance.ChangeUI(UIManager.UIType.PAUSEMENU);
        else if (UIManager.Instance.CurrentUI == UIManager.UIType.PAUSEMENU) UIManager.Instance.ChangeUI(UIManager.UIType.GAMEPLAY);
        else UIManager.Instance.ChangeUI(UIManager.Instance.PreviousUI);
    }
}
