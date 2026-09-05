using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerHealth playerHealth;

    private float cooldownTimer;

    private void Awake()
    {
        if (playerTransform != null && playerHealth != null)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return;
        }

        if (playerTransform == null)
        {
            playerTransform = player.transform;
        }

        if (playerHealth == null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (enemyData == null || playerTransform == null || playerHealth == null)
        {
            return;
        }

        cooldownTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance <= enemyData.attackRange && cooldownTimer <= 0f)
        {
            playerHealth.TakeDamage(enemyData.attackDamage);

            // ===== [임시 테스트 로그] 테스트 완료 후: 이 호출 1줄 + 클래스 하단 LogPlayerHpForTest() 메서드를 함께 삭제 =====
            LogPlayerHpForTest();

            cooldownTimer = enemyData.attackCooldown;
        }
    }

    // ==================== 임시 테스트용 코드 (테스트 후 삭제) ====================
    // Player HP 감소를 Console에서 확인하기 위한 임시 로그.
    private void LogPlayerHpForTest()
    {
        Debug.Log($"Player HP: {playerHealth.CurrentHp} / {playerHealth.MaxHp}");
    }
    // =====================================================================
}
