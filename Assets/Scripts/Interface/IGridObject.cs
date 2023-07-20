using UnityEngine;
using UnityEngine.UI;

public interface IGridObject
{
    int WidthIndex { get; set; }
    int HeightIndex { get; set; }
    Image ObjectImage { get; }
    Sprite[] StateSprites { get; }
    void SetSprite(int index);
}