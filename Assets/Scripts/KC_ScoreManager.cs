using UnityEngine;

public class KC_ScoreManager : MonoBehaviour
{
    public static KC_ScoreManager instance;
    private int score = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Debug.Log("ScoreManager initialized.");
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Score updated: {score}");
    }

    public int GetScore()
    {
        Debug.Log($"Current Score: {score}");
        return score;
    }
}
