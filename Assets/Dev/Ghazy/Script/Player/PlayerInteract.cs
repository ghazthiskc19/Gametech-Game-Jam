using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerMovement _playerMovenent;
    public float holdTime = 2f;
    public float holdTimeDuration = 0;
    public TMP_Text interactText;
    void Start()
    {
        _playerMovenent = GetComponentInParent<PlayerMovement>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("interacable"))
        {
            InteracableObject obj = other.gameObject.GetComponent<InteracableObject>();
            if (!obj.hasInteracted)
            {
                interactText.enabled = true;
                if (_playerMovenent.isHoldInteract)
                {
                    holdTimeDuration += Time.deltaTime;
                    if (holdTimeDuration >= holdTime)
                    {
                        EventManager.instance.WhenObjectInteract();
                        holdTimeDuration = 0;
                        obj.gameObject.transform.SetParent(transform);
                    }
                }
                else
                {
                    holdTimeDuration = 0;
                }
            }
        }

        if (other.gameObject.CompareTag("shore"))
        {
            Debug.Log("Masuk shore");
            if (transform.childCount > 0)
            {
                Transform sampah = transform.GetChild(0);
                InteracableObject obj = sampah.GetComponent<InteracableObject>();
                Debug.Log("Masuk shore + punya anak");
                if (obj.hasInteracted && _playerMovenent.isHoldInteract)
                {
                Debug.Log("Masuk shore + punya anak + turun ccuk");
                    sampah.SetParent(other.gameObject.transform);
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
        }
    }
}
