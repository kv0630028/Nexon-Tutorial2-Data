using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private int currentHp;

    public int CurrentHp => currentHp;
    public int MaxHp => playerData.maxHp;

    private void Awake()
    {
        currentHp = playerData.maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        // ===== [임시 진단 로그] 원인 확인 후 삭제 =====
        Debug.Log($"Player HP: {currentHp} / {MaxHp}");
        // ===========================================

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
