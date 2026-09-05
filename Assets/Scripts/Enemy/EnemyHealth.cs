using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private int currentHp;
    private ScoreManager scoreManager;

    private void Awake()
    {
        currentHp = enemyData.maxHp;
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        // ===== [HP 확인용 로그] 유지 =====
        Debug.Log($"Enemy HP: {currentHp} / {enemyData.maxHp}");
        // ================================

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(enemyData.score);
        }

        // ===== [임시 테스트 로그] 테스트 완료 후 이 한 줄 삭제 =====
        Debug.Log($"Enemy Dead / Score: {enemyData.score}");

        Destroy(gameObject);
    }
}
