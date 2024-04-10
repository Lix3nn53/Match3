using System.Collections.Generic;
using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public Board Board;
    public Vector2Int Position;
    public BoardItem CurrentItem = null;
    public BoardItem ItemIncoming = null;

    public virtual List<BoardItemType> DefaultFactoryValues()
    {
        return new(){
            BoardItemType.Type00,
            BoardItemType.Type01,
            BoardItemType.Type02,
            BoardItemType.Type03,
            BoardItemType.Obstacle01
        };
    }

    public virtual void FillRandom(List<BoardItemType> values = null)
    {
        values ??= DefaultFactoryValues();

        int randomIndex = UnityEngine.Random.Range(0, values.Count);
        BoardItemType type = values[randomIndex];

        ReplaceWith(type);
    }

    public void ReplaceWith(BoardItemType type)
    {
        if (ItemIncoming)
        {
            return;
        }

        if (CurrentItem != null)
        {
            CurrentItem.DestroySelf();
        }

        GameObject _slotPrefab = AssetManager.Instance.GetItemTypePrefab(type);

        GameObject boardItemGameObject = Instantiate(_slotPrefab, transform.position, Quaternion.identity, null);

        CurrentItem = boardItemGameObject.GetComponent<BoardItem>();
        CurrentItem.CurrentSlot = this;

        CurrentItem.gameObject.name = "Item#" + Board.ItemCount;
        Board.ItemCount++;
    }
}
