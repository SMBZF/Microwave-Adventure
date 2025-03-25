using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private GameObject activeUI = null; // 记录当前打开的 UI

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowUI(GameObject uiPanel)
    {
        if (activeUI != null && activeUI != uiPanel)
        {
            activeUI.SetActive(false); // 🎯 关闭当前 UI
        }

        uiPanel.SetActive(true);
        activeUI = uiPanel; // 🎯 记录新打开的 UI
    }

    public void HideUI(GameObject uiPanel)
    {
        if (activeUI == uiPanel)
        {
            uiPanel.SetActive(false);
            activeUI = null;
        }
    }

    public void HideAllUI()
    {
        if (activeUI != null)
        {
            activeUI.SetActive(false);
            activeUI = null;
        }
    }
}
