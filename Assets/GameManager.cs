using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int scoreA = 0;
    public int scoreB = 0;
    public int winningScore = 30;

    public TextMeshProUGUI characterAScoreText;
    public TextMeshProUGUI characterBScoreText;
    public TextMeshProUGUI winnerText;

    public GameObject characterAPrefab;
    public GameObject characterBPrefab;
    public GameObject respawnTextPrefab;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddScore(string character)
    {
        if (character == "CharacterA")
        {
            scoreB++;
            characterBScoreText.text = scoreB.ToString();
        }
        else if (character == "CharacterB")
        {
            scoreA++;
            characterAScoreText.text = scoreA.ToString();
        }

        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (scoreA >= winningScore)
        {
            winnerText.text = "Character A Wins!";
            Time.timeScale = 0;
        }
        else if (scoreB >= winningScore)
        {
            winnerText.text = "Character B Wins!";
            Time.timeScale = 0;
        }
    }

    public void RespawnCharacter(string character, Vector3 deathPosition)
    {
        StartCoroutine(RespawnCoroutine(character, deathPosition));
    }

    IEnumerator RespawnCoroutine(string character, Vector3 deathPosition)
    {
        GameObject respawnText = Instantiate(respawnTextPrefab, deathPosition + Vector3.up * 2, Quaternion.identity);
        TextMeshPro textMesh = respawnText.GetComponent<TextMeshPro>();

        for (int i = 3; i > 0; i--)
        {
            textMesh.text = "Respawning in " + i + "...";
            yield return new WaitForSeconds(1f);
        }

        Destroy(respawnText); 

        if (character == "CharacterA")
        {
            Instantiate(characterAPrefab, deathPosition, Quaternion.identity);
        }
        else if (character == "CharacterB")
        {
            Instantiate(characterBPrefab, deathPosition, Quaternion.identity);
        }
    }
}
