using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CombinationUI : MonoBehaviour
{
    public TMP_Text comboTextPrefab;
    public Transform comboPanel;
    public float moveSpeed = 100f;

    private string correctCombo = "WASD";
    private List<TMP_Text> comboList = new List<TMP_Text>();
    private int currentComboIndex = 0;
    private bool isComboRunning = true;

    void Start()
    {
        GenerateComboUI();
    }

    void Update()
    {
        if (isComboRunning)
        {
            MoveComboUI();
        }

        if (Input.GetKeyDown(KeyCode.W)) CheckInput("W");
        if (Input.GetKeyDown(KeyCode.A)) CheckInput("A");
        if (Input.GetKeyDown(KeyCode.S)) CheckInput("S");
        if (Input.GetKeyDown(KeyCode.D)) CheckInput("D");
    }

    void GenerateComboUI()
    {
        ResetComboUI();

        for (int i = correctCombo.Length - 1; i >= 0; i--)
        {
            TMP_Text newText = Instantiate(comboTextPrefab, comboPanel);
            newText.text = correctCombo[i].ToString();
            comboList.Insert(0, newText);
        }
        currentComboIndex = 0;
    }

    void MoveComboUI()
    {
        foreach (TMP_Text text in comboList)
        {
            if (text != null && text.gameObject.activeSelf) 
            {
                text.transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void CheckInput(string key)
    {
        if (currentComboIndex < comboList.Count && comboList[currentComboIndex] != null && comboList[currentComboIndex].text == key)
        {
            comboList[currentComboIndex].gameObject.SetActive(false); 
            currentComboIndex++;

            if (currentComboIndex == correctCombo.Length)
            {
                Debug.Log("Combo Completed!");
                isComboRunning = false;
            }
        }
        else
        {
            Debug.Log("Wrong Key! Resetting...");
            GenerateComboUI();
        }
    }

    void ResetComboUI()
    {
        foreach (TMP_Text text in comboList)
        {
            if (text != null)
            {
                Destroy(text.gameObject);
            }
        }
        comboList.Clear();
        currentComboIndex = 0;
        isComboRunning = true;
    }
}
