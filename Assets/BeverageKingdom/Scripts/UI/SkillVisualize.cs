using UnityEngine;
using UnityEngine.UI;

public class SkillVisualize : MonoBehaviour
{
    public Image IceSkillVisualize;
    public Image ThunderSkillVisualize;

    void Awake()
    {
        IceSkillVisualize.gameObject.SetActive(false);
        ThunderSkillVisualize.gameObject.SetActive(false);
    }
}
