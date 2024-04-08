using System;
using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public Vector2Int Position;

    public void FillRandom()
    {
        BoardItemType Type = GetRandomItem();

        GameObject _slotPrefab = AssetManager.Instance.GetItemTypePrefab(Type);

        GameObject boardItemGameObject = Instantiate(_slotPrefab, transform.position, Quaternion.identity, transform);

        BoardItem boardItem = boardItemGameObject.GetComponent<BoardItem>();
    }

    // Function to get a random enum value
    private BoardItemType GetRandomItem()
    {
        // Get all values of the enum
        Array enumValues = Enum.GetValues(typeof(BoardItemType));

        // Generate a random index
        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);

        // Get the enum value at the random index
        BoardItemType randomEnumValue = (BoardItemType)enumValues.GetValue(randomIndex);

        return randomEnumValue;
    }

    public BoardItem GetCurrentItem()
    {
        if (transform.childCount == 0)
        {
            return null;
        }

        return transform.GetChild(0).GetComponent<BoardItem>();
    }
}
