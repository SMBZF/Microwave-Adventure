using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition;
    public int tileNumber; // 用于胜利判断
    private GridManager gridManager;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    // 使用碰撞器触发移动
    private void OnTriggerEnter(Collider other)
    {
        if (gridManager != null)
        {
            gridManager.TryMoveTile(this);
        }
    }

    // 更新 Tile 位置（使用世界坐标，支持旋转）
    public void UpdatePosition(Vector2Int newGridPos, float spacing, Transform parent)
    {
        gridPosition = newGridPos;
        Vector3 localOffset = new Vector3(newGridPos.x * spacing, newGridPos.y * spacing, 0);
        transform.position = parent.TransformPoint(localOffset);
    }
}
