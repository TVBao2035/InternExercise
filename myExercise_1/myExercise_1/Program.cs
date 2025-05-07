using System.Threading.Tasks;

namespace myExercise_1
{
    internal class Program
    {

        public static async Task SendMessage(string yourMessage, string yourName, int time)
        {
            Console.WriteLine($"Welcome to chat room:  {yourName}");
            await Task.Delay(time);
            Console.WriteLine($"{yourName}: {yourMessage}");
        }

        static async Task Main(string[] args)
        {

             await SendMessage("how are you today?", "BaoBao", 1000);
             await SendMessage("I am fine", "Truong", 900);


            //SendMessage("how are you today?", "BaoBao", 1000);
            //SendMessage("I am fine", "Truong", 1000);
            Console.ReadKey();
        }
    }
}
