using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour
{
    // Bu fonksiyonu butonun OnClick() event'ine atayýn
    public void StartGame()
    {
        // Oyun sahnesini yüklemek için (ayný sahnede baþka UI ya da oyun modlarýna geçiþ yapacaksanýz ona göre düzenleyin)
        SceneManager.LoadScene("TutorialScene");
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

}
