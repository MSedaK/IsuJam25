using UnityEngine;
using UnityEngine.UI;

public class CombinationInput : MonoBehaviour
{
    public Text comboText;  // UI'da kombinasyon göstermek için
    public float comboTime = 2f;  // Her bir tuþ için verilen süre
    private string correctCombo = "WASD";  // Doðru kombinasyon
    private float timer;
    private int currentComboIndex = 0;  // Sýradaki beklenen tuþun indexi

    private Animator animator;  // Karakterin Animator bileþeni
    public int health = 100;  // Oyuncunun saðlýðý
    private bool isComboActive = false;  // Kombinasyon baþladý mý?

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = comboTime;
        comboText.text = "SHAKE IT UP OR...";  // Ýlk mesaj
    }

    void Update()
    {
        // Eðer kombinasyon aktifse, zaman sayacý iþler
        if (isComboActive)
        {
            timer -= Time.deltaTime;

            // Eðer süre biterse, Victory animasyonunu anýnda kes ve Idle'a dön
            if (timer <= 0)
            {
                comboText.text = "TIME'S UP!";
                HealthChange(-10);  // Saðlýk azalt
                ResetToIdle();
            }
        }

        // Tuþ giriþlerini kontrol et
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
        // Eðer kombinasyon daha önce baþlamadýysa ve ilk tuþa basýldýysa, Victory animasyonu baþlat
        if (!isComboActive && key == correctCombo[0].ToString())
        {
            animator.Play("Victory", 0, 0f);  // Victory animasyonunu baþlat (SIFIRDAN)
            isComboActive = true;
        }

        // Eðer doðru tuþa bastýysa
        if (key == correctCombo[currentComboIndex].ToString())
        {
            currentComboIndex++;
            timer = comboTime;  // Süreyi yenile
            comboText.text = "Success: " + currentComboIndex + "/" + correctCombo.Length;

            // Kombinasyon tamamlandýðýnda saðlýk kazan
            if (currentComboIndex == correctCombo.Length)
            {
                comboText.text = "COMBO SUCCESS!";
                HealthChange(10);  // Saðlýk artýr
                ResetToIdle();  // Victory bitti, tekrar Idle'a dön
            }
        }
        else  // Yanlýþ tuþ basýldýysa Victory anýnda durmalý ve Idle'a geçmeli
        {
            comboText.text = "WRONG COMBO!";
            HealthChange(-10);  // Saðlýk azalt
            ResetToIdle();  // Victory iptal, anýnda Idle'a geç
        }
    }

    void HealthChange(int amount)
    {
        health += amount;
        comboText.text += "\nHealth: " + health;
    }

    void ResetToIdle()
    {
        currentComboIndex = 0;  // Kombinasyonu sýfýrla
        timer = comboTime;  // Zamaný sýfýrla
        isComboActive = false;  // Kombinasyonu sýfýrla
        animator.Play("Idle", 0, 0f);  // ANÝMASYONU ANINDA IDLE'A GEÇÝR
    }
}
