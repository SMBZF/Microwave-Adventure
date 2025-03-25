using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutWithSubtitleFade : MonoBehaviour
{
    public CanvasGroup blackoutGroup;     // ��Ļ
    public CanvasGroup subtitleGroup;     // ��Ļ
    public float fadeDuration = 2f;       // ����ʱ�䣨�룩

    [Header("��ѡ����")]
    public bool autoFadeOnStart = false;  // �����Ҫ�ڳ���һ��ʼ�ͽ��룬�ɴ򿪴˿���

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

    // Timeline �е� Signal Emitter ��������
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
