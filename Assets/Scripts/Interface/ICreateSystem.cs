public interface ICreateSystem
{
    IGridObject CreateRandomGridObject(int width, int height);
    IGridObject CreateRandomMatchObject(int width, int height);
    IGridObject CreateRandomObstacleObject(int width, int height);

}