using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D bulletRb;
    private WeaponData weaponData;

    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(WeaponData data, Vector2 direction)
    {
        weaponData = data;

        if (bulletRb == null)
        {
            bulletRb = GetComponent<Rigidbody2D>();
        }

        bulletRb.linearVelocity = direction.normalized * weaponData.bulletSpeed;
        Destroy(gameObject, weaponData.bulletLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(weaponData.damage);
            Destroy(gameObject);
        }
    }
}
