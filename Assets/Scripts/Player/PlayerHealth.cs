using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private int currentHp;
    private bool isDead;

    public int CurrentHp => currentHp;
    public int MaxHp => playerData.maxHp;

    public event Action OnDeath;

    private void Awake()
    {
        currentHp = playerData.maxHp;
    }

    // Load 시 저장된 상태로 복구한다: 사망 상태 해제 → currentHp 복구 → GameObject 재활성화.
    public void RestoreState(int hp)
    {
        isDead = false;
        currentHp = hp;
        gameObject.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        // 이미 사망 상태면 추가 피격을 무시한다 (Die 재실행 / HP 무한 감소 방지).
        if (isDead)
        {
            return;
        }

        currentHp -= damage;

        if (currentHp < 0)
        {
            currentHp = 0;
        }

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
        // Die는 1회만 실행된다.
        if (isDead)
        {
            return;
        }
        isDead = true;

        OnDeath?.Invoke();

        gameObject.SetActive(false);
    }
}
