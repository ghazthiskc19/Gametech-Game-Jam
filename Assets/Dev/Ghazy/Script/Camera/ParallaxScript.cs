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
        // Cari dan simpan transform dari kamera utama
        cameraTransform = Camera.main.transform;

        // Catat posisi awal kamera saat game dimulai
        cameraStartPosition = cameraTransform.position;
        
        // Catat posisi awal dari background (objek tempat script ini menempel)
        backgroundStartPosition = transform.position;
    }

    void LateUpdate()
    {
        // Hitung seberapa jauh kamera telah bergerak DARI TITIK AWALNYA
        Vector3 distanceTraveled = cameraTransform.position - cameraStartPosition;

        // Hitung pergerakan parallax untuk background ini
        float parallaxMoveX = distanceTraveled.x * parallaxEffectMultiplier.x;
        float parallaxMoveY = distanceTraveled.y * parallaxEffectMultiplier.y;

        transform.position = new Vector3(
            backgroundStartPosition.x + parallaxMoveX,
            backgroundStartPosition.y + parallaxMoveY,
            transform.position.z
        );
    }
}