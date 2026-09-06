using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore;

    public int CurrentScore => currentScore;

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    // Load 시 저장된 점수를 복구한다.
    public void SetScore(int score)
    {
        currentScore = score;
    }
}
