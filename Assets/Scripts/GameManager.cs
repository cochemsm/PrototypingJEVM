using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance => instance;

    private Image healthbar;
    private Image oilbar;

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
    }

    public void SetHealthbar(float fill) {
        healthbar.fillAmount = fill;
    }

    public void SetOilbar(float fill) {
        healthbar.fillAmount = fill;
    }
}