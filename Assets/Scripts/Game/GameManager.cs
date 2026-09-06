using UnityEngine;

// 최소 HUD(HP / Score / Wave / Weapon) + 게임 종료 상태(GAME OVER / GAME CLEAR) 표시.
// 진행 정지는 Time.timeScale = 0 으로 처리하고, R 재시작은 기존 RestartManager가 담당한다.
// 6(Load)로 Time.timeScale이 1로 복귀하면 자동으로 Playing 상태로 돌아온다.
public class GameManager : MonoBehaviour
{
    private enum State { Playing, GameOver, GameClear }

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private PlayerAttack playerAttack;

    private State state = State.Playing;
    private GUIStyle hudStyle;
    private GUIStyle centerStyle;
    private GUIStyle subStyle;

    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath += HandlePlayerDeath;
        }

        if (waveManager != null)
        {
            waveManager.OnAllWavesCleared += HandleAllWavesCleared;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandlePlayerDeath;
        }

        if (waveManager != null)
        {
            waveManager.OnAllWavesCleared -= HandleAllWavesCleared;
        }
    }

    private void Update()
    {
        // 6(Load)이 Time.timeScale을 1로 되돌리면 종료 상태에서 플레이로 복귀.
        if (state != State.Playing && !Mathf.Approximately(Time.timeScale, 0f))
        {
            state = State.Playing;
        }
    }

    private void HandlePlayerDeath()
    {
        state = State.GameOver;
        Time.timeScale = 0f;
    }

    private void HandleAllWavesCleared()
    {
        if (state == State.GameOver)
        {
            return;
        }

        state = State.GameClear;
        Time.timeScale = 0f;
    }

    private void OnGUI()
    {
        EnsureStyles();

        // 좌상단 HUD: 글자 56px. 잘림 방지를 위해 줄 높이 68, 줄 간격 72.
        GUI.Label(new Rect(16f, 16f, 800f, 68f), $"HP: {GetHp()} / {GetMaxHp()}", hudStyle);
        GUI.Label(new Rect(16f, 88f, 800f, 68f), $"Score: {GetScore()}", hudStyle);
        GUI.Label(new Rect(16f, 160f, 800f, 68f), $"Wave: {GetWave()}", hudStyle);
        GUI.Label(new Rect(16f, 232f, 800f, 68f), $"Weapon: {GetWeaponName()}", hudStyle);

        if (state == State.Playing)
        {
            return;
        }

        // 화면 중앙: 글자 112px가 잘리지 않도록 높이 180, 화면 중앙에 세로 정렬.
        string title = state == State.GameOver ? "GAME OVER" : "GAME CLEAR";
        GUI.Label(new Rect(0f, Screen.height * 0.5f - 90f, Screen.width, 180f), title, centerStyle);
        // 안내문: 글자 44px. 높이 64, 타이틀 영역(중앙 +90) 아래로 배치해 겹침 방지.
        GUI.Label(new Rect(0f, Screen.height * 0.5f + 95f, Screen.width, 64f), "Press R to restart", subStyle);
    }

    // 글자 크기와 정렬만 관리하는 스타일. 최초 OnGUI에서 1회 생성.
    private void EnsureStyles()
    {
        if (hudStyle != null)
        {
            return;
        }

        hudStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 56,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperLeft
        };

        centerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 112,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        subStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 44,
            alignment = TextAnchor.MiddleCenter
        };
    }

    private int GetHp() => playerHealth != null ? playerHealth.CurrentHp : 0;
    private int GetMaxHp() => playerHealth != null ? playerHealth.MaxHp : 0;
    private int GetScore() => scoreManager != null ? scoreManager.CurrentScore : 0;
    private int GetWave() => waveManager != null ? waveManager.CurrentWave : 0;
    private string GetWeaponName() => playerAttack != null ? playerAttack.CurrentWeaponName : "-";
}
