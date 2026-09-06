using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private int currentHp;
    private bool isDead;
    private ScoreManager scoreManager;

    public event Action OnDeath;

    private void Awake()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();

        if (enemyData != null)
        {
            currentHp = enemyData.maxHp;
        }
    }

    // WaveManager가 공통 프리팹으로 스폰할 때 데이터를 주입한다.
    public void SetData(EnemyData data)
    {
        enemyData = data;
        currentHp = enemyData.maxHp;
    }

    public void TakeDamage(int damage)
    {
        // 이미 죽은 뒤 같은 프레임에 들어오는 추가 피격(샷건 등)을 무시한다.
        if (isDead)
        {
            return;
        }

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
        // Die는 Enemy 1마리당 정확히 한 번만 실행된다 (OnDeath / Score 중복 방지).
        if (isDead)
        {
            return;
        }
        isDead = true;

        if (scoreManager != null)
        {
            scoreManager.AddScore(enemyData.score);
        }

        // ===== [임시 테스트 로그] 테스트 완료 후 이 한 줄 삭제 =====
        Debug.Log($"Enemy Dead / Score: {enemyData.score}");

        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
