using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas instance;

    [SerializeField] Joystick _joystick;
    [SerializeField] RectTransform _pickSlotSpawnVillager;
    [SerializeField] Button _spawnVillagerAtSlot1Button;
    [SerializeField] Button _spawnVillagerAtSlot2Button;
    [SerializeField] Button _spawnVillagerAtSlot3Button;

    public Button SpawnVillagerButton;
    public Button ActiveSkill;
    public Button AttackButton;

    public RectTransform GameWinOverPopup;
    public RectTransform GameWinImageText;
    public RectTransform GameOverImageText;

    public Button PlayAgainButton;
    public Button ExitButton;
    public TextMeshProUGUI tmp;
    public Image LevelProgressFillUI;

    public Action OnSpawnVillagerAtSlot1;
    public Action OnSpawnVillagerAtSlot2;
    public Action OnSpawnVillagerAtSlot3;

    public Action OnSpawnVillagerAtAllSlot;
    public Action OnAttack;
    public Action OnActiveSkill;

    public ComboBar comboBar;
    public Joystick GetJoystick() { return _joystick; }


    [Header("Animation Wave Settings")]
    [SerializeField] private TextMeshProUGUI nextWave;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private Ease fadeEase = Ease.Linear;

    [Header("")]
    private Sequence waveSequence;
    void Awake()
    {
        InitListener();
        instance = this;
    }

    void Start()
    {
        GameSystem.instance.OnGameOver += OnGameOver;
        GameSystem.instance.OnGameWin += OnGameWin;
    }

    void OnGameOver()
    {
        GameOverImageText.gameObject.SetActive(true);
        GameWinOverPopup.gameObject.SetActive(true);
    }

    void OnGameWin()
    {
        GameWinImageText.gameObject.SetActive(true);
        GameWinOverPopup.gameObject.SetActive(true);
    }

    void InitListener()
    {
        _spawnVillagerAtSlot1Button.onClick.AddListener(() =>
        {
            OnSpawnVillagerAtSlot1?.Invoke();

            _pickSlotSpawnVillager.gameObject.SetActive(false);
        });

        _spawnVillagerAtSlot2Button.onClick.AddListener(() =>
        {
            OnSpawnVillagerAtSlot2?.Invoke();

            _pickSlotSpawnVillager.gameObject.SetActive(false);
        });

        _spawnVillagerAtSlot3Button.onClick.AddListener(() =>
        {
            OnSpawnVillagerAtSlot3?.Invoke();

            _pickSlotSpawnVillager.gameObject.SetActive(false);
        });

        SpawnVillagerButton.onClick.AddListener(() =>
        {
            // _pickSlotSpawnVillager.gameObject.SetActive(true);

            OnSpawnVillagerAtAllSlot?.Invoke();
        });

        ActiveSkill.onClick.AddListener(() =>
        {
            Debug.Log("Change weapon");
            OnActiveSkill?.Invoke();
        });

        AttackButton.onClick.AddListener(() =>
        {
            OnAttack?.Invoke();
        });

        PlayAgainButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync("PlayScene");
        });

        ExitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            SoundManager.Instance?.StopSound();
            SceneManager.LoadSceneAsync("HomeScene");
        });
    }
    public void ShowNextWave(int wave, int enemyCount)
    {
        // Kill sequence trước đó nếu đang chạy
        if (waveSequence != null && waveSequence.IsActive())
            waveSequence.Kill();

        // Cài đặt text và reset alpha
        nextWave.text = $"Wave {wave}: {enemyCount} Enemies";
        nextWave.color = new Color(nextWave.color.r, nextWave.color.g, nextWave.color.b, 0f);

        // Tạo và chạy sequence animation
        waveSequence = DOTween.Sequence()
            .Append(nextWave.DOFade(1f, fadeDuration).SetEase(fadeEase))
            .AppendInterval(displayDuration)
            .Append(nextWave.DOFade(0f, fadeDuration).SetEase(fadeEase))
            .OnComplete(() => waveSequence = null);
    }

    /// <summary>
    /// Hiển thị thông báo khi hoàn thành tất cả các wave.
    /// </summary>
    public void ShowAllWavesCompleted()
    {
        // Kill sequence trước đó nếu đang chạy
        if (waveSequence != null && waveSequence.IsActive())
            waveSequence.Kill();

        // Cài đặt text và reset alpha
        nextWave.text = "All waves completed!";
        nextWave.color = new Color(nextWave.color.r, nextWave.color.g, nextWave.color.b, 0f);

        // Tạo và chạy sequence animation
        waveSequence = DOTween.Sequence()
            .Append(nextWave.DOFade(1f, fadeDuration).SetEase(fadeEase))
            .AppendInterval(displayDuration)
            .Append(nextWave.DOFade(0f, fadeDuration).SetEase(fadeEase))
            .OnComplete(() => waveSequence = null);
    }

    public void UpdateLevelProgressBar(float fillAmount)
    {
        LevelProgressFillUI.fillAmount = fillAmount;

        if (fillAmount > 1f) LevelProgressFillUI.fillAmount = 1;
    }
}
