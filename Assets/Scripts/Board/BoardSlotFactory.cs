using System.Collections.Generic;

public class BoardSlotFactory : BoardSlot
{
    public int CountForDelay;
    public override void FillRandom(List<BoardItemType> values = null)
    {
        base.FillRandom(values);
        CurrentItem.gameObject.SetActive(false);
    }
    public override List<BoardItemType> DefaultFactoryValues()
    {
        return new(){
            BoardItemType.Type00,
            BoardItemType.Type01,
            BoardItemType.Type02,
            BoardItemType.Type03,
        };
    }
}
