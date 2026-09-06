using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// R = 현재 Scene을 새 게임 상태로 다시 시작. (저장 파일은 건드리지 않는다.)
public class RestartManager : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Restart();
        }
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
