using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankStats : MonoBehaviour
{
    [Header("Параметры танка")]
    public static TankStats instance;
    public int maxHP = 100;       // Максимальное здоровье
    public int currentHP;         // Текущее здоровье
    public int maxAmmo = 10;      // Максимальное количество боеприпасов
    public int currentAmmo;       // Текущее количество боеприпасов
    public float reloadTime = 2f; // Время перезарядки

    [Header("Состояние")]
    private bool isReloading = false; // Идёт ли перезарядка
    private bool isReloaded = true;   // Перезарядка завершена

    [Header("Привязка к UI")]
    public Slider healthSlider;      // Слайдер здоровья
    public TextMeshProUGUI ammoText; // Текст боекомплекта
    public Image reloadIndicator;    // Индикатор перезарядки (например, круг или иконка)

    [Header("Оружие")]
    public GameObject projectilePrefab; // Префаб снаряда
    public Transform firePoint;         // Точка вылета снаряда

    [Header("Настройки стрельбы")]
    public float projectileSpeed = 20f; // Скорость снаряда

    private void Awake()
    {
        instance = this;
        ammoText.text = "Ammo: " + currentAmmo.ToString();
        reloadIndicator.enabled = false;
    }

    void Start()
    {
        currentHP = maxHP;
        currentAmmo = maxAmmo;

        UpdateUI();
    }

    void Update()
    {
        // Проверка на выстрел
        if (Input.GetMouseButtonDown(0) && !isReloading && isReloaded == true && currentAmmo > 0)
        {
            isReloaded = false;
            Fire();
        }

        // Принудительная перезарядка по кнопке R
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    // Выстрел
    void Fire()
    {
        // Проверка наличия точки вылета и префаба снаряда
        if (firePoint == null || projectilePrefab == null)
        {
            Debug.LogWarning("FirePoint или ProjectilePrefab не установлены!");
            return;
        }

        // Проверка, есть ли патроны для выстрела
        if (currentAmmo <= 0)
        {
            Debug.LogWarning("Нет патронов!");
            return;
        }

        // Создание снаряда
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Добавление скорости снаряду
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * projectileSpeed; // Вылет по оси Z
        }

        currentAmmo--; // Уменьшаем количество боеприпасов

        UpdateUI();    // Обновляем UI
    }

    // Перезарядка
    IEnumerator Reload()
    {
        isReloading = true;

        // Показать индикатор перезарядки
        if (reloadIndicator != null)
            reloadIndicator.enabled = true;

        yield return new WaitForSeconds(reloadTime);

        // Завершаем перезарядку
        isReloading = false;
        isReloaded = true;

        // Скрыть индикатор перезарядки
        if (reloadIndicator != null)
            reloadIndicator.enabled = false;

        UpdateUI();  // Обновляем UI
    }

    // Получение урона
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
        UpdateUI();
    }

    // Умереть
    void Die()
    {
        Debug.Log("Танк уничтожен!");
        // Добавьте логику уничтожения
    }

    // Обновление UI
    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHP / maxHP;
        }

        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
        }
    }
}
