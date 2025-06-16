using System.Collections;
using TMPro;
using UnityEngine;

public class WaveInform : MonoBehaviour
{
    public TMP_Text WaveInformText;

    public float fadeInDuration = 1f;
    public float visibleDuration = 2f;
    public float fadeOutDuration = 1f;

    void Awake()
    {
        WaveInformText.text = "";
    }

    void Start()
    {
        LevelController.Instance.StartWave += StartEffect;
    }

    void StartEffect(int waveNum)
    {
        WaveInformText.text = "Wave " + (waveNum + 1).ToString();
        SetAlpha(0); // reset alpha sau khi set text
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeInDuration));
        yield return new WaitForSeconds(visibleDuration);
        yield return StartCoroutine(Fade(1f, 0f, fadeOutDuration));
        WaveInformText.text = ""; // xóa sau khi fade-out hoàn toàn
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = Mathf.Clamp01(time / duration);
            float alpha = Mathf.Lerp(from, to, t);
            SetAlpha(alpha);
            time += Time.deltaTime;
            yield return null;
        }
        SetAlpha(to); // đảm bảo alpha đúng hẳn
    }

    void SetAlpha(float a)
    {
        if (WaveInformText != null)
        {
            Color color = WaveInformText.color;
            color.a = a;
            WaveInformText.color = color;
        }
    }
}
