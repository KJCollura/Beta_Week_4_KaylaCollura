using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerInteraction : MonoBehaviour
{
    public float moveSpeed = 5f;
    public TMP_Text scoreText; // Use TMP_Text instead of Text

    void Update()
    {
        MovePlayer();
        UpdateScoreUI();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveX, 0, moveZ));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube")) // Cube deducts points
        {
            KC_ScoreManager.instance?.AddScore(-10);
            Debug.Log("Cube collected: -10 points.");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Sphere")) // Sphere adds points
        {
            KC_ScoreManager.instance?.AddScore(5);
            Debug.Log("Sphere collected: +5 points.");
            Destroy(other.gameObject);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null && KC_ScoreManager.instance != null)
        {
            int currentScore = KC_ScoreManager.instance.GetScore();
            scoreText.text = $"Score: {currentScore}";
        }
    }
}