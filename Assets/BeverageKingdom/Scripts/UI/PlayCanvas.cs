using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayCanvas : MonoBehaviour
{
    public static PlayCanvas Instance;

    [SerializeField] Joystick _joystick;
    [SerializeField] RectTransform _pickSlotSpawnVillager;
    [SerializeField] Button _spawnVillagerAtSlot1Button;
    [SerializeField] Button _spawnVillagerAtSlot2Button;
    [SerializeField] Button _spawnVillagerAtSlot3Button;

    [SerializeField] Button _cheatButton;

    public Button SpawnVillagerButton;
    public Button ActiveSkill;
    public Button AttackButton;

    public Button PauseGameButton;
    public Transform PauseGamePopUp;

    public GameWinOverPopup GameWinOverPopup;
    public MileStone MileStoneProgressBar;

    public SkillVisualize SkillVisualize;
    public Image LevelProgressFillUI;

    public Action OnSpawnVillagerAtSlot1;
    public Action OnSpawnVillagerAtSlot2;
    public Action OnSpawnVillagerAtSlot3;

    public Action OnSpawnVillagerAtAllSlot;
    public Action OnAttack;
    public Action OnActiveSkill;

    public ComboBar comboBar;
    public Joystick GetJoystick() { return _joystick; }

    public TextMeshProUGUI levelText;

    [Header("Animation Wave Settings")]
    [SerializeField] TextMeshProUGUI _nextWave;
    [SerializeField] float _fadeDuration = 0.5f;
    [SerializeField] float _displayDuration = 2f;
    [SerializeField] Ease _fadeEase = Ease.Linear;

    [Header("")]
    private Sequence waveSequence;
    void Awake()
    {
        InitListener();
        Instance = this;
    }

    void Start()
    {
        GameSystem.Instance.OnGameOver += OnGameOver;
        GameSystem.Instance.OnGameWin += OnGameWin;
    }

    void OnGameOver()
    {
        GameWinOverPopup.gameObject.SetActive(true);

        GameWinOverPopup.OnGameOver();
    }

    void OnGameWin()
    {
        GameWinOverPopup.gameObject.SetActive(true);

        GameWinOverPopup.OnGameWin();
    }

    public void UpdateLevelText(int levelNum)
    {
        levelText.text = "Lv. " + levelNum.ToString();
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
            OnActiveSkill?.Invoke();
        });

        AttackButton.onClick.AddListener(() =>
        {
            OnAttack?.Invoke();
        });

        _cheatButton.onClick.AddListener(() =>
        {
            Cheater.Instance.ToggleVisualizeHp();

            Controller.Instance.VisualizeDetectionRange.Value = !Controller.Instance.VisualizeDetectionRange.Value;
            Controller.Instance.VisualizeBoundingBox.Value = !Controller.Instance.VisualizeBoundingBox.Value;
        });

        PauseGameButton.onClick.AddListener(() =>
        {
            PauseGamePopUp.gameObject.SetActive(true);
        });

        if (Cheater.Instance == null)
        {
            _cheatButton.gameObject.SetActive(false);
        }
    }

    public void ShowNextWave(int wave, int enemyCount)
    {
        // Kill sequence trước đó nếu đang chạy
        if (waveSequence != null && waveSequence.IsActive())
            waveSequence.Kill();

        // Cài đặt text và reset alpha
        _nextWave.text = $"Wave {wave}: {enemyCount} Enemies";
        _nextWave.color = new Color(_nextWave.color.r, _nextWave.color.g, _nextWave.color.b, 0f);

        // Tạo và chạy sequence animation
        waveSequence = DOTween.Sequence()
            .Append(_nextWave.DOFade(1f, _fadeDuration).SetEase(_fadeEase))
            .AppendInterval(_displayDuration)
            .Append(_nextWave.DOFade(0f, _fadeDuration).SetEase(_fadeEase))
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
        _nextWave.text = "All waves completed!";
        _nextWave.color = new Color(_nextWave.color.r, _nextWave.color.g, _nextWave.color.b, 0f);

        // Tạo và chạy sequence animation
        waveSequence = DOTween.Sequence()
            .Append(_nextWave.DOFade(1f, _fadeDuration).SetEase(_fadeEase))
            .AppendInterval(_displayDuration)
            .Append(_nextWave.DOFade(0f, _fadeDuration).SetEase(_fadeEase))
            .OnComplete(() => waveSequence = null);
    }

    public void UpdateLevelProgressBar(float fillAmount)
    {
        LevelProgressFillUI.fillAmount = fillAmount;

        if (fillAmount > 1f) LevelProgressFillUI.fillAmount = 1;
    }
}
