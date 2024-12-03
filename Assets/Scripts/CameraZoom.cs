using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Настройки зума")]
    public Camera playerCamera;    // Камера игрока
    public float zoomSpeed = 10f;  // Скорость зума
    public float minZoom = 20f;    // Минимальное поле зрения (больше зума)
    public float maxZoom = 60f;    // Максимальное поле зрения (меньше зума)

    void Update()
    {
        // Получение ввода от колеса мыши
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Изменение поля обзора камеры
        if (scrollInput != 0f)
        {
            playerCamera.fieldOfView = Mathf.Clamp(
                playerCamera.fieldOfView - scrollInput * zoomSpeed,
                minZoom,
                maxZoom
            );
        }
    }
}
