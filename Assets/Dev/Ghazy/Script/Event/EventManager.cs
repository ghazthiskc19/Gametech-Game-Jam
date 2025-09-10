using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action OnPlayerLowHealth;
    public event Action OnObjectInteract;
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
}
