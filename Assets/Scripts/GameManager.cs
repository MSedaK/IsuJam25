using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public GameObject respawnVFXPrefab;

    public Transform spawnPointA;
    public Transform spawnPointB;

    private bool comboActive = false;
    private int comboIndex = 0;
    private List<int> comboStages = new List<int> { 5, 15, 22, 29 }; 

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddScore(string character)
    {
        if (character == "CharacterA")
        {
            scoreA++;
            characterAScoreText.text = scoreA.ToString();
        }
        else if (character == "CharacterB")
        {
            scoreB++;
            characterBScoreText.text = scoreB.ToString();
        }

        if (comboIndex < comboStages.Count && (scoreA + scoreB) == comboStages[comboIndex] && !comboActive)
        {
            StartComboPhase(comboIndex);
            comboIndex++; 
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

    public void RespawnCharacter(PlayerHealth character)
    {
        StartCoroutine(RespawnCoroutine(character));
    }

    IEnumerator RespawnCoroutine(PlayerHealth character)
    {
        Transform spawnPoint = (character.CompareTag("CharacterA")) ? spawnPointA : spawnPointB;

        GameObject respawnEffect = Instantiate(respawnVFXPrefab, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        character.transform.position = spawnPoint.position;
        character.gameObject.SetActive(true);
        character.ResetHealth();

        Destroy(respawnEffect, 0.5f);
    }

    private void StartComboPhase(int comboLevel)
    {
        comboActive = true;
        Debug.Log($"Combo Phase {comboLevel + 1} Started!");

        CharacterAMovement characterA = FindObjectOfType<CharacterAMovement>();
        CharacterBMovement characterB = FindObjectOfType<CharacterBMovement>();
        CharacterAttack[] attacks = FindObjectsOfType<CharacterAttack>();

        if (characterA != null) characterA.StartCombo();
        if (characterB != null) characterB.StartCombo();

        foreach (var attack in attacks)
        {
            attack.enabled = false;
        }
        foreach (var attack in attacks)
        {
            attack.ResetStrongAttack(); 
        }

        CombinationInput[] combinationInputs = FindObjectsOfType<CombinationInput>();
        foreach (var input in combinationInputs)
        {
            input.StartCombo(comboLevel);
        }

        Invoke(nameof(EndComboPhase), 5f);
    }

    private void EndComboPhase()
    {
        comboActive = false;
        Debug.Log("Combo Phase Ended!");

        CharacterAMovement characterA = FindObjectOfType<CharacterAMovement>();
        CharacterBMovement characterB = FindObjectOfType<CharacterBMovement>();
        CharacterAttack[] attacks = FindObjectsOfType<CharacterAttack>();

        if (characterA != null) characterA.EndCombo();
        if (characterB != null) characterB.EndCombo();
        foreach (var attack in attacks)
        {
            attack.enabled = true;
        }

        CombinationInput[] combinationInputs = FindObjectsOfType<CombinationInput>();
        foreach (var input in combinationInputs)
        {
            input.EndCombo();
        }
    }
}
