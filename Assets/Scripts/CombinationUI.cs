using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombinationUI : MonoBehaviour
{
    public TMP_Text comboTextPrefabA; // Character A için
    public TMP_Text comboTextPrefabB; // Character B için
    public Transform comboPanelA;
    public Transform comboPanelB;
    public float moveSpeed = 100f;

    private string currentComboA;
    private string currentComboB;
    private List<TMP_Text> comboListA = new List<TMP_Text>();
    private List<TMP_Text> comboListB = new List<TMP_Text>();
    private int currentComboIndexA = 0;
    private int currentComboIndexB = 0;
    private bool isComboActive = false;

    void Start()
    {
        comboPanelA.gameObject.SetActive(false);
        comboPanelB.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isComboActive)
        {
            MoveComboUI();
        }
    }

    public void StartCombo(string comboA, string comboB)
    {
        isComboActive = true;
        currentComboA = comboA;
        currentComboB = comboB;
        comboPanelA.gameObject.SetActive(true);
        comboPanelB.gameObject.SetActive(true);
        GenerateComboUI();
    }

    public void EndCombo()
    {
        isComboActive = false;
        comboPanelA.gameObject.SetActive(false);
        comboPanelB.gameObject.SetActive(false);
    }

    void GenerateComboUI()
    {
        ResetComboUI();

        for (int i = 0; i < currentComboA.Length; i++)
        {
            TMP_Text newTextA = Instantiate(comboTextPrefabA, comboPanelA);
            newTextA.text = currentComboA[i].ToString();
            comboListA.Add(newTextA);
        }

        for (int i = 0; i < currentComboB.Length; i++)
        {
            TMP_Text newTextB = Instantiate(comboTextPrefabB, comboPanelB);
            newTextB.text = currentComboB[i].ToString();
            comboListB.Add(newTextB);
        }

        currentComboIndexA = 0;
        currentComboIndexB = 0;
    }

    void MoveComboUI()
    {
        foreach (TMP_Text text in comboListA)
        {
            if (text != null && text.gameObject.activeSelf)
            {
                text.transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }
        }

        foreach (TMP_Text text in comboListB)
        {
            if (text != null && text.gameObject.activeSelf)
            {
                text.transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void CheckInput(string key, string character)
    {
        if (!isComboActive) return;

        if (character == "CharacterA")
        {
            if (currentComboIndexA < comboListA.Count && comboListA[currentComboIndexA] != null && comboListA[currentComboIndexA].text == key)
            {
                comboListA[currentComboIndexA].gameObject.SetActive(false);
                currentComboIndexA++;

                if (currentComboIndexA == currentComboA.Length)
                {
                    Debug.Log("Character A Combo Completed!");
                    CheckBothCombos();
                }
            }
            else
            {
                Debug.Log("Character A Wrong Key! Resetting...");
                GenerateComboUI();
            }
        }
        else if (character == "CharacterB")
        {
            if (currentComboIndexB < comboListB.Count && comboListB[currentComboIndexB] != null && comboListB[currentComboIndexB].text == key)
            {
                comboListB[currentComboIndexB].gameObject.SetActive(false);
                currentComboIndexB++;

                if (currentComboIndexB == currentComboB.Length)
                {
                    Debug.Log("Character B Combo Completed!");
                    CheckBothCombos();
                }
            }
            else
            {
                Debug.Log("Character B Wrong Key! Resetting...");
                GenerateComboUI();
            }
        }
    }

    void CheckBothCombos()
    {
        if (currentComboIndexA == currentComboA.Length && currentComboIndexB == currentComboB.Length)
        {
            Debug.Log("Both Players Completed Their Combos!");
            EndCombo();
        }
    }

    void ResetComboUI()
    {
        foreach (TMP_Text text in comboListA)
        {
            if (text != null)
            {
                Destroy(text.gameObject);
            }
        }
        comboListA.Clear();
        currentComboIndexA = 0;

        foreach (TMP_Text text in comboListB)
        {
            if (text != null)
            {
                Destroy(text.gameObject);
            }
        }
        comboListB.Clear();
        currentComboIndexB = 0;
    }
}
