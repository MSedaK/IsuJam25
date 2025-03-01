using UnityEngine;
using TMPro;

public class CombinationInput : MonoBehaviour
{
    public TMP_Text comboText;
    public float comboTime = 2f;
    private string correctCombo = "WASD";
    private float timer;
    private int currentComboIndex = 0;

    private Animator animator;
    public int health = 100;
    private bool isComboActive = false;
    private bool comboCompleted = false;

    private CharacterMovement characterMovement;
    private CombinationUI combinationUI;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
        combinationUI = FindObjectOfType<CombinationUI>();
        timer = comboTime;
        comboText.text = "";
    }

    void Update()
    {
        if (isComboActive && !comboCompleted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Debug.Log("Combo Time Expired - Resetting");
                comboText.text = "<color=red>TIME'S UP!</color>";
                HealthChange(-10);
                ResetToIdle();
            }
        }
    }

    void StartCombo()
    {
        isComboActive = true;
        characterMovement.StartCombo();
        combinationUI.StartCombo();
        comboText.text = "<color=yellow>COMBO STARTED!</color>";
        Debug.Log("Combo Triggered!");
    }

    void EndComboSequence()
    {
        Invoke("ResetToIdle", 1f);
    }

    void ResetToIdle()
    {
        if (comboCompleted) return;

        isComboActive = false;
        characterMovement.EndCombo();
        combinationUI.EndCombo();
        comboText.text = "";
        Debug.Log("Combo Ended - Movement Restored");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ComboTrigger") && !isComboActive)
        {
            StartCombo();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ComboTrigger") && isComboActive)
        {
            ResetToIdle();
        }
    }

    void HealthChange(int amount)
    {
        health += amount;
        comboText.text += "\n<color=yellow>Health: " + health + "</color>";
        Debug.Log("Health Changed: " + health);
    }
}
