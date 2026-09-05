using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHp;
    public float moveSpeed;
    public int attackDamage;
    public float attackRange = 1f;
    public float attackCooldown;
    public int score;
}
