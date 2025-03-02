using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int scoreA = 0;
    public int scoreB = 0;
    public int winningScore = 10;

    public TextMeshProUGUI characterAScoreText;
    public TextMeshProUGUI characterBScoreText;
    public TextMeshProUGUI winnerText;

    public GameObject characterAPrefab;
    public GameObject characterBPrefab;
    public GameObject respawnVFXPrefab;
    public GameObject pauseMenuPanel;

    public Transform spawnPointA;
    public Transform spawnPointB;

    public AudioSource backgroundMusic;
    public AudioSource sfxAudioSource; 
    public AudioClip iceTransitionSFX; 

    private bool comboActive = false;
    private int comboIndex = 0;
    private List<int> comboStages = new List<int> { 5, 8, 10, 18 };
    private bool isPaused = false;

    public GameObject sandFloor;
    public GameObject iceFloor;

    public Volume globalVolume;
    private Vignette vignette;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

        sandFloor.SetActive(true);
        iceFloor.SetActive(false);

        if (globalVolume != null && globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f; 
        }
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

        if (vignette != null)
        {
            vignette.intensity.value = 0.5f;
        }

        Invoke(nameof(SwitchToIceFloor), 0.3f);

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

        CombinationUI combinationUI = FindObjectOfType<CombinationUI>();
        if (combinationUI != null)
        {
            combinationUI.StartCombo("QWE", "ASD");
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

        SwitchToSandFloor();

        if (vignette != null)
        {
            vignette.intensity.value = 0f; 
        }

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

    private void SwitchToIceFloor()
    {
        sandFloor.SetActive(false);
        iceFloor.SetActive(true);
        Debug.Log("Switched to Ice Floor!");

        PlayIceTransitionSFX();
    }

    private void SwitchToSandFloor()
    {
        sandFloor.SetActive(true);
        iceFloor.SetActive(false);
        Debug.Log("Switched to Sand Floor!");

        PlayIceTransitionSFX();
    }

    private void PlayIceTransitionSFX()
    {
        if (sfxAudioSource != null && iceTransitionSFX != null)
        {
            sfxAudioSource.PlayOneShot(iceTransitionSFX);
        }
    }

    public void TogglePauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuPanel.SetActive(false);
        }
    }
}
