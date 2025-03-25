using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;

public class GridManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public int gridSize = 3;
    public Transform tileParent;
    public float tileSpacing = 1f;
    public Camera mainCamera;
    public Camera endCamera;
    public PlayableDirector endingTimeline;

    private Tile[,] tiles;
    private Vector2Int emptyPosition;
    private Dictionary<int, GameObject> tileDictionary;

    private void Awake()
    {
        InitializeTileDictionary();
        InitializeGrid();
        if (endCamera != null)
            endCamera.enabled = false;
    }

    private void InitializeTileDictionary()
    {
        tileDictionary = new Dictionary<int, GameObject>();
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            tileDictionary[i + 1] = tilePrefabs[i];
        }
    }

    public void InitializeGrid()
    {
        tiles = new Tile[gridSize, gridSize];

        int[,] fixedPositions = new int[,]
        {
            { 1, 0, 5 },
            { 7, 2, 4 },
            { 3, 6, 8 }
        };

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                int val = fixedPositions[gridSize - 1 - y, x]; // 让拼图从左上角开始排
                Vector2Int gridPos = new Vector2Int(x, y);

                Vector3 localOffset = new Vector3(x * tileSpacing, y * tileSpacing, 0);
                Vector3 worldPos = tileParent.TransformPoint(localOffset);

                if (val == 0)
                {
                    emptyPosition = gridPos;
                    tiles[x, y] = null;
                }
                else
                {
                    GameObject prefab = tileDictionary[val];
                    Quaternion tileRotation = tileParent.rotation * Quaternion.Euler(0, -90, 0);
                    GameObject newTile = Instantiate(prefab, worldPos, tileRotation, tileParent);

                    Tile tile = newTile.GetComponent<Tile>();
                    tile.gridPosition = gridPos;
                    tile.tileNumber = val; // ✅ 标记编号用于胜利判断
                    tiles[x, y] = tile;
                }
            }
        }
    }

    public void TryMoveTile(Tile tile)
    {
        Vector2Int tilePos = tile.gridPosition;

        if (Vector2Int.Distance(tilePos, emptyPosition) == 1)
        {
            SwapTiles(tile, emptyPosition);
        }
    }

    private void SwapTiles(Tile tile, Vector2Int emptyPos)
    {
        Vector2Int oldPos = tile.gridPosition;
        tile.UpdatePosition(emptyPos, tileSpacing, tileParent);

        tiles[emptyPos.x, emptyPos.y] = tile;
        tiles[oldPos.x, oldPos.y] = null;
        emptyPosition = oldPos;

        // ✅ 打印当前拼图状态
        PrintCurrentGridState();

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        int[,] winCondition = new int[,]
        {
            { 1, 2, 0 },
            { 3, 4, 5 },
            { 6, 7, 8 }
        };

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                int expected = winCondition[gridSize - 1 - y, x]; // 保持坐标一致性
                if (expected == 0) continue;

                if (tiles[x, y] == null || tiles[x, y].tileNumber != expected)
                {
                    Debug.LogWarning($"❌ 第 ({x},{y}) 格子错误：应为 {expected}，实际为 {(tiles[x, y] == null ? "空" : tiles[x, y].tileNumber.ToString())}");
                    return;
                }
            }
        }

        Debug.Log("🎉 You Win! 拼图完成！");
        // ✅ 切换摄像机
        if (mainCamera != null) mainCamera.enabled = false;
        if (endCamera != null) endCamera.enabled = true;

        // ✅ 你可以这里触发动画或 Timeline 播放
        endingTimeline.Play();
    }

    // ✅ 可视化拼图状态输出
    public void PrintCurrentGridState()
    {
        string output = "当前拼图状态：\n";

        for (int y = gridSize - 1; y >= 0; y--) // 从上到下打印
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (tiles[x, y] == null)
                {
                    output += "[ ]";
                }
                else
                {
                    output += $"[{tiles[x, y].tileNumber}]";
                }
            }
            output += "\n";
        }

        Debug.Log(output);
    }
}
