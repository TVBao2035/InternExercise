namespace myExercise_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetLevelMax levelMax = new GetLevelMax();
            levelMax.Process("graph.txt");
            GetLevelMin levelMin = new GetLevelMin();
            levelMin.Process("graph.txt");
        }
    }
}
