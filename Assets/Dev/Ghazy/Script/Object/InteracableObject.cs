using UnityEngine;

public class InteracableObject : MonoBehaviour
{
    public Transform parent;
    public bool hasInteracted = false;
    void Start()
    {
        parent = transform.parent;
    }
    void OnEnable()
    {
        EventManager.instance.OnObjectInteract += ObjectInteracted;
    }
    void OnDisable()
    {
        EventManager.instance.OnObjectInteract -= ObjectInteracted;
    }

    private void ObjectInteracted()
    {
        hasInteracted = true;
        Debug.Log("Object has been interacted");
    }


}
