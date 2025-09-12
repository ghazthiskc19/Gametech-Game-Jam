using UnityEngine;
using TMPro;
public class TMPTextManager : MonoBehaviour
{
    public TMP_FontAsset font;
    void Start()
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);

        foreach (TMP_Text text in allTexts)
        {
            text.font = font;
        }
    }
    [ContextMenu("Ganti semua font")]
    public void changeFont()
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);
        foreach (TMP_Text text in allTexts)
        {
            text.font = font;
        }
    }
}
