using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DetectiveGameManager : MonoBehaviour
{
    [System.Serializable]
    public class Clue
    {
        public string clueName;
        public string question;
        public bool correctAnswer;
        public GameObject doorToUnlock;
    }

    public Clue[] clues;

    [Header("UI")]
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public Button yesButton;
    public Button noButton;
    public TextMeshProUGUI feedbackText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip unlockSound;
    public AudioClip wrongAnswerSound;

    [Header("Raycast Settings")]
    public float rayDistance = 3f;
    public LayerMask clueLayer;

    [Header("Gameplay")]
    public float penaltyTime = 2f;

    private Clue currentClue;
    private bool isInteracting = false;
    private GameObject currentLookTarget;

    void Start()
    {
        questionPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        yesButton.onClick.AddListener(() => AnswerQuestion(true));
        noButton.onClick.AddListener(() => AnswerQuestion(false));

        // Ensure cursor is locked at game start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isInteracting || questionPanel.activeSelf) return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Allow interaction even if layer is not explicitly used
        if (clueLayer.value != 0)
        {
            Physics.Raycast(ray, out hit, rayDistance, clueLayer);
        }
        else
        {
            Physics.Raycast(ray, out hit, rayDistance);
        }

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Clue"))
        {
            currentLookTarget = hit.collider.gameObject;

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteractWithClue(currentLookTarget);
            }
        }
        else
        {
            currentLookTarget = null;
        }
    }

    public void TryInteractWithClue(GameObject clueObject)
    {
        foreach (Clue clue in clues)
        {
            if (clueObject.name == clue.clueName)
            {
                currentClue = clue;
                ShowQuestion(clue);
                break;
            }
        }
    }

    void ShowQuestion(Clue clue)
    {
        isInteracting = true;
        questionPanel.SetActive(true);
        questionText.text = clue.question;

        // 🔓 Unlock cursor so player can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void AnswerQuestion(bool answer)
{
    questionPanel.SetActive(false);

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    if (answer == currentClue.correctAnswer)
    {
        var doorScript = currentClue.doorToUnlock.GetComponent<DoorScript.Door>();
if (doorScript != null)
{
    doorScript.OpenDoor();  // ✅ This triggers your door script
}


        isInteracting = false;
    }
    else
    {
        StartCoroutine(WrongAnswerPenalty());
    }
}


    IEnumerator WrongAnswerPenalty()
    {
        if (audioSource && wrongAnswerSound)
            audioSource.PlayOneShot(wrongAnswerSound);

        feedbackText.text = "Wrong answer! Try again...";
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(penaltyTime);
        feedbackText.gameObject.SetActive(false);

        // 🔒 Lock cursor again after feedback
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isInteracting = false;
    }
}
