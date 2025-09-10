using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform camera;
    void Start()
    {
        camera = Camera.main.transform;
    }

    void Update()
    {
        transform.position = new Vector3
        (
            camera.position.x,
            camera.position.y,
            transform.position.z  
        );
    }
}
