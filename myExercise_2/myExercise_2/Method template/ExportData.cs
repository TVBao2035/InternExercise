using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myExercise_2.Method_template
{
    public abstract class ExportData
    {

        private Dictionary<int, string> _responseMessages = new Dictionary<int, string>()
        {
            {404,"Error" },
            {200, "Success" }
        };
        public void Process(int responseCode, string messageDetails)
        {
            string messageReponse = GetResponseMessage(responseCode);
            Export(messageReponse, messageDetails);
        }

        public abstract void Export(string responseMessage, string messageDetails);
        public string GetResponseMessage(int responseCode)
        {
            return _responseMessages[responseCode];
        }

    }
}
