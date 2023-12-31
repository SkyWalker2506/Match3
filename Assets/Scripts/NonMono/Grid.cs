﻿using System;
using UnityEngine;

namespace Match3.NonMono
{
    public class Grid<T> {

        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
        public T[,] GridObjects;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<T>, int, int, T> createGridObject) 
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            GridObjects = new T[width, height];

            for (int x = 0; x < GridObjects.GetLength(0); x++) 
            {
                for (int y = 0; y < GridObjects.GetLength(1); y++) 
                {
                    GridObjects[x, y] = createGridObject(this, x, y);
                }
            }
        }

        public int GetWidth() {
            return width;
        }

        public int GetHeight() {
            return height;
        }

        public float GetCellSize() {
            return cellSize;
        }

        public Vector3 GetWorldPosition(int x, int y) {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y) {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetGridObject(int x, int y, T value) {
            if (x >= 0 && y >= 0 && x < width && y < height) {
                GridObjects[x, y] = value;
            }
        }

        public void SetGridObject(Vector3 worldPosition, T value) {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public T GetGridObject(int x, int y) {
            if (x >= 0 && y >= 0 && x < width && y < height) {
                return GridObjects[x, y];
            } else {
                return default(T);
            }
        }

        public T GetGridObject(Vector3 worldPosition) {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }

    }
}
