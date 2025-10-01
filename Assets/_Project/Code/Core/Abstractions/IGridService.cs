using System.Collections.Generic;

namespace PlanA.Core
{
    public interface IGridService
    {
        int Rows { get; }
        int Cols { get; }
        int[,] Data { get; }         
        void Init(int rows, int cols, int colorCount, int? seed = null);
        List<(int r,int c)> GetCluster(int r, int c);  
        void Remove(List<(int r,int c)> cells);         
        void Refill();  
    }
}