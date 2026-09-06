using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private float cooldownTimer;
    private bool playerInContact;
    private PlayerHealth playerHealth;

    // WaveManager가 공통 프리팹으로 스폰할 때 데이터를 주입한다.
    public void SetData(EnemyData data)
    {
        enemyData = data;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (enemyData == null || !playerInContact || playerHealth == null)
        {
            return;
        }

        if (cooldownTimer <= 0f)
        {
            playerHealth.TakeDamage(enemyData.attackDamage);
            cooldownTimer = enemyData.attackCooldown;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerHealth = other.GetComponent<PlayerHealth>();
        playerInContact = playerHealth != null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInContact = false;
        playerHealth = null;
    }
}
