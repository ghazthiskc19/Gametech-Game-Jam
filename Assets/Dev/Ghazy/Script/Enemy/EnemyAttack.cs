using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    [SerializeField] private bool collisionOccurred = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (collisionOccurred) return;

        if (other.gameObject.CompareTag("Player"))
        {
            EventManager.instance.WhenPlayerHit(damage);
            collisionOccurred = true;
        }
    }
    public void ResetCollision()
    {
        collisionOccurred = false;
    }
}
