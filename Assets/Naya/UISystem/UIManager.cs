#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;


[System.Serializable]
public class UIEntry
{
    [SerializeField, HideInInspector] public string name;
    public UIManager.UIType type;
    public UIManager.UILayer layer = UIManager.UILayer.MAIN;
    public UIBase prefab;
    public bool pauseGame = false;
    public bool enableInput = true;
    public bool allowEscape = true;
    public bool isForceHideOtherUIShow = false;
}
public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public enum UILayer
    {
        MAIN,
        POPUP
    }
    public enum UIType
    {
        MAINMENU,
        SETTINGS,
        CREDIT,
        STAGESELECTION,
        SHIFTSELECTION,
        LEVELSELECTION,
        GAMEPLAY,
        PAUSEMENU,
        GAMEOVER
    }

    [Header("Information")]
    [SerializeField] private UIType currentUI = UIType.MAINMENU;
    [SerializeField] private UIType previousUI;
    public UIType CurrentUI { get => currentUI; }
    public UIType PreviousUI { get => previousUI; }

    [Header("Atribut[Need To Set]")]
    [SerializeField] private Transform parent;
    [SerializeField] public List<UIEntry> uiEntries;
#if UNITY_EDITOR
    private void OnValidate()
    {
        var enumValues = System.Enum.GetValues(typeof(UIType));
        if (uiEntries.Count != enumValues.Length)
        {
            Debug.LogWarning("UIEntries count does not match UIType enum count.");
        }
        else
        {
            for (int i = 0; i < uiEntries.Count; i++)
            {
                if (uiEntries[i].type != (UIType)enumValues.GetValue(i))
                {
                    Debug.LogWarning($"UIEntry at index {i} ({uiEntries[i].type}) does not match UIType enum order ({(UIType)enumValues.GetValue(i)}).");
                }
            }
        }
        for (int i = 0; i < uiEntries.Count; i++)
        {
            uiEntries[i].name = uiEntries[i].type.ToString();
        }
    }
#endif
    private Dictionary<UIType, UIBase> uiInstances = new();
    private Dictionary<UIType, UIEntry> uiConfigs = new();

    [Space]
    [SerializeField] private UIConfirmation confirmationPrefab;
    [SerializeField, ReadOnly] private UIConfirmation confirmation;

    [SerializeField] private UIShiftInformation shiftInformationPrefab;
    [SerializeField, ReadOnly] private UIShiftInformation shiftInformation;

    [SerializeField] private UITutorial UITutorialPrefab;
    [SerializeField, ReadOnly] private UITutorial UITutorial;

    // public UIBase debuggingJancok;

    public bool IsUIActive(UIType type)
    {
        if (uiInstances.TryGetValue(type, out var ui))
        {
            return ui.IsActive;
        }
        return false;
    }
    public override void Awake()
    {
        base.Awake();
        InitUI();
    }
    void Start()
    {
        // ShowUI(currentUI, true);
        StartCoroutine(LateShowUI());
    }
    public IEnumerator LateShowUI()
    {
        yield return null;
        yield return null;
        yield return null;

        ShowUI(currentUI, true);
    }
    public void InitUI()
    {
        foreach (var entry in uiEntries)
        {
            if (uiConfigs.ContainsKey(entry.type))
            {
                Debug.LogWarning($"Duplicate UI type: {entry.type}");
                continue;
            }
            if (entry.prefab == null) continue;

            UIBase instance = Instantiate(entry.prefab, parent);
            if (currentUI != entry.type) instance.Hide();

            uiInstances[entry.type] = instance;
            uiConfigs[entry.type] = entry;
        }

        confirmation = Instantiate(confirmationPrefab, parent); confirmation.Hide();
        UITutorial = Instantiate(UITutorialPrefab, parent); UITutorial.Hide();
        shiftInformation = Instantiate(shiftInformationPrefab, parent); shiftInformation.Hide();
    }
    public void ChangeUI(UIType toUI)
    {
        Debug.Log($"Change ui to {toUI}");
        ShowUI(toUI);
        if (currentUI != toUI)
        {
            HideUI(currentUI, toUI);
            previousUI = currentUI;
        }

        currentUI = toUI;
    }

    public void ShowUI(UIType toUI, bool forceShow = false)
    {
        if (!uiInstances.TryGetValue(toUI, out var ui))
        {
            Debug.LogError($"UI not found: {toUI}");
            return;
        }

        var config = uiConfigs[toUI];
        GameManager.Instance.PauseGame(config.pauseGame);
        GameManager.Instance.SetInput(!config.enableInput);
        GameManager.Instance.SetEscape(config.allowEscape);
        Cursor.lockState = CursorLockMode.None;


        if (!ui.IsActive || forceShow)
            ui.Show();
    }

    public void HideUI(UIType currentUI, UIType toUI)
    {
        if (uiInstances.TryGetValue(currentUI, out var instance))
        {
            if (uiConfigs[toUI].layer == UILayer.MAIN)
                instance.Hide();
            if (uiConfigs[toUI].layer == UILayer.POPUP && uiConfigs[currentUI].layer == UILayer.POPUP)
                instance.Hide();
            if (uiConfigs[toUI].isForceHideOtherUIShow)
                instance.Hide();

            // if (uiConfigs[currentUI].isHideWhenOtherUIShow)
            //     instance.Hide();
        }
    }

    #region UI Confirmation
    public void ShowConfirmation(ConfirmationClient informationClient)
    {
        if (confirmation == null) return;

        Debug.Log("ui information show (Not null)");
        // GameManager.Instance.SetInput(false);
        // GameManager.Instance.SetEscape(false);

        if (informationClient.IsGamePause)
        {
            GameManager.Instance.PauseGame(true);
            informationClient.OnYesConfirmation.AddListener(() => GameManager.Instance.PauseGame(false));
        }


        confirmation.InitConfirmation(informationClient);
        confirmation.Show();
    }
    public void HideConfirmation()//bukan hide, tapi lebih ke ngembalikan setting ke semua
    {
        ChangeUI(currentUI);
        confirmation.Hide();
    }
    #endregion
    #region UI Tutorial
    public void ShowTutorial(TutorialClient informationClient)
    {
        if (UITutorial == null) return;

        Debug.Log("ui information show (Not null)");
        // GameManager.Instance.SetEscape(false);

        if (informationClient.IsGamePause)
        {
            GameManager.Instance.PauseGame(true);
            GameManager.Instance.SetInput(false);
            GameManager.Instance.SetEscape(false);
        }

        UITutorial.InitConfirmation(informationClient);
        UITutorial.Show();
    }
    public void HideTutorial()//bukan hide, tapi lebih ke ngembalikan setting ke semua
    {
        ChangeUI(currentUI);
        UITutorial.Hide();
    }
    #endregion
    #region UI Shift Information
    public void ShowShiftInformation(ShiftSO shiftSO, Transform informationClient)
    {
        if (shiftInformation == null) return;

        Debug.Log("ui shift information show (Not null)");

        shiftInformation.InitConfirmation(shiftSO, informationClient);
        shiftInformation.Show();
    }
    public void HideShiftInformation()//bukan hide, tapi lebih ke ngembalikan setting ke semua
    {
        shiftInformation.Hide();
    }
    #endregion
    #region UI Level Selection
    public void ShowLevelSelection(ShiftSO shiftSO)
    {
        uiInstances[UIType.LEVELSELECTION].GetComponent<UILevelSelection>().InitLevel(shiftSO);
        ChangeUI(UIType.LEVELSELECTION);
    }
    #endregion
}


