using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;

    private void Start()
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager 未设置，请在 Unity Inspector 中赋值！");
            return;
        }

        StartNewGame();
    }

    private void StartNewGame()
    {
        //gridManager.InitializeGrid();
        
    }
}
