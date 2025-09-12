using UnityEngine;
using System.Collections;
using DG.Tweening;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 90;
    public int currentHealth;
    public int minusHealth;
    public float invisibleDuration;
    public bool isInvisible;
    [SerializeField] private bool _isInWater = false;
    private Coroutine damageCoroutine;
    void Start()
    {
        currentHealth = maxHealth;
        EventManager.instance.OnPlayerHit += OnPlayerHit;
    }
    private void OnDisable()
    {
        EventManager.instance.OnPlayerHit -= OnPlayerHit;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
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
                        EventManager.instance.WhenPlayerDied();
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

    private void OnPlayerHit(int damage)
    {
        if (isInvisible) return;

        int target = Mathf.Max(currentHealth - damage, 0);
        DOTween.To(() => currentHealth, x => currentHealth = x, target, 0.5f)
        // .OnUpdate(() => ))
        .OnComplete(() =>
        {
            if (currentHealth <= 0)
            {
                EventManager.instance.WhenPlayerDied();
                return;
            }
        });
        StartCoroutine(SetInvisible());
    }
    private IEnumerator SetInvisible()
    {
        isInvisible = true;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        collider.enabled = false;

        while (elapsed < invisibleDuration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        sr.enabled = true;
        collider.enabled = true;
        isInvisible = false;

        // ✅ Reset collision di semua EnemyAttack
        EnemyAttack[] enemies = FindObjectsOfType<EnemyAttack>();
        foreach (var enemy in enemies)
        {
            enemy.ResetCollision();
        }
    }

}
