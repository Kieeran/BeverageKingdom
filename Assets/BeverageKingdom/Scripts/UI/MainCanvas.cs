using System;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] Joystick _joystick;
    [SerializeField] Button _spawnVillagerButton;
    [SerializeField] RectTransform _pickSlotSpawnVillager;
    [SerializeField] Button _spawnVillagerAtSlot1Button;
    [SerializeField] Button _spawnVillagerAtSlot2Button;
    [SerializeField] Button _spawnVillagerAtSlot3Button;

    public Action OnSpawnVillagerAtSlot1;
    public Action OnSpawnVillagerAtSlot2;
    public Action OnSpawnVillagerAtSlot3;
    public Joystick GetJoystick() { return _joystick; }

    void Awake()
    {
        _spawnVillagerButton.onClick.AddListener(() =>
        {
            _pickSlotSpawnVillager.gameObject.SetActive(true);
        });

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
    }
}
