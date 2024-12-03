using UnityEngine;

public class ThirdPersonTankController : MonoBehaviour
{
    [Header("Движение танка")]
    public float moveSpeed = 5f;    // Скорость движения корпуса
    public float turnSpeed = 50f;  // Скорость поворота корпуса

    [Header("Башня и пушка")]
    public Transform turret;       // Башня танка
    public Transform gun;          // Пушка танка
    public float turretTurnSpeed = 5f;  // Скорость поворота башни
    public float gunTiltSpeed = 5f;     // Скорость наклона пушки
    public float minGunAngle = -10f;    // Минимальный угол наклона пушки
    public float maxGunAngle = 20f;     // Максимальный угол наклона пушки

    [Header("Камера")]
    public Transform cameraPivot;  // Точка вращения камеры
    public float cameraSensitivity = 3f; // Чувствительность камеры

    private float cameraRotationX = 0f; // Текущий угол вращения камеры по X
    private bool isCursorLocked = true; // Состояние блокировки курсора

    void Start()
    {
        SetCursorLockState(true); // Заблокировать курсор при старте
    }

    void Update()
    {
        HandleTankMovement();         // Управление движением танка
        HandleCursorLock();          // Управление блокировкой курсора
        if (isCursorLocked)
        {
            HandleCameraRotation();       // Управление камерой
            HandleTurretAndGunRotation(); // Управление башней и пушкой
        }
    }

    void HandleCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isCursorLocked = !isCursorLocked;
            SetCursorLockState(isCursorLocked);
        }
    }

    void SetCursorLockState(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }


void HandleTankMovement()
{
    float moveInput = Input.GetAxis("Vertical"); // Вперед/назад
    float turnInput = Input.GetAxis("Horizontal"); // Поворот корпуса

    // Движение корпуса
    if (moveInput != 0f)
    {
        // Двигаем танк в его локальном направлении (оси Z танка)
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime); 
    }

    // Поворот корпуса
    if (turnInput != 0f)
    {
        // Поворачиваем танк вокруг своей оси Y
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * turnAmount);  // Поворачиваем сам танк
    }
}


    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity; // Горизонтальный ввод мыши
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity; // Вертикальный ввод мыши

        // Поворот вокруг оси Y (вокруг танка)
        cameraPivot.Rotate(Vector3.up * mouseX);

        // Ограничение вертикального вращения камеры
        cameraRotationX = Mathf.Clamp(cameraRotationX - mouseY, -30f, 60f);
        cameraPivot.localRotation = Quaternion.Euler(cameraRotationX, cameraPivot.localEulerAngles.y, 0f);
    }

    void HandleTurretAndGunRotation()
    {
        if (!turret || !gun) return;

        // Поворот башни (по оси Y, локальная ось)
        Vector3 turretDirection = new Vector3(cameraPivot.forward.x, 90, cameraPivot.forward.z).normalized;
        if (turretDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetTurretRotation = Quaternion.LookRotation(turretDirection, Vector3.up);
            turret.rotation = Quaternion.Slerp(turret.rotation, targetTurretRotation, Time.deltaTime * turretTurnSpeed);
        }

        // Наклон пушки (по оси X, локально)
        float gunAngle = Mathf.Clamp(cameraRotationX, minGunAngle, maxGunAngle);
        gun.localRotation = Quaternion.Euler(gunAngle + 90f, 0, 0);
    }
}
