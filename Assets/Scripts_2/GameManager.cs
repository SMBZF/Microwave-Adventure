using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;

    private void Start()
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager δ���ã����� Unity Inspector �и�ֵ��");
            return;
        }

        StartNewGame();
    }

    private void StartNewGame()
    {
        //gridManager.InitializeGrid();
        
    }
}
