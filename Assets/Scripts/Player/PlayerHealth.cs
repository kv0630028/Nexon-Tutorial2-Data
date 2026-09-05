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
