using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myExercise_2
{
    internal class GetLevelMin : ExcuseGraph
    {
        public override void Excuse()
        {
            int level = 0;
            int levelMin = -1;
            for (int i = 0; i < graph.n; i++)
            {
                level = 0;
                for (int j = 0; j < graph.n; j++)
                {
                    if (graph.arr[i][j] != 0)
                    {
                        level++;
                    }
                }
                if (levelMin == -1 || level < levelMin)
                {
                    levelMin = level;
                }
            }


            Console.WriteLine("Level Min: " + levelMin);
        }
    }
}
