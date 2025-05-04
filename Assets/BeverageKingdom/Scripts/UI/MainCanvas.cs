using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] Joystick _joystick;
    [SerializeField] RectTransform _pickSlotSpawnVillager;
    [SerializeField] Button _spawnVillagerAtSlot1Button;
    [SerializeField] Button _spawnVillagerAtSlot2Button;
    [SerializeField] Button _spawnVillagerAtSlot3Button;

    public Button SpawnVillagerButton;
    public Button ChangeWeaponButton;

    public RectTransform GameOverPopup;
    public Button PlayAgainButton;
    public Button ExitButton;

    public Action OnSpawnVillagerAtSlot1;
    public Action OnSpawnVillagerAtSlot2;
    public Action OnSpawnVillagerAtSlot3;
    public Joystick GetJoystick() { return _joystick; }

    void Awake()
    {
        InitListener();
    }

    void Start()
    {
        GameSystem.instance.OnGameOver += OnGameOver;
    }

    void OnGameOver()
    {
        GameOverPopup.gameObject.SetActive(true);
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
            _pickSlotSpawnVillager.gameObject.SetActive(true);
        });

        ChangeWeaponButton.onClick.AddListener(() =>
        {
            Debug.Log("Change weapon");
        });

        PlayAgainButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            SceneManager.LoadSceneAsync("PlayScene");
        });

        ExitButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("HomeScene");
        });
    }
}
