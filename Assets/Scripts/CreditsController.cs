using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public GameObject creditsPanel; // Credits panelini referans al
    private bool isOpen = false;

    public void ShowCredits()
    {
        isOpen = true;
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        isOpen = false;
        creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("oyuncikti");
    }
}
