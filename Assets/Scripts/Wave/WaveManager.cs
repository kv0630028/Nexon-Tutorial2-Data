using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // 하나의 Wave 안에서 "어떤 EnemyData를 몇 마리" 스폰할지 정의하는 데이터.
    [Serializable]
    public class EnemySpawn
    {
        public EnemyData data;
        public int count = 1;
    }

    // 한 Wave = EnemySpawn 목록. 종류 분기 없이 데이터 조합만으로 구성된다.
    [Serializable]
    public class Wave
    {
        public EnemySpawn[] spawns;
    }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 0.4f;
    [SerializeField] private float delayBetweenWaves = 2f;
    [SerializeField] private Wave[] waves;

    private int currentWaveIndex = 0;
    private int aliveEnemies = 0;
    private bool spawning = false;
    private bool advancing = false;

    private void Start()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("[WaveManager] All waves cleared");
            return;
        }

        Debug.Log($"[WaveManager] Wave {currentWaveIndex + 1} start");
        StartCoroutine(SpawnWaveRoutine(waves[currentWaveIndex]));
        currentWaveIndex++;
    }

    private IEnumerator SpawnWaveRoutine(Wave wave)
    {
        spawning = true;

        foreach (EnemySpawn spawn in wave.spawns)
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
