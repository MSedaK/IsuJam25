using UnityEngine;
using UnityEngine.UI;

public class CombinationInput : MonoBehaviour
{
    public Text comboText;  // UI'da kombinasyon g�stermek i�in
    public float comboTime = 2f;  // Her bir tu� i�in verilen s�re
    private string correctCombo = "WASD";  // Do�ru kombinasyon
    private float timer;
    private int currentComboIndex = 0;  // S�radaki beklenen tu�un indexi

    private Animator animator;  // Karakterin Animator bile�eni
    public int health = 100;  // Oyuncunun sa�l���
    private bool isComboActive = false;  // Kombinasyon ba�lad� m�?

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = comboTime;
        comboText.text = "SHAKE IT UP OR...";  // �lk mesaj
    }

    void Update()
    {
        // E�er kombinasyon aktifse, zaman sayac� i�ler
        if (isComboActive)
        {
            timer -= Time.deltaTime;

            // E�er s�re biterse, Victory animasyonunu an�nda kes ve Idle'a d�n
            if (timer <= 0)
            {
                comboText.text = "TIME'S UP!";
                HealthChange(-10);  // Sa�l�k azalt
                ResetToIdle();
            }
        }

        // Tu� giri�lerini kontrol et
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
        // E�er kombinasyon daha �nce ba�lamad�ysa ve ilk tu�a bas�ld�ysa, Victory animasyonu ba�lat
        if (!isComboActive && key == correctCombo[0].ToString())
        {
            animator.Play("Victory", 0, 0f);  // Victory animasyonunu ba�lat (SIFIRDAN)
            isComboActive = true;
        }

        // E�er do�ru tu�a bast�ysa
        if (key == correctCombo[currentComboIndex].ToString())
        {
            currentComboIndex++;
            timer = comboTime;  // S�reyi yenile
            comboText.text = "Success: " + currentComboIndex + "/" + correctCombo.Length;

            // Kombinasyon tamamland���nda sa�l�k kazan
            if (currentComboIndex == correctCombo.Length)
            {
                comboText.text = "COMBO SUCCESS!";
                HealthChange(10);  // Sa�l�k art�r
                ResetToIdle();  // Victory bitti, tekrar Idle'a d�n
            }
        }
        else  // Yanl�� tu� bas�ld�ysa Victory an�nda durmal� ve Idle'a ge�meli
        {
            comboText.text = "WRONG COMBO!";
            HealthChange(-10);  // Sa�l�k azalt
            ResetToIdle();  // Victory iptal, an�nda Idle'a ge�
        }
    }

    void HealthChange(int amount)
    {
        health += amount;
        comboText.text += "\nHealth: " + health;
    }

    void ResetToIdle()
    {
        currentComboIndex = 0;  // Kombinasyonu s�f�rla
        timer = comboTime;  // Zaman� s�f�rla
        isComboActive = false;  // Kombinasyonu s�f�rla
        animator.Play("Idle", 0, 0f);  // AN�MASYONU ANINDA IDLE'A GE��R
    }
}
