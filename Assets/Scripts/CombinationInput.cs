using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombinationInput : MonoBehaviour
{
    public TMP_Text comboText;
    public float comboTime = 10f;
    private float timer;
    private int currentComboIndex = 0;

    private bool isComboActive = false;
    private bool comboCompleted = false;

    private CharacterAMovement characterAMovement;
    private CharacterBMovement characterBMovement;
    private CombinationUI combinationUI;
    private PlayerHealth playerHealth;
    private Animator animator; 

    private string[] combosA = { "WASDWA", "DSAWDS", "AWDSAW", "SDWASD" };
    private string[] combosB = { "↑↓←→", "→←↓↑", "↓→↑←", "←↑→↓" };
    private string currentCombo;

    void Start()
    {
        combinationUI = FindObjectOfType<CombinationUI>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();

        if (CompareTag("CharacterA"))
        {
            characterAMovement = GetComponent<CharacterAMovement>();
        }
        else if (CompareTag("CharacterB"))
        {
            characterBMovement = GetComponent<CharacterBMovement>();
        }

        comboText.text = "";
    }

    void Update()
    {
        if (isComboActive && !comboCompleted)
        {
            timer -= Time.deltaTime;

            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        string keyPressed = key.ToString();
                        Debug.Log("Pressed: " + keyPressed);
                        CheckInput(keyPressed);
                    }
                }
            }

            if (timer <= 0)
            {
                Debug.Log("Combo Time Expired - Resetting");
                comboText.text = "<color=red>TIME'S UP!</color>";
                ApplyHealthChange(-10);
                ResetToIdle();
            }
        }
    }

    public void StartCombo(int comboLevel)
    {
        isComboActive = true;
        comboCompleted = false;
        timer = comboTime;
        currentComboIndex = 0;

        string comboA = combosA[comboLevel];
        string comboB = combosB[comboLevel];

        combinationUI.StartCombo(comboA, comboB);

        if (CompareTag("CharacterA"))
        {
            characterAMovement.StartCombo();
            currentCombo = comboA;
        }
        else if (CompareTag("CharacterB"))
        {
            characterBMovement.StartCombo();
            currentCombo = comboB;
        }

        comboText.text = "<color=yellow>COMBO STARTED!</color>";
        Debug.Log($"Combo {comboLevel + 1} Triggered! Required: {currentCombo}");

        Invoke("EndCombo", comboTime);
    }

    public void EndCombo()
    {
        if (!isComboActive) return;

        isComboActive = false;
        comboCompleted = false;
        timer = comboTime;
        comboText.text = "";

        if (CompareTag("CharacterA")) characterAMovement.EndCombo();
        if (CompareTag("CharacterB")) characterBMovement.EndCombo();

        combinationUI.EndCombo();
        Debug.Log("Combo Ended!");
    }

    void CheckInput(string input)
    {
        if (!isComboActive || comboCompleted) return;

        string expectedKey = currentCombo[currentComboIndex].ToString().ToUpper();
        string inputKey = input.ToUpper();

        if (expectedKey == inputKey)
        {
            Debug.Log($"Correct Input: {inputKey}");
            currentComboIndex++;

            if (currentComboIndex == currentCombo.Length)
            {
                comboCompleted = true;
                Debug.Log("Combo Completed Successfully!");
                comboText.text = "<color=green>COMBO SUCCESS!</color>";

                CharacterAttack attackScript = GetComponent<CharacterAttack>();
                if (attackScript != null)
                {
                    attackScript.UnlockStrongAttack();
                }

                ApplyHealthChange(100);
                EndCombo();
            }
        }
        else
        {
            Debug.Log($"Wrong Key! Expected: {expectedKey}, Got: {inputKey}");
            ApplyHealthChange(-10);
        }
    }

    void ApplyHealthChange(int amount)
    {
        if (amount > 0 && !comboCompleted) return;

        playerHealth.TakeDamage(-amount);
        comboText.text += $"\n<color=yellow>Health: {playerHealth.GetHealth()}</color>";
        Debug.Log($"Health Changed: {playerHealth.GetHealth()}");
    }

    void ResetToIdle()
    {
        if (comboCompleted) return;
        EndCombo();
    }

    void PlayVictoryAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Victory"); // Önce Trigger’ı sıfırlıyoruz
            animator.Play("Victory", -1, 0f); 
            Debug.Log("Victory Animation Triggered Instantly!");
        }
        else
        {
            Debug.LogWarning("Animator component not found!");
        }
    }
}
