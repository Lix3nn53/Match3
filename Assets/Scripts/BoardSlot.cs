using System;
using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public Vector2Int Position;
    public BoardItem CurrentItem = null;
    public BoardItem ItemIncoming = null;
    public virtual void FillRandom()
    {
        BoardItemType Type = GetRandomItem();

        GameObject _slotPrefab = AssetManager.Instance.GetItemTypePrefab(Type);

        GameObject boardItemGameObject = Instantiate(_slotPrefab, transform.position, Quaternion.identity, null);

        CurrentItem = boardItemGameObject.GetComponent<BoardItem>();
        CurrentItem.CurrentSlot = this;

        CurrentItem.gameObject.name = "Item#" + Board.Instance.ItemCount;
        Board.Instance.ItemCount++;
    }

    // Function to get a random enum value
    public virtual BoardItemType GetRandomItem()
    {
        // Get all values of the enum
        Array enumValues = Enum.GetValues(typeof(BoardItemType));

        // Generate a random index
        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);

        // Get the enum value at the random index
        BoardItemType randomEnumValue = (BoardItemType)enumValues.GetValue(randomIndex);

        return randomEnumValue;
    }
}
