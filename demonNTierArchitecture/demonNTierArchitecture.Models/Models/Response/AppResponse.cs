using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demonNTierArchitecture.Models.Models.Response
{
    public class AppResponse<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public int ? CodeStatus { get; set; }


        public AppResponse<T> SendResponse(int CodeStatus,  string Message)
        {
            this.CodeStatus = CodeStatus;
            this.Message = Message;
            return this;
        }

        public AppResponse<T> SendResponse(int CodeStatus, string Message, T Data)
        {
            this.CodeStatus = CodeStatus;
            this.Message = Message;
            this.Data = Data;
            return this;
        }

    }
}
