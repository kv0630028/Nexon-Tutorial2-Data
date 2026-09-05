using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public int maxHp = 100;
    public float moveSpeed = 5f;
}
