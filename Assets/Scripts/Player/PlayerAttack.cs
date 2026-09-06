using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private WeaponData pistolData;
    [SerializeField] private WeaponData shotgunData;
    [SerializeField] private WeaponData burstData;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Camera mainCamera;
    private float cooldownTimer;
    private float aimAngle;

    // UI 표시용: 현재 선택된 무기 이름.
    public string CurrentWeaponName => weaponData != null ? weaponData.weaponName : "-";

    private void Awake()
    {
        mainCamera = Camera.main;

        // 시작 무기는 Pistol.
        if (pistolData != null)
        {
            weaponData = pistolData;
        }
    }

    private void Update()
    {
        // 숫자키로 현재 무기(WeaponData 참조)만 교체한다. 발사 로직은 그대로.
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame && pistolData != null)
            {
                weaponData = pistolData;
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame && shotgunData != null)
            {
                weaponData = shotgunData;
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame && burstData != null)
            {
                weaponData = burstData;
            }
        }

        if (Mouse.current == null)
        {
            return;
        }

        AimFirePoint();

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (weaponData != null
            && Mouse.current.leftButton.wasPressedThisFrame
            && cooldownTimer <= 0f)
        {
            Fire();
            cooldownTimer = weaponData.fireCooldown;
        }
    }

    private void AimFirePoint()
    {
        Vector3 screenPosition = Mouse.current.position.ReadValue();
        screenPosition.z = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        Vector2 direction = (Vector2)mouseWorldPosition - (Vector2)firePoint.position;
        aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0f, 0f, aimAngle);
    }

    private void Fire()
    {
        int count = Mathf.Max(1, weaponData.projectilesPerShot);
        float step = count > 1 ? weaponData.spreadAngle / (count - 1) : 0f;
        float startAngle = aimAngle - step * (count - 1) * 0.5f;

        for (int i = 0; i < count; i++)
        {
            float shotAngle = startAngle + step * i;
            Vector2 shotDirection = new Vector2(
                Mathf.Cos(shotAngle * Mathf.Deg2Rad),
                Mathf.Sin(shotAngle * Mathf.Deg2Rad));

            GameObject bulletObject = Instantiate(
                bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, shotAngle));

            PlayerBullet bullet = bulletObject.GetComponent<PlayerBullet>();
            if (bullet != null)
            {
                bullet.Initialize(weaponData, shotDirection);
            }
        }
    }
}
