using System.Collections;
using Match3.Interface;
using Match3.NonMono;
using Match3.Scriptable;
using UnityEngine;

namespace Match3.Mono
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridData gridData;
        private float dropHeight => gridData.DropHeight;
        private float dropTime => gridData.DropTime;
        private Grid<IGridObject> gameGrid;
        private IGroupSystem groupSystem;
        private IDamageSystem damageSystem = new DamageSystem();
        private ICreateSystem createSystem;
        private IMoveSystem moveSystem;
        private IModifySystem modifySystem;
    
        private void Awake()
        {
            createSystem = new CreateSystem(gridData.MatchObjects,gridData.ObstacleObjects);
            CreateGrid();
            createSystem.SetGrid(gameGrid);
            groupSystem = new GroupSystem(gameGrid);
            moveSystem = new MoveSystem(gameGrid, dropTime);
            modifySystem = new ModifySystem(gameGrid);
            Camera.main.orthographicSize =
                gridData.CellSize * Mathf.Max(gridData.Width, gridData.Height * Camera.main.aspect)*1.1f;
            StartCoroutine(nameof(UpdateGridElements));
        }
    
        private void CreateGrid()
        {
            gameGrid = new Grid<IGridObject>(gridData.Width, gridData.Height, gridData.CellSize, new Vector3(gridData.Width-1,gridData.Height-1,0) * gridData.CellSize * -.5f, CreateFirstGridObjects);
        }

        private IEnumerator UpdateGridElements()
        {
            modifySystem.ReindexGridObjects();
            CreateMissingGridObjects();
            moveSystem.MoveGridObjectsToPositions();
            yield return new WaitForFixedUpdate();
            groupSystem.GroupGridObjects();
            yield return new WaitForFixedUpdate();
            foreach (IGridObject gridObject in gameGrid.GridObjects)
            {
                gridObject?.UpdateSprite();
            }
            if (!groupSystem.HasAnyGroup())
            {
                yield return new WaitForSecondsRealtime(1);
                modifySystem.RearrangeDeadlock();
                StartCoroutine(UpdateGridElements());
            }
        }
    
        private IGridObject CreateFirstGridObjects(Grid<IGridObject> grid, int width, int height)
        {
            IGridObject gridObject = createSystem.CreateRandomGridObject(width, height);
            SetGridObject(grid, gridObject);
            return gridObject;
        }

        private void SetGridObject(Grid<IGridObject> grid, IGridObject gridObject)
        {
            Vector3 startPos = grid.GetWorldPosition(gridObject.WidthIndex, gridObject.HeightIndex);
            startPos.y = dropHeight;
            gridObject.transform.position = startPos;
            if (gridObject.transform.TryGetComponent(out IRenderByGroupSize renderByGroupSize))
            {
                renderByGroupSize.RendererLevelLimits = gridData.RendererLevelLimits;
            }

            if (gridObject.transform.TryGetComponent(out IClickable clickable))
            {
                clickable.OnClicked += OnGridObjectClicked;
            }

            if (gridObject.transform.TryGetComponent(out IDestroyable destroyable))
            {
                destroyable.OnDestroyed += OnGridObjectDestroyed;
            }
        }

        private void OnGridObjectClicked(IGridObject gridObject)
        {
            damageSystem.ApplyMatchDamage(gameGrid,gridObject);
        }

        private void OnGridObjectDestroyed(IGridObject gridObject)
        {
            if (gridObject.transform.TryGetComponent(out IClickable clickable))
            {
                clickable.OnClicked -= OnGridObjectClicked;
            }
        
            if (gridObject.transform.TryGetComponent(out IDestroyable destroyable))
            {
                destroyable.OnDestroyed -= OnGridObjectDestroyed;
            }

            gameGrid.GridObjects[gridObject.WidthIndex, gridObject.HeightIndex] = null;
            StartCoroutine(nameof(UpdateGridElements));
        }

        void CreateMissingGridObjects()
        {
            foreach (var gridObject in createSystem.CreateMissingGridObjects())
            {
                SetGridObject(gameGrid, gridObject);
            }
        }
    }
}