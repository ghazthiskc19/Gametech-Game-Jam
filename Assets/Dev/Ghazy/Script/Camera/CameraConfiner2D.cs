using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraConfiner2D : MonoBehaviour
{
    public Collider2D confinerCollider;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (confinerCollider == null) return;

        Vector3 pos = transform.position;

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        Bounds bounds = confinerCollider.bounds;

        float minX = bounds.min.x + horzExtent;
        float maxX = bounds.max.x - horzExtent;
        float minY = bounds.min.y + vertExtent;
        float maxY = bounds.max.y - vertExtent;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
