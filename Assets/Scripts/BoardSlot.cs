using System;
using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public Vector2Int Position;

    public void FillRandom(bool withObstacle)
    {
        BoardItemType Type = withObstacle ? GetRandomItem() : GetRandomItemGenericOnly();

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

    // Function to get a random enum value
    private BoardItemType GetRandomItemGenericOnly()
    {
        // Get all values of the enum
        BoardItemType[] enumValues = {
            BoardItemType.Type00,
            BoardItemType.Type01,
            BoardItemType.Type02,
            BoardItemType.Type03,
        };

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
