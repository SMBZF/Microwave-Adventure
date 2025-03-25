using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineSceneLoader : MonoBehaviour
{
    private string nextSceneName = "sence final"; // Ŀ�곡����

    public void LoadNextScene()
    {
        if (SceneExists(nextSceneName))
        {
            Debug.Log($"�л�������: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError($"���� '{nextSceneName}' δ�� Build Settings ���ҵ�������ƴд����ӵ� Build Settings");
        }
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}
