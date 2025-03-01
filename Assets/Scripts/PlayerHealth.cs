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
        currentHealth = maxHealth;
        UpdateHealthUI();
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
            if (i < activeSegments)
                healthSegments[i].enabled = true;
            else
                healthSegments[i].enabled = false;
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");
        Vector3 deathPosition = transform.position;

        if (gameObject.CompareTag("CharacterA"))
        {
            GameManager.instance.AddScore("CharacterA");
            GameManager.instance.RespawnCharacter("CharacterA", deathPosition);
        }
        else if (gameObject.CompareTag("CharacterB"))
        {
            GameManager.instance.AddScore("CharacterB");
            GameManager.instance.RespawnCharacter("CharacterB", deathPosition);
        }

        Destroy(gameObject); 
    }
}
