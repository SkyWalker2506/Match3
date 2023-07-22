using UnityEngine;

[CreateAssetMenu]
public class GridData : ScriptableObject
{
    [Range(2,12)]
    public int Width;
    [Range(2,12)]
    public int Height;
    [Min(.25f)]
    public float CellSize;
    public GameObject[] GridObjects;

    public GameObject GetRandomGridObject()
    {
       return GridObjects[Random.Range(0, GridObjects.Length)];
    }
}