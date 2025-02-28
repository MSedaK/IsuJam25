using UnityEngine;
using UnityEngine.UI;

public class CombinationInput : MonoBehaviour
{
    public Text comboText;
    public float comboTime = 2f;
    private string correctCombo = "WASD";
    private float timer;
    private int currentComboIndex = 0;

    private Animator animator;
    public int health = 100;
    private bool isComboActive = false;
    private bool comboCompleted = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = comboTime;
        comboText.text = "SHAKE IT UP OR...";
    }

    void Update()
    {
        if (isComboActive && !comboCompleted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                comboText.text = "TIME'S UP!";
                HealthChange(-10);
                ResetToIdle();
            }
        }

        if (currentComboIndex < correctCombo.Length)
        {
            if (Input.GetKeyDown(KeyCode.W)) CheckInput("W");
            if (Input.GetKeyDown(KeyCode.A)) CheckInput("A");
            if (Input.GetKeyDown(KeyCode.S)) CheckInput("S");
            if (Input.GetKeyDown(KeyCode.D)) CheckInput("D");
        }
    }

    void CheckInput(string key)
    {
        if (!isComboActive && key == correctCombo[0].ToString())
        {
            animator.Play("Victory", 0, 0f);
            isComboActive = true;
        }

        if (key == correctCombo[currentComboIndex].ToString())
        {
            currentComboIndex++;
            timer = comboTime;
            comboText.text = "Success: " + currentComboIndex + "/" + correctCombo.Length;

            if (currentComboIndex == correctCombo.Length)
            {
                comboText.text = "COMBO SUCCESS!";
                HealthChange(10);
                comboCompleted = true;
            }
        }
        else
        {
            comboText.text = "WRONG COMBO!";
            HealthChange(-10);
            ResetToIdle();
        }
    }

    void HealthChange(int amount)
    {
        health += amount;
        comboText.text += "\nHealth: " + health;
    }

    void ResetToIdle()
    {
        if (comboCompleted) return;

        currentComboIndex = 0;
        timer = comboTime;
        isComboActive = false;
        animator.Play("Idle", 0, 0f);
    }
}
