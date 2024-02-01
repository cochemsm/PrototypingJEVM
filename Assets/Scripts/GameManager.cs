using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance => instance;

    private Image healthbar;
    private Image oilbar;
    private Image bossbar;
    private GameObject bossbarObject;
    private GameObject gameOverScreen;
    private GameObject pauseMenu;

    private bool gameOver = true;

    [SerializeField] private Sprite[] oilIcons;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        SetRefrences();
    }

    private void SetRefrences() {
        healthbar = GameObject.FindGameObjectWithTag("healthbar").GetComponent<Image>();
        oilbar = GameObject.FindGameObjectWithTag("oilbar").GetComponent<Image>();
        bossbar = GameObject.FindGameObjectWithTag("bossbar").transform.GetChild(1).GetComponent<Image>();
        bossbarObject = GameObject.FindGameObjectWithTag("bossbar");
        bossbarObject.SetActive(false);
        gameOverScreen = GameObject.FindGameObjectWithTag("gameOver");
        pauseMenu = GameObject.FindGameObjectWithTag("pauseMenu");
        TriggerPauseMenu();
    }

    public void SetHealthbar(float fill) {
        healthbar.fillAmount = fill;
    }

    public void SetOilbar(int icon) {
        oilbar.sprite = oilIcons[icon];
    }

    public void SetBossHealth(float fill) {
        bossbar.fillAmount = fill;
    }

    public void ActivateBossbar() {
        bossbarObject.SetActive(true);
    }

    public void TriggerGameOverScreen() {
        gameOverScreen.SetActive(!gameOverScreen.activeSelf);
        gameOver = !gameOver;
        Time.timeScale = 0;
    }

    public void TriggerPauseMenu() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (Time.timeScale == 0 && !gameOver) Time.timeScale = 1;
        else Time.timeScale = 0;
        TriggerCursor();
    }

    public void GameWon() {
        SceneManager.LoadScene("MainMenu");
        AudioManager.Instance.PlayMusic(AudioManager.BackgroundMusic);
        TriggerCursor();
    }

    private void TriggerCursor() {
        Cursor.visible = !Cursor.visible;
        // Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}