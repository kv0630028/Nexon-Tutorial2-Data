using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float stopDistance = 0.8f;

    private Rigidbody2D enemyRb;
    private Transform playerTransform;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (enemyData == null || playerTransform == null)
        {
            enemyRb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(playerTransform.position, enemyRb.position);

        if (distance <= stopDistance)
        {
            enemyRb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)playerTransform.position - enemyRb.position).normalized;
        enemyRb.linearVelocity = direction * enemyData.moveSpeed;
    }
}
