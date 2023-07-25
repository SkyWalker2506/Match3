using UnityEngine;

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
    
    public GameObject sGetRandomGridObject()
    {
       return MatchObjects[Random.Range(0, MatchObjects.Length)];
    }
}