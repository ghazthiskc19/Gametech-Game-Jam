using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.Collections;

public class UIBase : MonoBehaviour
{
    public enum ANIMATION_TYPE
    {
        NONE,
        ANIMATOR,
        SLIDE_FROM_LEFT,
        SLIDE_FROM_RIGHT,
        SLIDE_FROM_TOP,
        SLIDE_FROM_BOTTOM,
        MOVE_TO_X
    }

    [Header("Animation Settings")]
    [SerializeField] protected bool isActive = false;
    [SerializeField] protected GameObject targetObject;
    [SerializeField] protected ANIMATION_TYPE startAnimation = ANIMATION_TYPE.NONE;
    [SerializeField] protected ANIMATION_TYPE endAnimation = ANIMATION_TYPE.NONE;
    [SerializeField] protected float animationStartDuration = 0.5f;
    [SerializeField] protected float animationEndDuration = 0.5f;
    [SerializeField] protected float offscreenOffset = 500f;
    [SerializeField] protected Ease easeType = Ease.OutQuad;

    public bool IsActive { get => isActive; }

    [Header("Fade Settings")]
    [SerializeField] protected float fadeDuration = 0.2f;
    [SerializeField] protected bool isFadeIn = false;
    [SerializeField] protected bool isFadeOut = false;

    [Header("Scale Settings")]
    [SerializeField] protected float scaleDuration = 0.2f;
    [SerializeField] protected Ease scaleEaseType = Ease.OutBack;
    [SerializeField] protected bool isScaleIn = false;
    [SerializeField] protected bool isScaleOut = false;

    [Header("Animation")]
    [SerializeField, ReadOnly] protected Animator animator;
    [SerializeField] protected string animStartName = "Start";
    [SerializeField] protected string animEndName = "End";

    [Header("References")]
    [SerializeField,  Tooltip("Use for Move To X Animation type")] protected RectTransform targetMove;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Vector3 defaultPos;
    [SerializeField] private float widthUI;
    [SerializeField] private float heightUI;

    private RectTransform targetTransform;

    protected virtual void Awake()
    {
        if (targetObject == null) targetObject = gameObject;

        targetTransform = targetObject.GetComponent<RectTransform>();
        if (!canvasGroup) canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (!canvasGroup) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        animator = GetComponent<Animator>();

        defaultPos = Vector3.zero;
        widthUI = targetTransform.rect.width;
        heightUI = targetTransform.rect.height;
    }

    public virtual void Show()
    {
        Debug.Log(gameObject.name + " Show");
        gameObject.SetActive(true);
        isActive = true;
        
        PlayAnimation(startAnimation, true);
    }

    public virtual void Hide()
    {
        Debug.Log(gameObject.name + " Hide");
        PlayAnimation(endAnimation, false);
        isActive = false;
    }

    private void PlayAnimation(ANIMATION_TYPE type, bool isShowing)
    {
        targetTransform.DOKill(false);
        canvasGroup.DOKill(false);

        switch (type)
        {
            case ANIMATION_TYPE.NONE:
                if (!isShowing) gameObject.SetActive(false);
                break;

            case ANIMATION_TYPE.ANIMATOR:
                if (animator == null)
                {
                    Debug.LogWarning("Animator is not assigned for ANIMATION_TYPE.ANIMATOR.");
                    if (!isShowing) gameObject.SetActive(false);
                    break;
                }

                if (isShowing)
                {
                    Debug.Log("Play animator show");
                    animator.Play(animStartName);
                }
                else
                    StartCoroutine(PlayAnimThenDisable(animEndName));
                break;

            case ANIMATION_TYPE.SLIDE_FROM_LEFT:
                SlideFrom(new Vector3(-widthUI - offscreenOffset, defaultPos.y, 0), isShowing);
                break;

            case ANIMATION_TYPE.SLIDE_FROM_RIGHT:
                SlideFrom(new Vector3(widthUI + offscreenOffset, defaultPos.y, 0), isShowing);
                break;

            case ANIMATION_TYPE.SLIDE_FROM_TOP:
                SlideFrom(new Vector3(defaultPos.x, heightUI + offscreenOffset, 0), isShowing);
                break;

            case ANIMATION_TYPE.SLIDE_FROM_BOTTOM:
                SlideFrom(new Vector3(defaultPos.x, -heightUI - offscreenOffset, 0), isShowing);
                break;

            case ANIMATION_TYPE.MOVE_TO_X:
                MoveToX(isShowing);
                break;
        }

        Fade(isShowing);
        Scale(isShowing);
    }

    private void Fade(bool isShowing)
    {
        if (isShowing && isFadeIn)
        {
            canvasGroup.alpha = 0;
            Debug.Log("Fade in");
            canvasGroup.DOFade(1f, fadeDuration)
                .SetEase(easeType)
                .SetUpdate(true)
                .OnComplete(() => canvasGroup.alpha = 1f);
        }
        else if (isFadeOut && !isShowing)
        {
            canvasGroup.DOFade(0f, fadeDuration)
                .SetEase(easeType)
                .SetUpdate(true)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
    private void Scale(bool isShowing)
    {
        if (isShowing && isScaleIn)
        {
            targetTransform.localScale = Vector3.zero;
            targetTransform.DOScale(Vector3.one, scaleDuration)
                .SetEase(scaleEaseType)
                .SetUpdate(true);
        }
        else if (isScaleOut && !isShowing)
        {
            targetTransform.DOScale(Vector3.zero, scaleDuration)
                .SetEase(scaleEaseType)
                .SetUpdate(true)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }

    private void SlideFrom(Vector3 startOffset, bool isShowing)
    {
        if (isShowing)
        {
            Debug.Log("Slide from: " + startOffset);
            targetTransform.localPosition = startOffset;
            targetTransform.DOLocalMove(defaultPos, animationStartDuration)
                .SetEase(easeType)
                .SetUpdate(true);
        }
        else
        {
            targetTransform.DOLocalMove(startOffset, animationEndDuration)
                .SetEase(easeType)
                .SetUpdate(true)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }

    private void MoveToX(bool isShowing)
    {
        if (targetMove == null) return;

        if (isShowing)
        {
            targetTransform.localPosition = targetMove.localPosition;
            targetTransform.DOLocalMove(defaultPos, animationStartDuration)
                .SetEase(easeType)
                .SetUpdate(true);
        }
        else
        {
            targetTransform.DOLocalMove(targetMove.localPosition, animationEndDuration)
                .SetEase(easeType)
                .SetUpdate(true)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }

    private IEnumerator PlayAnimThenDisable(string animName)
    {
        animator.Play(animName);

        yield return new WaitForSecondsRealtime(animationEndDuration); // also unscaled
        gameObject.SetActive(false);
    }
}
