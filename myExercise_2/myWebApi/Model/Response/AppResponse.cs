namespace myWebApi.Model.Response
{
    public class AppResponse<T>
    {
            public T Data { get; set; }
            public string Message { get;set ; }
            public int Code { get ; set; }
        public AppResponse<T> Send(int Colde, string? message, T data) {
            Data = data;
            Message = message;  
            this.Code = Code;
            return this;
        }
        public AppResponse<T> Send(int Colde, string? message)
        {
            Message = message;
            this.Code = Code;
            return this;
        }

    }
}
