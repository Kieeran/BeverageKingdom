using System.Collections.Generic;
using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    [SerializeField] Villager _villagerPrefab;

    List<SpawnArea> _spawnAreas = new();

    public int spawnNum;

    void Start()
    {
        UIManager.Instance.MainCanvas.OnSpawnVillagerAtSlot1 += OnSpawnVillagerAtSlot1;
        UIManager.Instance.MainCanvas.OnSpawnVillagerAtSlot2 += OnSpawnVillagerAtSlot2;
        UIManager.Instance.MainCanvas.OnSpawnVillagerAtSlot3 += OnSpawnVillagerAtSlot3;

        UIManager.Instance.MainCanvas.OnSpawnVillagerAtAllSlot += OnSpawnVillagerAtAllSlot;

        Env env = Controller.Instance.Env.GetComponent<Env>();

        _spawnAreas.Add(env.VillagerSpawnPosSlot1.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.VillagerSpawnPosSlot2.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.VillagerSpawnPosSlot3.GetChild(0).GetComponent<SpawnArea>());
    }

    void OnSpawnVillagerAtSlot1()
    {
        SpawnVillagetAt(1);
    }

    void OnSpawnVillagerAtAllSlot()
    {
        OnSpawnVillagerAtSlot1();
        OnSpawnVillagerAtSlot2();
        OnSpawnVillagerAtSlot3();
    }

    void OnSpawnVillagerAtSlot2()
    {
        SpawnVillagetAt(2);
    }

    void OnSpawnVillagerAtSlot3()
    {
        SpawnVillagetAt(3);
    }

    void SpawnVillagetAt(int index)
    {
        for (int i = 0; i < spawnNum; i++)
        {
            Villager villager = Instantiate(_villagerPrefab);
            villager.transform.position = _spawnAreas[index - 1].transform.position;

            villager.SetTargetPosition(_spawnAreas[index - 1].GetRandomSpawnPos());
        }
    }
}
