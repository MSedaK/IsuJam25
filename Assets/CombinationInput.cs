using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationInput : MonoBehaviour
{
    public Text comboText;  
    public float comboTime = 4f;  
    private string correctCombo = "WSDA";  
    private float timer;
    private string currentInput = "";

    private Animator animator; 
    public int health = 100;  

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = comboTime;
        comboText.text = "SHAKE IT UP OR...";  
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            comboText.text = "TIME'S UP!";  
            HealthChange(-10);  
            ResetCombo();
        }

        if (Input.GetKeyDown(KeyCode.W)) AddInput("W");
        if (Input.GetKeyDown(KeyCode.S)) AddInput("S");
        if (Input.GetKeyDown(KeyCode.D)) AddInput("D");
        if (Input.GetKeyDown(KeyCode.A)) AddInput("A");
    }

    void AddInput(string key)
    {
        currentInput += key;
        if (currentInput == correctCombo)
        {
            comboText.text = "SUCCESS!";
            HealthChange(10); 
            animator.SetTrigger("Victory"); 
            ResetCombo();
        }
        else if (!correctCombo.StartsWith(currentInput))
        {
            comboText.text = "WRONG COMBO!";
            HealthChange(-10);  
            ResetCombo();
        }
    }

    void HealthChange(int amount)
    {
        health += amount;
        comboText.text += "\nHealth: " + health;
    }

    void ResetCombo()
    {
        currentInput = "";
        timer = comboTime;  
    }
}
