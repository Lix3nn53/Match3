using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardItemType
{
    Type00,
    Type01,
    Type02,
    Type03,
    Obstacle01
}
public static class BoardItemTypeExtensions
{
    public static bool CanMatch(this BoardItemType type)
    {
        return type switch
        {
            BoardItemType.Type00 => true,
            BoardItemType.Type01 => true,
            BoardItemType.Type02 => true,
            BoardItemType.Type03 => true,
            _ => false,
        };
    }

}