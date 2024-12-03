using UnityEngine;

public class UI_AimCursor : MonoBehaviour
{
    [Header("Привязки")]
    public Camera playerCamera;         // Камера игрока
    public Transform gun;               // Пушка танка
    public RectTransform crosshairUI;   // UI перекрестие

    [Header("Настройки")]
    public float crosshairDistance = 50f; // Расстояние, на котором будет находиться перекрестие от пушки

    void Update()
    {
        // Вычисляем направление от пушки
        Vector3 gunDirection = gun.forward;

        // Определяем точку, куда пушка направлена, на фиксированном расстоянии от пушки
        Vector3 targetPosition = gun.position + gunDirection * crosshairDistance;

        // Преобразуем эту точку в экранные координаты
        Vector3 screenPoint = playerCamera.WorldToScreenPoint(targetPosition);

        // Перемещаем UI перекрестие в эту точку
        crosshairUI.position = screenPoint;
    }
}