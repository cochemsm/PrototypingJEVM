using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public void ResumeButton() {
        GameManager.Instance.TriggerPauseMenu();
    }

    public void OptionsButton() {
        
    }

    public void ExitButton() {
        Application.Quit();
    }
}