using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    private PlayerMovement _playerMovenent;
    public float holdTime = 2f;
    public float holdTimeDuration = 0;
    public bool isHolding = false;
    public TMP_Text interactText;
    public Image holdImage;
    public GameObject timerCircle;
    public bool IsInShoreZone { get { return _isShoreZone; } }
    private bool _isShoreZone;
    private bool _isInJumpZone = false;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _playerMovenent = GetComponentInParent<PlayerMovement>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("interacable"))
        {
            if (isHolding) return;
            InteracableObject obj = other.gameObject.GetComponent<InteracableObject>();
            if (!obj.hasInteracted)
            {
                interactText.enabled = true;
                interactText.text = "HOLD MOUSE LEFT\nTO INTERACT";
                if (_playerMovenent.isHoldInteract)
                {
                    holdTimeDuration += Time.deltaTime;
                    timerCircle.SetActive(true);
                    interactText.enabled = false;
                    holdImage.fillAmount = holdTimeDuration / holdTime;

                    if (holdTimeDuration >= holdTime)
                    {
                        EventManager.instance.WhenObjectInteract();
                        holdTimeDuration = 0;
                        isHolding = true;
                        _animator.SetBool("IsHolding", true);
                        obj.gameObject.transform.SetParent(transform);
                        timerCircle.SetActive(false);
                        holdImage.fillAmount = 0;
                        obj.hasInteracted = true;
                    }
                }
                else
                {
                    holdImage.fillAmount = 0;
                    holdTimeDuration = 0;
                    timerCircle.SetActive(false);
                }
            }
        }
        if (other.gameObject.CompareTag("shore"))
        {
            _isShoreZone = true;
            interactText.enabled = true;
            if (transform.childCount > 0)
            {
                if (!isHolding) return;
                interactText.text = "Click Left  to jump and drop trash";
                Transform sampah = transform.GetChild(0);
                InteracableObject obj = sampah.GetComponent<InteracableObject>();
                if (obj.hasInteracted && _playerMovenent.isHoldInteract)
                {
                    isHolding = false;
                    _animator.SetBool("IsHolding", false);
                    AudioManager.instance.PlaySFX("Trash_Obtained");
                    interactText.enabled = false;
                    StartCoroutine(_playerMovenent.DoDolphinJump(sampah));
                }
            }
            else
            {
                interactText.text = "Click Left to jump";
                if (_playerMovenent.isHoldInteract)
                {
                    interactText.enabled = false;
                    StartCoroutine(_playerMovenent.DoDolphinJump(null));
                }
            }
        }

        if (other.CompareTag("JumpZone"))
        {
            _isInJumpZone = true;
            interactText.enabled = true;
            interactText.text = "Click Left Mouse to jump";
            if (_playerMovenent.isHoldInteract)
            {
                interactText.enabled = false;
                StartCoroutine(_playerMovenent.DoDolphinJump(null));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("interacable"))
        {
            interactText.enabled = false;
            _playerMovenent.isHoldInteract = false;
            holdTimeDuration = 0;
            timerCircle.SetActive(false);
            holdImage.fillAmount = 0;
        }
        if (other.gameObject.CompareTag("shore"))
        {
            interactText.enabled = false;
            _playerMovenent.isHoldInteract = false;
        }

        if (other.CompareTag("JumpZone"))
        {
            _isInJumpZone = false;
            interactText.enabled = false;
        }
    }
}
