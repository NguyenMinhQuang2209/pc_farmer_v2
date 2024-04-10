using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI remainPointTxt;
    [SerializeField] private Slider exeSlider;
    [SerializeField] private TextMeshProUGUI exeTxt;

    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private Button plusHealthBtn;
    [SerializeField] private TextMeshProUGUI healthPlusTxt;
    [SerializeField] private TextMeshProUGUI hungryTxt;
    [SerializeField] private Button plusHungyBtn;
    [SerializeField] private TextMeshProUGUI hungryPlusTxt;
    [SerializeField] private TextMeshProUGUI speedTxt;
    [SerializeField] private Button plusSpeedBtn;
    [SerializeField] private TextMeshProUGUI speedPlusTxt;
    [SerializeField] private TextMeshProUGUI recoverHealthTxt;
    [SerializeField] private Button plusRecoverHealthBtn;
    [SerializeField] private TextMeshProUGUI recoverHealthPlusTxt;

    private int plusHealth = 0;
    private int plusHungry = 0;
    private int plusSpeed = 0;
    private int plusRecoverHealth = 0;

    private int remainPoint = 0;

    private int currentLevel = 1;
    [SerializeField] private float levelRate = 1.2f;
    [SerializeField] private float firstLevelExe = 10f;
    float targetExe = 0f;
    float currentExe = 0f;

    private PlayerHealth playerHealth = null;

    [Header("Health Food UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthShowTxt;
    [SerializeField] private Slider foodSlider;
    [SerializeField] private TextMeshProUGUI foodShowTxt;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        plusHealthBtn.onClick.AddListener(() =>
        {
            Plus("Health");
        });
        plusHungyBtn.onClick.AddListener(() =>
        {
            Plus("Food");
        });
        plusSpeedBtn.onClick.AddListener(() =>
        {
            Plus("Speed");
        });
        plusRecoverHealthBtn.onClick.AddListener(() =>
        {
            Plus("Recover");
        });

        targetExe = firstLevelExe * currentLevel;

        levelTxt.text = "Cấp: " + currentLevel.ToString();
        remainPointTxt.text = "Điểm tiềm năng: " + remainPoint.ToString();
        exeSlider.minValue = 0f;
        exeSlider.maxValue = targetExe;
        exeSlider.value = currentExe;

        exeTxt.text = currentExe + "/" + targetExe;

        Transform player = PreferenceController.instance.GetPlayer();

        if (player.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            UpdateTxt();
            healthSlider.maxValue = playerHealth.GetMaxHealth();
            foodSlider.maxValue = playerHealth.GetMaxFood();

            healthShowTxt.text = Mathf.Round(playerHealth.GetCurrentHealth()) + "/" + Mathf.Round(playerHealth.GetMaxHealth());
            foodShowTxt.text = Mathf.Round(playerHealth.GetCurrentFood()) + "/" + Mathf.Round(playerHealth.GetMaxFood());
        }

        healthPlusTxt.text = "+" + plusHealth;
        hungryPlusTxt.text = "+" + plusHungry;
        speedPlusTxt.text = "+" + plusSpeed;
        recoverHealthPlusTxt.text = "+" + plusRecoverHealth;

        healthSlider.minValue = 0f;
        foodSlider.minValue = 0f;

    }

    private void Update()
    {
        string cursor = CursorController.instance.CurrentCursor();
        if (cursor != "")
        {
            healthTxt.text = "Máu: " + Mathf.Round(playerHealth.GetCurrentHealth()) + "/" + Mathf.Round(playerHealth.GetMaxHealth());
            hungryTxt.text = "Đói: " + Mathf.Round(playerHealth.GetCurrentFood()) + "/" + Mathf.Round(playerHealth.GetMaxFood());
        }

        healthSlider.value = playerHealth.GetCurrentHealth();
        foodSlider.value = playerHealth.GetCurrentFood();
        healthShowTxt.text = Mathf.Round(playerHealth.GetCurrentHealth()) + "/" + Mathf.Round(playerHealth.GetMaxHealth());
        foodShowTxt.text = Mathf.Round(playerHealth.GetCurrentFood()) + "/" + Mathf.Round(playerHealth.GetMaxFood());

    }

    public void UpdateTxt()
    {
        healthTxt.text = "Máu: " + Mathf.Round(playerHealth.GetCurrentHealth()) + "/" + Mathf.Round(playerHealth.GetMaxHealth());
        hungryTxt.text = "Đói: " + Mathf.Round(playerHealth.GetCurrentFood()) + "/" + Mathf.Round(playerHealth.GetMaxFood());
        speedTxt.text = "Tốc độ:" + playerHealth.GetSpeed();
        recoverHealthTxt.text = "Tốc độ hồi phục: " + playerHealth.GetRecoverRate();
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public float GetCurrentExe()
    {
        return currentExe;
    }
    public int GetPlusHealth()
    {
        return plusHealth;
    }
    public int GetPlusFood()
    {
        return plusHungry;
    }
    public int GetPlusSpeed()
    {
        return plusSpeed;
    }
    public int GetPlusRecover()
    {
        return plusRecoverHealth;
    }

    public void Plus(string v)
    {
        if (remainPoint == 0)
        {
            LogController.instance.Log(LogMode.Lack_Coin);
            return;
        }
        switch (v)
        {
            case "Health":
                plusHealth += 1;
                remainPoint -= 1;
                break;
            case "Food":
                plusHungry += 1;
                remainPoint -= 1;
                break;
            case "Speed":
                plusSpeed += 1;
                remainPoint -= 1;
                break;
            case "Recover":
                plusRecoverHealth += 1;
                remainPoint -= 1;
                break;
        }
        healthPlusTxt.text = "+" + plusHealth;
        hungryPlusTxt.text = "+" + plusHungry;
        speedPlusTxt.text = "+" + plusSpeed;
        recoverHealthPlusTxt.text = "+" + plusRecoverHealth;
        remainPointTxt.text = "Điểm tiềm năng: " + remainPoint.ToString();
        playerHealth.ChangePlus(plusHealth, plusHungry, plusSpeed, plusRecoverHealth);

        healthSlider.maxValue = playerHealth.GetMaxHealth();
        foodSlider.maxValue = playerHealth.GetMaxFood();

        UpdateTxt();
    }
    public void AddExe(float v)
    {
        currentExe += v;
        Transform player = PreferenceController.instance.GetPlayer();
        ShowUIWorldController.instance.ShowWorldTextItem(player.position, v.ToString(), new(1.0f, 0.5f, 0.0f));
        if (currentExe >= targetExe)
        {
            currentLevel++;
            targetExe = Mathf.Ceil(targetExe * levelRate);
            currentExe = 0;
            exeSlider.maxValue = targetExe;
            remainPoint++;
            remainPointTxt.text = "Điểm tiềm năng: " + remainPoint.ToString();
            levelTxt.text = "Cấp: " + currentLevel.ToString();
        }
        exeTxt.text = currentExe + "/" + targetExe;
        exeSlider.value = currentExe;
    }

    public void AddExeTotal(float v)
    {
        AddExe(v);
        PetController.instance.AddExeToPets(v);
    }
}
