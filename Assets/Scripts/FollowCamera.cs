using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutWithSubtitleFade : MonoBehaviour
{
    public CanvasGroup blackoutGroup;     // 黑幕
    public CanvasGroup subtitleGroup;     // 字幕
    public float fadeDuration = 2f;       // 渐变时间（秒）

    [Header("可选设置")]
    public bool autoFadeOnStart = false;  // 如果需要在场景一开始就渐入，可打开此开关

    private void Start()
    {
        if (blackoutGroup != null)
            blackoutGroup.alpha = 0f;

        if (subtitleGroup != null)
            subtitleGroup.alpha = 0f;

        if (autoFadeOnStart)
        {
            StartFade();
        }
    }

    // Timeline 中的 Signal Emitter 会调用这个
    public void StartFade()
    {
        StartCoroutine(FadeBoth());
    }

    private IEnumerator FadeBoth()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;

            if (blackoutGroup != null)
                blackoutGroup.alpha = t;

            if (subtitleGroup != null)
                subtitleGroup.alpha = t;

            timer += Time.deltaTime;
            yield return null;
        }

        if (blackoutGroup != null)
            blackoutGroup.alpha = 1f;

        if (subtitleGroup != null)
            subtitleGroup.alpha = 1f;
    }
}
