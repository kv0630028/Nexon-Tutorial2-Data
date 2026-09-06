using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

// 5 = 현재 Runtime State 저장, 6 = 저장된 Runtime State 복구.
// Scene Reload(R)는 RestartManager 담당이며 여기서 다루지 않는다.
public class SaveManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private WaveManager waveManager;

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            Save();
        }

        if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            Load();
        }
    }

    private void Save()
    {
        SaveData data = new SaveData
        {
            playerHp = playerHealth.CurrentHp,
            score = scoreManager.CurrentScore,
            wave = waveManager.CurrentWave
        };

        File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
        Debug.Log($"[SaveManager] Save -> HP:{data.playerHp} Score:{data.score} Wave:{data.wave} ({SavePath})");
    }

    private void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[SaveManager] Load -> 저장 파일 없음");
            return;
        }

        // GameOver / GameClear로 정지돼 있어도 Load하면 진행을 재개한다.
        Time.timeScale = 1f;

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));

        // 기존 Runtime State에 적용한다. Scene Reload는 하지 않는다.
        scoreManager.SetScore(data.score);
        playerHealth.RestoreState(data.playerHp);
        waveManager.LoadWave(data.wave);

        Debug.Log($"[SaveManager] Load -> HP:{data.playerHp} Score:{data.score} Wave:{data.wave}");
    }
}
