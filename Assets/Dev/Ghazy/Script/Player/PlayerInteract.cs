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
    void Start()
    {
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
            if (!isHolding) return;
            interactText.enabled = true;
            interactText.text = "Click Left Moues to drop trash";
            if (transform.childCount > 0)
            {
                Transform sampah = transform.GetChild(0);
                InteracableObject obj = sampah.GetComponent<InteracableObject>();
                if (obj.hasInteracted && _playerMovenent.isHoldInteract)
                {
                    isHolding = false;
                    interactText.enabled = false;
                    StartCoroutine(_playerMovenent.DoDolphinJump(sampah, other.transform));
                }
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
    }
}
