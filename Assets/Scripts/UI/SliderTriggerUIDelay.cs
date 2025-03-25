using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTriggerUIDelay : MonoBehaviour
{
    public Slider targetSlider;
    public GameObject uiPanel;
    public Text uiText;
    public GameObject targetObject;
    public GameObject firstHalo;

    public float radioMinValue = 150.0f;
    public float radioMaxValue = 174.0f;
    public float uvMinValue = 75.0f;
    public float uvMaxValue = 100.0f;
    public float xrayMinValue = 81.8f;
    public float xrayMaxValue = 100.0f;
    public float delayTime = 0.5f;

    public string xrayTargetTag = "XRayTarget";
    public float xrayEffectHoldTime = 10f;

    public List<GameObject> radioObjectsToActivate;

    private Coroutine checkCoroutine;
    private bool isUIVisible = false;
    private bool isObjectVisible = false;
    private bool isFirstHaloVisible = false;
    private bool hasTriggeredUVEffect = false;
    private bool hasTriggeredXRayEffect = false;

    private float xrayTimer = 0f;
    private bool xrayEffectActive = false;
    private List<Collider> xrayTargetColliders = new List<Collider>();

    private bool radioActive = false;

    private bool uvEffectLocked = false;
    private bool xrayEffectLocked = false; // ✅ 新增：X射线锁定状态

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

    private void Update()
    {
        if (xrayEffectActive)
        {
            // 如果已锁定，则不再自动关闭
            if (xrayEffectLocked)
            {
                return;
            }

            xrayTimer -= Time.deltaTime;
            if (xrayTimer <= 0f)
            {
                foreach (Collider col in xrayTargetColliders)
                {
                    if (col != null)
                    {
                        col.isTrigger = false;
                    }
                }

                var interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.DeactivateXRayEffect();
                }

                xrayTargetColliders.Clear();
                xrayEffectActive = false;
                hasTriggeredXRayEffect = false;
            }
        }
    }


    private void OnSliderValueChanged(float value)
    {
        if (ElectromagneticMode.isRadioModeUnlocked && value >= radioMinValue && value <= radioMaxValue)
        {
            if (checkCoroutine == null)
                checkCoroutine = StartCoroutine(WaitToShowUIAndObject("Radio connection successful"));

            Debug.Log("RadioObjectsToActivate 列表总数: " + radioObjectsToActivate.Count);
            for (int i = 0; i < radioObjectsToActivate.Count; i++)
            {
                GameObject obj = radioObjectsToActivate[i];
                if (obj == null)
                {
                    Debug.LogWarning($"Radio 列表中第 {i} 项是空的！");
                    continue;
                }

                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                    Debug.Log($"Radio 激活物体: {obj.name}（索引 {i}）");
                }
                else
                {
                    Debug.Log($"Radio 跳过已激活物体: {obj.name}（索引 {i}）");
                }
            }
            radioActive = true;
        }
        else if (ElectromagneticMode.isUVModeUnlocked && value >= uvMinValue && value <= uvMaxValue)
        {
            if (checkCoroutine == null)
                checkCoroutine = StartCoroutine(WaitToShowUIAndObject("UV connection successful"));

            if (!hasTriggeredUVEffect && !uvEffectLocked)
            {
                var interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.ActivateUVEffect();
                    hasTriggeredUVEffect = true;
                }
            }
        }
        else if (ElectromagneticMode.isXRayModeUnlocked && value >= xrayMinValue && value <= xrayMaxValue)
        {
            if (checkCoroutine == null)
                checkCoroutine = StartCoroutine(WaitToShowUIAndObject("X-ray connection successful"));

            if (!hasTriggeredXRayEffect)
            {
                var interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.ActivateXRayEffect();
                }

                GameObject[] targets = GameObject.FindGameObjectsWithTag(xrayTargetTag);
                xrayTargetColliders.Clear();
                foreach (GameObject obj in targets)
                {
                    Collider col = obj.GetComponent<Collider>();
                    if (col != null)
                    {
                        col.isTrigger = true;
                        xrayTargetColliders.Add(col);
                    }
                }

                xrayTimer = xrayEffectHoldTime;
                xrayEffectActive = true;
                hasTriggeredXRayEffect = true;
            }
        }
        else
        {
            if (checkCoroutine != null)
            {
                StopCoroutine(checkCoroutine);
                checkCoroutine = null;
            }
            HideUIAndObject();

            if (hasTriggeredUVEffect)
            {
                var interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.DeactivateUVEffect();
                }
                hasTriggeredUVEffect = false;
            }

            if (hasTriggeredXRayEffect)
            {
                var interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.DeactivateXRayEffect();
                }

                foreach (Collider col in xrayTargetColliders)
                {
                    if (col != null)
                    {
                        col.isTrigger = false;
                    }
                }

                xrayTargetColliders.Clear();
                hasTriggeredXRayEffect = false;
                xrayEffectActive = false;
            }

            if (radioActive)
            {
                for (int i = 0; i < radioObjectsToActivate.Count; i++)
                {
                    GameObject obj = radioObjectsToActivate[i];
                    if (obj != null && obj.activeSelf)
                    {
                        obj.SetActive(false);
                        Debug.Log($"Radio 隐藏物体: {obj.name}（索引 {i}）");
                    }
                }
                radioActive = false;
            }
        }

    }

    public void LockUVEffect()
    {
        uvEffectLocked = true;

        Interactor interactor = FindObjectOfType<Interactor>();
        if (interactor != null)
        {
            interactor.DeactivateUVEffect();
            Debug.Log("紫外线已被锁定并立即关闭视觉效果");
        }
        else
        {
            Debug.LogWarning("未找到 Interactor，无法关闭紫外线效果");
        }
    }

    public void LockXRayEffect() 
    {
        xrayEffectLocked = true;
        Debug.Log("X 射线效果已被锁定，不再关闭 Trigger 或收回范围");
    }

    private IEnumerator WaitToShowUIAndObject(string message)
    {
        yield return new WaitForSeconds(delayTime);

        if (!isUIVisible && uiPanel != null && uiText != null)
        {
            uiPanel.SetActive(true);
            uiText.text = message;
            isUIVisible = true;
        }

        if (!isObjectVisible && targetObject != null)
        {
            targetObject.SetActive(true);
            isObjectVisible = true;
        }

        if (!isFirstHaloVisible && firstHalo != null)
        {
            firstHalo.SetActive(true);
            isFirstHaloVisible = true;

            HaloSequence haloSequence = firstHalo.GetComponentInParent<HaloSequence>();
            if (haloSequence != null)
                haloSequence.ActivateFirstHalo();
        }
    }

    private void HideUIAndObject()
    {
        if (uiPanel != null && isUIVisible)
        {
            uiPanel.SetActive(false);
            isUIVisible = false;
        }

        if (targetObject != null && isObjectVisible)
        {
            targetObject.SetActive(false);
            isObjectVisible = false;
        }

        if (firstHalo != null && isFirstHaloVisible)
        {
            firstHalo.SetActive(false);
            isFirstHaloVisible = false;
        }
    }

    private void OnDestroy()
    {
        if (targetSlider != null)
            targetSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
