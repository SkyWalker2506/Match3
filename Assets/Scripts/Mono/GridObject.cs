using System;
using Match3.Interface;
using Match3.NonMono;
using UnityEngine;

namespace Match3.Mono
{
    public abstract class GridObject : MonoBehaviour, IGridObject
    {
        public Grid<IGridObject> Grid { get; set; }
        public int WidthIndex { get; set; }
        public int HeightIndex { get; set; }
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public Sprite[] StateSprites { get; private set; }

        public abstract void UpdateSprite();

        protected void SetSprite(int index)
        {
            if (SpriteRenderer)
            {
                index = Math.Clamp(index, 0, StateSprites.Length-1);
                SpriteRenderer.sprite = StateSprites[index];
            }
        }

    }
}