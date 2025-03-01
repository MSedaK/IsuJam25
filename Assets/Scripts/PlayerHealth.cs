using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Image[] healthSegments;

    void Start()
    {
        ResetHealth();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " has " + currentHealth + " health left.");
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        int activeSegments = Mathf.CeilToInt((float)currentHealth / (maxHealth / healthSegments.Length));

        for (int i = 0; i < healthSegments.Length; i++)
        {
            healthSegments[i].enabled = i < activeSegments;
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");

        if (gameObject.CompareTag("CharacterA"))
        {
            GameManager.instance.AddScore("CharacterB"); 
        }
        else if (gameObject.CompareTag("CharacterB"))
        {
            GameManager.instance.AddScore("CharacterA"); 
        }

        gameObject.SetActive(false);
        GameManager.instance.RespawnCharacter(this);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
}
