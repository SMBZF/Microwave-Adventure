using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float radius = 0f; // ��ǰ��Χ

    private float uvMaxRadius = 8f;     // UV ���Χ
    private float xrayMaxRadius = 6f;   // X �������Χ
    private float duration = 2f;        // ����ʱ��

    private Coroutine transitionCoroutine;

    void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
    }

    public void ActivateUVEffect()
    {
        StartTransition(radius, uvMaxRadius);
    }

    public void DeactivateUVEffect()
    {
        StartTransition(radius, 0);
    }

    public void ActivateXRayEffect()
    {
        StartTransition(radius, xrayMaxRadius);
    }

    public void DeactivateXRayEffect()
    {
        StartTransition(radius, 0);
    }

    private void StartTransition(float from, float to)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(SmoothTransition(from, to));
    }

    private IEnumerator SmoothTransition(float start, float target)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            radius = Mathf.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        radius = target;
    }
}
