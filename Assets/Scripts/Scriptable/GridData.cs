using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class GridData : ScriptableObject
{
    [Header("Grid Settings")]
    [Range(2,12)]
    public int Width;
    [Range(2,12)]
    public int Height;
    [Min(.25f)]
    public float CellSize;
    public GameObject[] MatchObjects;
    public GameObject[] ObstacleObjects;
    [Header("Group Renderer Settings")] 
    public int[] RendererLevelLimits;
    [Header("GridObject Move Settings")] 
    public float DropTime = 1;

    public float DropHeight = 20;


}