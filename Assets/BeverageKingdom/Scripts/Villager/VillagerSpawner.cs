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

        Env env = Controller.Instance.Env.GetComponent<Env>();

        _spawnAreas.Add(env.spawnPosSlot1.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.spawnPosSlot2.GetChild(0).GetComponent<SpawnArea>());
        _spawnAreas.Add(env.spawnPosSlot3.GetChild(0).GetComponent<SpawnArea>());
    }

    void OnSpawnVillagerAtSlot1()
    {
        SpawnVillagetAt(1);
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
        Transform leftTop = _spawnAreas[index - 1].LeftTop;
        Transform rightBottom = _spawnAreas[index - 1].RightBottom;

        float minX = Mathf.Min(leftTop.position.x, rightBottom.position.x);
        float maxX = Mathf.Max(leftTop.position.x, rightBottom.position.x);
        float minY = Mathf.Min(leftTop.position.y, rightBottom.position.y);
        float maxY = Mathf.Max(leftTop.position.y, rightBottom.position.y);

        for (int i = 0; i < spawnNum; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            Vector3 moveToPosition = new Vector3(randomX, randomY, 0f);
            Villager villager = Instantiate(_villagerPrefab);
            villager.transform.position = _spawnAreas[index - 1].transform.position;

            villager.SetTargetPosition(moveToPosition);
        }
    }
}
