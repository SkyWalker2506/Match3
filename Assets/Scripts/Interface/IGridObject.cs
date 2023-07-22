using UnityEngine;

public interface IGridObject
{
    Grid<IGridObject> Grid{ get; set; }
    int WidthIndex { get; set; }
    int HeightIndex { get; set; }
    SpriteRenderer SpriteRenderer { get; }
    Sprite[] StateSprites { get; }
    void SetSprite(int index);
}