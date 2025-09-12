using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action OnPlayerLowHealth;
    public event Action OnObjectInteract;
    public event Action<float> OnObjectDropped;
    public event Action<GameObject> OnObjectChangeState;
    public event Action OnObjectiveCompleted;
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
    public void WhenObjectDropped(float score, GameObject go)
    {
        Debug.Log("Score nambah gak sih?");
        OnObjectDropped?.Invoke(score);
        OnObjectiveCompleted?.Invoke();
        OnObjectChangeState?.Invoke(go);
    }
}
