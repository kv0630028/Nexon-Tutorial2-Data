using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHp;
    public float moveSpeed;
    public int attackDamage;

    // 접촉(Collider2D 트리거) 기반 공격으로 전환된 뒤로 EnemyAttack에서 사용하지 않음.
    // 데이터 스키마와 기존 에셋 값을 건드리지 않기 위해 필드는 그대로 남겨 둠(원거리 적을 추가하면 재사용 가능).
    public float attackRange = 1f;

    public float attackCooldown;
    public int score;

    [Header("Visual")]
    public float visualScale = 1f;
    public Color visualColor = Color.white;
}
