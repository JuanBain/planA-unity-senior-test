using System;
using System.Collections.Generic;
using PlanA.Core;

namespace PLanA.Code.Grid
{
    public class GridService:IGridService
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public int[,] Data { get; private set; }

        private int _colorCount;
        private Random _rng;
        
        public void Init(int rows, int cols, int colorCount, int? seed = null)
        {
            Rows = rows; Cols = cols; _colorCount = colorCount;
            Data = new int[Rows, Cols];
            _rng = seed.HasValue ? new Random(seed.Value) : new Random();
            for (int r=0;r<Rows;r++)
            for (int c=0;c<Cols;c++)
                Data[r,c] = _rng.Next(0, _colorCount);
        }

        public List<(int r, int c)> GetCluster(int r, int c)
        {
            var result = new List<(int,int)>();
            if (!Inside(r,c)) return result;
            int target = Data[r,c];
            if (target < 0) return result;

            var q = new Queue<(int,int)>();
            var vis = new bool[Rows, Cols];
            q.Enqueue((r,c)); vis[r,c]=true;

            int[] dr = {-1,1,0,0}; int[] dc = {0,0,-1,1};
            while (q.Count>0)
            {
                var (rr,cc) = q.Dequeue();
                if (Data[rr,cc]==target) result.Add((rr,cc));
                for (int k=0;k<4;k++)
                {
                    int nr=rr+dr[k], nc=cc+dc[k];
                    if (Inside(nr,nc) && !vis[nr,nc] && Data[nr,nc]==target)
                    { vis[nr,nc]=true; q.Enqueue((nr,nc)); }
                }
            }
            return result;
        }

        public void Remove(List<(int r, int c)> cells)
        {
            foreach (var (r,c) in cells) Data[r,c] = -1;
        }

        public void Refill()
        {
            for (int c=0;c<Cols;c++)
            {
                int write = Rows-1;
                for (int r=Rows-1;r>=0;r--)
                {
                    if (Data[r,c] >= 0)
                    {
                        Data[write,c] = Data[r,c];
                        if (write!=r) Data[r,c] = -1;
                        write--;
                    }
                }
                
                for (int r=write; r>=0; r--)
                    Data[r,c] = _rng.Next(0,_colorCount);
            }
        }

        private bool Inside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Cols;
        }
    }
}