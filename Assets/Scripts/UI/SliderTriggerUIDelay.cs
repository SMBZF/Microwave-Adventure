using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderTriggerUIDelay : MonoBehaviour
{
    public Slider targetSlider; // 目标 Slider
    public GameObject uiPanel; // UI 面板
    public Text uiText; // UI 文字
    public GameObject targetObject; // 目标物体
    public GameObject firstHalo; // 第一个光圈

    public float radioMinValue = 150.0f;
    public float radioMaxValue = 174.0f;
    public float uvMinValue = 75.0f;
    public float uvMaxValue = 100.0f;
    public float xrayMinValue = 81.8f; // X 射线模式的最小值
    public float xrayMaxValue = 100.0f; // X 射线模式的最大值
    public float delayTime = 0.5f;

    public Transform playerTransform; // 玩家位置
    public float xrayDetectionRadius = 5.0f; // X 射线销毁检测半径
    public string xrayTargetTag = "XRayTarget"; // 要销毁的物体的标签

    private Coroutine checkCoroutine;
    private bool isUIVisible = false;
    private bool isObjectVisible = false;
    private bool isFirstHaloVisible = false;
    private bool hasTriggeredUVEffect = false; // 确保 UV 交互只触发一次
    private bool hasExitedUVRange = true; // 确保离开 UV 范围只触发一次

    private void Start()
    {
        if (targetSlider != null && uiPanel != null && targetObject != null && firstHalo != null)
        {
            uiPanel.SetActive(false);
            targetObject.SetActive(false);
            firstHalo.SetActive(false);
            targetSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("请在 Inspector 里绑定 Slider、UI 面板、目标物体 和 第一个光圈");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (ElectromagneticMode.isRadioModeUnlocked && value >= radioMinValue && value <= radioMaxValue)
        {
            if (checkCoroutine == null)
            {
                checkCoroutine = StartCoroutine(WaitToShowUIAndObject("Radio connection successful"));
            }
        }
        else if (ElectromagneticMode.isUVModeUnlocked && value >= uvMinValue && value <= uvMaxValue)
        {
            if (checkCoroutine == null)
            {
                checkCoroutine = StartCoroutine(WaitToShowUIAndObject("UV connection successful"));
            }

            // 进入 UV 频率区间，只触发一次
            if (!hasTriggeredUVEffect)
            {
                Interactor interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.ActivateUVEffect();
                    hasTriggeredUVEffect = true;
                    hasExitedUVRange = false;
                }
            }
        }
        else if (ElectromagneticMode.isXRayModeUnlocked && value >= xrayMinValue && value <= xrayMaxValue)
        {
            if (checkCoroutine == null)
            {
                checkCoroutine = StartCoroutine(WaitToShowUIAndObject("X-ray connection successful"));
            }

            DetectAndDestroyXRayTargets(); // 新增：检测并销毁 X 射线目标
        }
        else
        {
            if (checkCoroutine != null)
            {
                StopCoroutine(checkCoroutine);
                checkCoroutine = null;
            }
            HideUIAndObject();

            // 离开 UV 频率区间，只触发一次
            if (!hasExitedUVRange)
            {
                Interactor interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.DeactivateUVEffect();
                    hasExitedUVRange = true;
                    hasTriggeredUVEffect = false;
                }
            }
        }
    }

    private IEnumerator WaitToShowUIAndObject(string message)
    {
        yield return new WaitForSeconds(delayTime);

        if (!isUIVisible && uiPanel != null && uiText != null)
        {
            uiPanel.SetActive(true);
            uiText.text = message;
            isUIVisible = true;
            Debug.Log($"UI 显示: {message}");
        }

        if (!isObjectVisible && targetObject != null)
        {
            targetObject.SetActive(true);
            isObjectVisible = true;
            Debug.Log("目标物体显示");
        }

        if (!isFirstHaloVisible && firstHalo != null)
        {
            firstHalo.SetActive(true);
            isFirstHaloVisible = true;

            HaloSequence haloSequence = firstHalo.GetComponentInParent<HaloSequence>();
            if (haloSequence != null)
            {
                haloSequence.ActivateFirstHalo();
            }

            Debug.Log("First halo activated");
        }
    }

    private void HideUIAndObject()
    {
        if (uiPanel != null && isUIVisible)
        {
            uiPanel.SetActive(false);
            isUIVisible = false;
            Debug.Log("UI 隐藏");
        }

        if (targetObject != null && isObjectVisible)
        {
            targetObject.SetActive(false);
            isObjectVisible = false;
            Debug.Log("目标物体隐藏");
        }

        if (firstHalo != null && isFirstHaloVisible)
        {
            firstHalo.SetActive(false);
            isFirstHaloVisible = false;
            Debug.Log("第一个光圈隐藏");
        }
    }

    private void OnDestroy()
    {
        if (targetSlider != null)
        {
            targetSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    private void DetectAndDestroyXRayTargets()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("playerTransform 未绑定，无法执行 X 射线检测");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(playerTransform.position, xrayDetectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(xrayTargetTag))
            {
                Debug.Log("X 射线检测到目标并销毁: " + hit.name);
                Destroy(hit.gameObject);
            }
        }
    }
}
