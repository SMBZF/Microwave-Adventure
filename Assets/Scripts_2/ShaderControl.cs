using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderControl : MonoBehaviour
{
    public Texture2D[] alphaTextures; // 9��ͼƬ
    private int currentIndex = 0;
    private Renderer rend;
    private Material mat;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            mat = rend.material;
        }
        else
        {
            Debug.LogError("No Renderer found on object: " + gameObject.name);
        }

        UpdateAlphaTexture();
    }

    // ��������ᱻ�����ű������������л�
    public void NextAlphaTexture()
    {
        if (alphaTextures == null || alphaTextures.Length == 0)
        {
            Debug.LogError("Alpha textures array is empty!");
            return;
        }

        // �л�����
        currentIndex = (currentIndex + 1) % alphaTextures.Length;

        // ������ͼ
        UpdateAlphaTexture();
    }

    private void UpdateAlphaTexture()
    {
        if (mat != null)
        {
            mat.SetTexture("_Alpha", alphaTextures[currentIndex]);
        }
    }
}

