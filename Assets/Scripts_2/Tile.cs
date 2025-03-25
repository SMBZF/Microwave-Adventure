using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition;
    public int tileNumber; // ����ʤ���ж�
    private GridManager gridManager;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    // ʹ����ײ�������ƶ�
    private void OnTriggerEnter(Collider other)
    {
        if (gridManager != null)
        {
            gridManager.TryMoveTile(this);
        }
    }

    // ���� Tile λ�ã�ʹ���������֧꣬����ת��
    public void UpdatePosition(Vector2Int newGridPos, float spacing, Transform parent)
    {
        gridPosition = newGridPos;
        Vector3 localOffset = new Vector3(newGridPos.x * spacing, newGridPos.y * spacing, 0);
        transform.position = parent.TransformPoint(localOffset);
    }
}
