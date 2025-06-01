using TMPro;
using UnityEngine;

public class Env : MonoBehaviour
{
    public Transform VillagerSpawnPosSlot1;
    public Transform VillagerSpawnPosSlot2;
    public Transform VillagerSpawnPosSlot3;

    public Transform EnemySpawnPosSlot1;
    public Transform EnemySpawnPosSlot2;
    public Transform EnemySpawnPosSlot3;

    public TMP_Text houseHP1;
    public TMP_Text houseHP2;
    public TMP_Text houseHP3;
    public void UpdateHpInfo(bool b)
    {
        houseHP1.gameObject.SetActive(b);
        houseHP2.gameObject.SetActive(b);
        houseHP3.gameObject.SetActive(b);
    }
}