using System;
using System.Collections.Generic;
using Match3.Interface;
using UnityEngine;

namespace Match3.Mono
{
    public class MatchObject : GridObject, IGroupable, IDamagable, IDestroyable, IMoveDown, IRenderByGroupSize, IClickable
    {
        [field: SerializeField] public int GroupIndex { get; private set; }
        public int GroupElementCount => GroupElements.Count;
        [field: SerializeField] public HashSet<IGroupable> GroupElements { get; set; }
        public int[] RendererLevelLimits { get; set; }
        public Action<IGridObject> OnDamaged { get; }
        public Action<IGridObject> OnDestroyed { get; set; }
        public Action OnMoved { get; }
        public Action<IGridObject> OnClicked { get; set; }


        private void Awake()
        {
            ResetGroupElements();
        }
    
        void OnMouseDown () 
        {
            OnClicked?.Invoke(this);
        }


        public void AddGroupElement(IGroupable groupable)
        {
            GroupElements.Add(groupable.transform.GetComponent<IGroupable>());
        }
    
        public void ResetGroupElements()
        {
            GroupElements = new HashSet<IGroupable>();
        }

        public void AddGroupElements(HashSet<IGroupable> groupables)
        {
            foreach (IGroupable groupable in groupables)
            {
                AddGroupElement(groupable);
            }
        }

        public void Damage(int damage)
        {
            OnDamaged?.Invoke(this);
            Destroy();
        }

        public void Destroy()
        {
            OnDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

        public void MoveDown(IGridObject gridObject)
        {
            OnMoved?.Invoke();
        }

        public override void UpdateSprite()
        {
            for (int i = 0; i < RendererLevelLimits.Length; i++)
            {
                if (RendererLevelLimits[i] > GroupElementCount)
                {
                    SetSprite(i);
                    return;
                }
            }
            SetSprite(RendererLevelLimits.Length);
        }

    }
}