using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyData != null)
        {
            Apply(enemyData);
        }
    }

    public void Apply(EnemyData data)
    {
        enemyData = data;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        transform.localScale = Vector3.one * data.visualScale;
        spriteRenderer.color = data.visualColor;

        // ===== [임시 진단 로그] 원인 확인 후 삭제 =====
        Debug.Log($"[EnemyVisual] {data.enemyName}: scale={data.visualScale}, color={data.visualColor}");
        // ===========================================
    }
}
