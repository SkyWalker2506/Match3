using UnityEngine;

public class GridObject : MonoBehaviour, IGridObject
{
    public Grid<IGridObject> Grid { get; set; }
    public int WidthIndex { get; set; }
    public int HeightIndex { get; set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public Sprite[] StateSprites { get; private set; }

    public void SetSprite(int index)
    {
        SpriteRenderer.sprite = StateSprites[index];
    }

}