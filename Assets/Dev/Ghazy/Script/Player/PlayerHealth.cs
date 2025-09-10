using UnityEngine;
using System.Collections;
using DG.Tweening;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 90;
    public int currentHealth;
    public int minusHealth;
    [SerializeField] private bool _isInWater = false;
    private Coroutine damageCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("water"))
        {
            _isInWater = true;
            damageCoroutine = StartCoroutine(StartDamage());
        }    
    }
    private IEnumerator StartDamage()
    {
        while (true)
        {
            if (_isInWater && currentHealth > 0)
            {
                int targetHealth = Mathf.Max(currentHealth - minusHealth);
                DOTween.To(() => currentHealth, x => currentHealth = x, targetHealth, 0.5f)
                // .OnUpdate(() => ))
                .OnComplete(() =>
                {
                    if (currentHealth <= 0)
                    {
                        Debug.Log("Player Mati!");
                    }
                });
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            _isInWater = false;
            StopCoroutine(damageCoroutine);
        }
    }
}
