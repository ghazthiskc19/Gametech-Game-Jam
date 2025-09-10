using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action OnPlayerLowHealth;
    public event Action OnObjectInteract;
    public event Action<int> OnPlayerHit;
    public event Action OnPlayerDied;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void WhenPlayerLowHealth()
    {
        OnPlayerLowHealth?.Invoke();
    }
    public void WhenObjectInteract()
    {
        OnObjectInteract?.Invoke();
    }
    public void WhenPlayerHit(int damage)
    {
        OnPlayerHit?.Invoke(damage);
    }
    public void WhenPlayerDied()
    {
        OnPlayerDied?.Invoke();
    }
}
