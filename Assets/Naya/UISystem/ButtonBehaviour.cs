using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Ami.BroAudio;
using Ami.BroAudio.Data;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum HoverType
    {
        ScaleBounce,
        Shake
    }
    [SerializeField] private HoverType hoverType = HoverType.ScaleBounce;
    [SerializeField] private float hoverDuration = 0.2f;
    [SerializeField] private float clickDuration = 0.15f;
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private Ease hoverEase = Ease.OutBack;
    [SerializeField] private Ease returnEase = Ease.OutCubic;
    [SerializeField] private Ease clickEase = Ease.InOutCubic;

    [SerializeField] private Vector3 originalScale;
    [SerializeField] private RectTransform rectTransform;


    [Space]
    [SerializeField] private SoundID hoverSFX;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DOTween.Kill(rectTransform);

        if (hoverSFX.IsValid()) BroAudio.Play(hoverSFX);
        else AudioManager.Instance.PlaySound(Sound.ButtonHover);

        switch (hoverType)
        {
            case HoverType.ScaleBounce:
                rectTransform.DOScale(originalScale * scaleMultiplier, hoverDuration)
                    .SetEase(hoverEase)
                    .SetUpdate(true); // <- Use unscaled time
                break;
            case HoverType.Shake:
                rectTransform.DOShakeAnchorPos(
                        hoverDuration,
                        strength: new Vector2(5f, 0),
                        vibrato: 10,
                        randomness: 90,
                        snapping: false,
                        fadeOut: true)
                    .SetUpdate(true); // <- Use unscaled time
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOTween.Kill(rectTransform);

        rectTransform.DOScale(originalScale, hoverDuration)
            .SetEase(returnEase)
            .SetUpdate(true); // <- Use unscaled time
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DOTween.Kill(rectTransform);

        rectTransform.DOScale(originalScale * 0.9f, clickDuration / 2)
            .SetEase(clickEase)
            .SetUpdate(true) // <- Use unscaled time
            .OnComplete(() =>
            {
                rectTransform.DOScale(originalScale, clickDuration / 2)
                    .SetEase(clickEase)
                    .SetUpdate(true); // <- Use unscaled time
            });
    }
}
