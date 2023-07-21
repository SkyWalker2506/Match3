using UnityEngine;

[CreateAssetMenu]
public class GridData : ScriptableObject
{
    [Range(2,12)]
    public int Width;
    [Range(2,12)]
    public int Height;
    [Min(25)]
    public float CellSize;
}