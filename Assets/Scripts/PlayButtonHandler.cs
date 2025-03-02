using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour
{
    // Bu fonksiyonu butonun OnClick() event'ine atay�n
    public void StartGame()
    {
        // Oyun sahnesini y�klemek i�in (ayn� sahnede ba�ka UI ya da oyun modlar�na ge�i� yapacaksan�z ona g�re d�zenleyin)
        SceneManager.LoadScene("TutorialScene");
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

}
