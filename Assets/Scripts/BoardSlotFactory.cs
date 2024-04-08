using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoardSlotFactory : BoardSlot
{
    public override BoardItemType GetRandomItem()
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
}
