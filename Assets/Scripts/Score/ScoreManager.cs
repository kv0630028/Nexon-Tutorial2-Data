using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore;

    public int CurrentScore => currentScore;

    public void AddScore(int amount)
    {
        currentScore += amount;
    }
}
