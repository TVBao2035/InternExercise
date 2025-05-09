using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myExercise_2.Method_template
{
    internal class ExportOnConsole : ExportData
    {
        public override void Export(string responseMessage, string messageDetails)
        {
           Console.WriteLine($"Response Message On Status Code: {responseMessage}\nMessage Details: {messageDetails}");
        }
    }
}
