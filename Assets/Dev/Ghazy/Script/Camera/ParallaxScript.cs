using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    [Tooltip("Kekuatan efek parallax (kecepatan). Semakin kecil angkanya, semakin lambat gerakannya (terlihat lebih jauh).")]
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    private Transform cameraTransform;
    private Vector3 cameraStartPosition;
    private Vector3 backgroundStartPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        cameraStartPosition = cameraTransform.position;
        
        backgroundStartPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 distanceTraveled = cameraTransform.position - cameraStartPosition;

        float parallaxMoveX = distanceTraveled.x * parallaxEffectMultiplier.x;
        float parallaxMoveY = distanceTraveled.y * parallaxEffectMultiplier.y;

        transform.position = new Vector3(
            backgroundStartPosition.x + parallaxMoveX,
            backgroundStartPosition.y + parallaxMoveY,
            transform.position.z
        );
    }
}