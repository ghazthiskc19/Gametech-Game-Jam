using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 90;
    public int currentHealth;
    public int minusHealth;
    public float invisibleDuration;
    public int plusHealth;
    public float intervalHealth;
    public Image healthbarImage;
    public bool isInvisible;
    public bool isHitByEnemy;
    public bool effectsPaused = false;
    public bool IsInSaveZone { get { return _isInSave; } }
    private PlayerMovement playerMovement;
    [SerializeField] private bool _isInWater = false;
    [SerializeField] private bool _isInSave = false;
    private Coroutine damageCoroutine;
    private Coroutine healCoroutine;
    void Start()
    {
        currentHealth = maxHealth;
        EventManager.instance.OnPlayerHit += OnPlayerHit;
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnDisable()
    {
        EventManager.instance.OnPlayerHit -= OnPlayerHit;
    }

    private Coroutine dolphinHealCoroutine;
    void Update()
    {
        if (healthbarImage != null)
        {
            healthbarImage.fillAmount = (float) currentHealth / maxHealth;
        }

        if (playerMovement != null)
        {
            if (playerMovement._isDolphinJumping)
            {
                if (dolphinHealCoroutine == null)
                {
                    dolphinHealCoroutine = StartCoroutine(HealDuringDolphinJump());
                }
            }
            else
            {
                if (dolphinHealCoroutine != null)
                {
                    StopCoroutine(dolphinHealCoroutine);
                    dolphinHealCoroutine = null;
                }
            }
        }
    }
    private IEnumerator HealDuringDolphinJump()
    {
        while (playerMovement._isDolphinJumping)
        {
            if (currentHealth < maxHealth)
            {
                int targetHealth = Mathf.Min(currentHealth + plusHealth, maxHealth);
                DOTween.To(() => currentHealth, x => currentHealth = x, targetHealth, 0.5f);
            }
            yield return new WaitForSeconds(intervalHealth);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            if (_isInSave)
            {
                _isInSave = false;
                if (healCoroutine != null)
                {
                    StopCoroutine(healCoroutine);
                    healCoroutine = null;
                }
            }

            _isInWater = true;
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(StartDamage());
            }
        }

        if (other.gameObject.CompareTag("save"))
        {
            if (_isInWater)
            {
                _isInWater = false;
                if (damageCoroutine != null)
                {
                    StopCoroutine(damageCoroutine);
                    damageCoroutine = null;
                }
            }

            _isInSave = true;
            if (healCoroutine == null)
            {
                healCoroutine = StartCoroutine(StartHeal());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            _isInWater = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }

        if (other.gameObject.CompareTag("save"))
        {
            _isInSave = false;
            _isInWater = true;
            if (healCoroutine != null)
            {
                StopCoroutine(healCoroutine);
                healCoroutine = null;
                damageCoroutine = StartCoroutine(StartDamage());
            }
        }
    }

    private IEnumerator StartDamage()
    {
        while (_isInWater)
        {
            if (currentHealth > 0 && !isHitByEnemy)
            {
                if (!playerMovement._isDolphinJumping)
                {
                    int targetHealth = Mathf.Max(0, currentHealth - minusHealth);
                    DOTween.To(() => currentHealth, x => currentHealth = x, targetHealth, 0.5f)
                        .OnComplete(() =>
                        {
                            if (currentHealth <= 0)
                            {
                                EventManager.instance.WhenPlayerDied();
                            }
                        });
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator StartHeal()
    {
        while (_isInSave)
        {
            if (currentHealth < maxHealth)
            {
                int targetHealth = Mathf.Min(currentHealth + plusHealth, maxHealth);
                DOTween.To(() => currentHealth, x => currentHealth = x, targetHealth, 0.5f);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnPlayerHit(int damage)
    {
        if (isInvisible) return;

        isHitByEnemy = true;
        int target = Mathf.Max(0, currentHealth - damage);
        DOTween.To(() => currentHealth, x => currentHealth = x, target, 0.4f)
            .OnComplete(() =>
            {
                isHitByEnemy = false;
                if (currentHealth <= 0)
                {
                    EventManager.instance.WhenPlayerDied();
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

        EnemyAttack[] enemies = FindObjectsOfType<EnemyAttack>();
        foreach (var enemy in enemies)
        {
            enemy.ResetCollision();
        }
    }
}