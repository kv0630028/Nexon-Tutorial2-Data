using System;

// 순수 저장 데이터. ScriptableObject 아님. JsonUtility로 직렬화 가능한 Runtime State만 담는다.
[Serializable]
public class SaveData
{
    public int playerHp;
    public int score;
    public int wave;
}
