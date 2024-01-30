using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance => instance;

    private Image healthbar;
    private Image oilbar;
    private Image bossbar;
    private GameObject bossbarObject;

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
}