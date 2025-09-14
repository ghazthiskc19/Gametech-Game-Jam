using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioButtonAssign : MonoBehaviour
{
    [SerializeField] private string sfxName = "tombol";
    [SerializeField] private bool preventOverlap = false;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (Button btn in allButtons)
        {
            btn.onClick.RemoveListener(() => PlayClick());
            btn.onClick.AddListener(() => PlayClick());
        }

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (Button btn in allButtons)
        {
            btn.onClick.RemoveListener(() => PlayClick());
            btn.onClick.AddListener(() => PlayClick());
        }
    }
    private void PlayClick()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(sfxName, preventOverlap);
        }
    }
}