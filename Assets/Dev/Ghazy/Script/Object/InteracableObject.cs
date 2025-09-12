using UnityEngine;

public class InteracableObject : MonoBehaviour
{
    public Transform parent;
    public float amountScore;
    public bool hasInteracted = false;
    void Start()
    {
        parent = transform.parent;
        EventManager.instance.OnObjectChangeState += ObjectInteracted;
    }
    void OnDisable()
    {
        EventManager.instance.OnObjectChangeState -= ObjectInteracted;
    }

    private void ObjectInteracted(GameObject gameObject)
    {
        if (this != gameObject) return;
        hasInteracted = true;
        Debug.Log("Object has been interacted");
    }


}
