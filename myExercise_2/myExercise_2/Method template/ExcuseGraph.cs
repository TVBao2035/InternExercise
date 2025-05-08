using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace myExercise_2
{
    abstract  class ExcuseGraph
    {
        public Graph graph;
        public void Process(string path)
        {
            ReadFile(path);
            PrintResult();
        }
        public ExcuseGraph()
        {
            graph = new Graph();
        }

        public virtual void ReadFile(string path) {
            string[] lines = File.ReadAllLines(path);
            int num = 0;
            graph.n = int.Parse(lines[0]);
           
            for(int j=1; j<=graph.n; j++)
            {
                var fields = lines[j].Split(' ');
                graph.arr[num] = new int[graph.n];
                for (int i = 0; i < fields.Length; i++)
                {

                    int x = int.Parse(fields[i]);
                    graph.arr[num][i] = x;
                   
                }
                num++;
            }
        }

        public abstract void Excuse();
        public virtual void PrintResult()
        {
            Console.WriteLine(graph.n);
            for (int i = 0; i < graph.n; i++)
            {
                for (int j = 0; j < graph.n; j++)
                {
                    Console.Write(graph.arr[i][j] + " ");
                }
                Console.WriteLine();
            }

             Excuse();
        }
    }
}
