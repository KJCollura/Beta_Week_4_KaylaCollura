using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerInteraction : MonoBehaviour
{
    public float moveSpeed = 5f;
    public TMP_Text scoreText; // Use TMP_Text instead of Text

    private int score = 0;

    void Update()
    {
        MovePlayer();
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
            score -= 10; 
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Sphere")) // Sphere adds points
        {
            score += 5; 
            Destroy(other.gameObject);
        }

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}