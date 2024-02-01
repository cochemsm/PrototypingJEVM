using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public void PlayButton() {
        SceneManager.LoadScene("PlayerLevel");
    }
    
    public void ResumeButton() {
        GameManager.Instance.TriggerPauseMenu();
    }

    public void OptionsButton() {
        
    }

    public void ExitButton() {
        SceneManager.LoadScene("MainMenu");
        GameManager.Instance.DestroySelf();
    }

    public void QuitButton() {
        Application.Quit();
    }
}