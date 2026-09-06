using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Game Data/Wave Data")]
public class WaveData : ScriptableObject
{
    // 한 Wave 안에서 "어떤 EnemyData를 몇 마리" 스폰할지 정의하는 데이터.
    [Serializable]
    public class EnemySpawn
    {
        public EnemyData data;
        public int count = 1;
    }

    // 여러 종류의 Enemy를 한 Wave에서 사용할 수 있다.
    public EnemySpawn[] spawns;
}
