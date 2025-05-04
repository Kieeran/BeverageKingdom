using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : TriBehaviour
{
    [SerializeField] protected List<Transform> prefabs;
    [SerializeField] protected List<Transform> poolObjs;
    [SerializeField] protected Transform holder;
    protected override void Reset()
    {
        LoadComponent();
    }
     protected  override void Awake()
    {
        base.Awake();
        LoadComponent();
    }
    protected override void LoadComponent()
    {
        LoadPrefabs();
        LoadHolder();

    }
    protected virtual void HidePrefabs()
    {
        foreach (Transform prefab in prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }
    protected override void LoadPrefabs()
    {
        if (prefabs.Count > 0) return;
        Transform preObj = transform.Find("Prefabs");
        foreach (Transform prefab in preObj)
        {
            prefabs.Add(prefab);
        }
        HidePrefabs();
    }
    protected virtual void LoadHolder()
    {
        if (holder != null) return;
        holder = transform.Find("Holder");
    }

    public virtual void Despawm(Transform obj)
    {
        this.poolObjs.Add(obj);
        obj.gameObject.SetActive(false);
    }
    public virtual Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        Transform prefab = this.GetPrefabByName(prefabName);
        if (prefab == null)
        {   
            Debug.LogWarning("Prefab not found: " + prefabName);
            return null;
        }

      return  Spawn(prefab, spawnPos, rotation);
    }
    public virtual Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform newPrefab = this.GetObjectFromPool(prefab);
        newPrefab.SetPositionAndRotation(spawnPos, rotation);
        newPrefab.SetParent(this.holder);
        newPrefab.gameObject.SetActive(true);
        return newPrefab;
    }

    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (Transform poolObj in this.poolObjs)
        {
            if (poolObj == null) continue;

            if (poolObj.name == prefab.name)
            {
                this.poolObjs.Remove(poolObj);
                return poolObj;
            }
        }

        Transform newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }
    public virtual Transform GetPrefabByName(string prefabName)
    {
        foreach (Transform prefab in this.prefabs)
        {
            if (prefab.name == prefabName) return prefab;
        }

        return null;
    }
    public virtual Transform randomPrefabs()
    {
        int rand = Random.Range(0, this.prefabs.Count);
        return prefabs[rand];
    }
    public virtual Transform randomPrefabHolder()
    {
        List<Transform> activeEnemies = new List<Transform>();
        foreach (Transform child in holder)
        {
            if (!child.gameObject.activeSelf)
                continue;
            activeEnemies.Add(child);
        }

        if (activeEnemies.Count == 0)
            return null;

        int rand = Random.Range(0, activeEnemies.Count);
        return activeEnemies[rand];
    }

}
