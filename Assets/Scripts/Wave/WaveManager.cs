using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 0.4f;
    [SerializeField] private float delayBetweenWaves = 2f;
    [SerializeField] private WaveData[] waves;

    private int currentWaveIndex = 0;
    private int aliveEnemies = 0;
    private bool spawning = false;
    private bool advancing = false;

    // 마지막 Wave까지 모두 클리어되면 1회 발생.
    public event System.Action OnAllWavesCleared;

    private void Start()
    {
        StartNextWave();
    }

    // 현재 진행 중인 Wave 번호(1-based). Save에서 읽는다.
    public int CurrentWave => currentWaveIndex;

    // Load 시 저장된 Wave부터 안전하게 다시 시작한다.
    // 진행 중이던 코루틴과 남아있는 Enemy를 정리한 뒤 해당 Wave를 새로 스폰한다.
    public void LoadWave(int waveNumber)
    {
        StopAllCoroutines();

        foreach (EnemyHealth enemy in FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None))
        {
            Destroy(enemy.gameObject);
        }

        aliveEnemies = 0;
        spawning = false;
        advancing = false;
        currentWaveIndex = Mathf.Clamp(waveNumber - 1, 0, Mathf.Max(0, waves.Length - 1));

        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("[WaveManager] All waves cleared");
            OnAllWavesCleared?.Invoke();
            return;
        }

        Debug.Log($"[WaveManager] Wave {currentWaveIndex + 1} start");
        StartCoroutine(SpawnWaveRoutine(waves[currentWaveIndex]));
        currentWaveIndex++;
    }

    private IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        spawning = true;

        foreach (WaveData.EnemySpawn spawn in wave.spawns)
        {
            for (int i = 0; i < spawn.count; i++)
            {
                SpawnEnemy(spawn.data);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        spawning = false;

        // 스폰이 끝났는데 이미 전부 죽어 있으면 바로 다음 Wave 예약.
        if (aliveEnemies <= 0)
        {
            ScheduleNextWave();
        }
    }

    // EnemyData만 넘겨서 공통 프리팹을 생성한다. 종류별 분기 없음.
    private void SpawnEnemy(EnemyData data)
    {
        GameObject enemy = Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity);

        enemy.GetComponent<EnemyMovement>().SetData(data);
        enemy.GetComponent<EnemyAttack>().SetData(data);
        enemy.GetComponent<EnemyVisual>().Apply(data);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        health.SetData(data);
        health.OnDeath += HandleEnemyDeath;

        aliveEnemies++;
    }

    // Enemy 사망 이벤트로만 카운트를 줄인다. 매 프레임 Enemy 검색하지 않는다.
    private void HandleEnemyDeath()
    {
        aliveEnemies--;

        if (!spawning && aliveEnemies <= 0)
        {
            ScheduleNextWave();
        }
    }

    private void ScheduleNextWave()
    {
        if (advancing)
        {
            return;
        }

        advancing = true;
        StartCoroutine(NextWaveAfterDelay());
    }

    private IEnumerator NextWaveAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenWaves);
        advancing = false;
        StartNextWave();
    }

    private Vector3 GetSpawnPosition()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;
        }

        return transform.position;
    }
}
