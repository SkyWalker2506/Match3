using Match3.NonMono;
using UnityEngine;

namespace Match3.Interface
{
    public interface IGridObject
    {
        Transform transform { get; }
        Grid<IGridObject> Grid{ get; set; }
        int WidthIndex { get; set; }
        int HeightIndex { get; set; }
        SpriteRenderer SpriteRenderer { get; }
        Sprite[] StateSprites { get; }
        void UpdateSprite();
    }
}