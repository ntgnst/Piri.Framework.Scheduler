namespace Piri.Framework.Scheduler.Quartz.Interface.Result
{
    public class Result<T> : IResult<T>
    {
        public bool IsSuccess { get; set; }
        public ResultTypeEnum ResultType { get; set; }
        public string Html { get; set; }
        public string Message { get; set; }
        public bool IsLastPackage { get; set; }
        public int DataCount { get; set; }
        public T Data { get; set; }

        public Result()
        {
            Data = default(T);
        }

        public Result(T Data)
            : this(true, ResultTypeEnum.None, string.Empty, Data, false, string.Empty)
        {
        }
        public Result(T Data, int DataCount)
           : this(true, ResultTypeEnum.None, string.Empty, Data, false, string.Empty, DataCount)
        {
        }
        public Result(ResultTypeEnum ResultTypeEnum, T Data, string Message)
          : this(true, ResultTypeEnum, string.Empty, Data, false, Message)
        {
        }
        public Result(ResultTypeEnum ResultTypeEnum, T Data, string Message, int DataCount)
          : this(true, ResultTypeEnum, string.Empty, Data, false, Message, DataCount)
        {
        }

        public Result(bool IsSuccess, string Message)
          : this(IsSuccess, ResultTypeEnum.None, string.Empty, default(T), false, Message)
        {
        }
        public Result(bool IsSuccess, ResultTypeEnum ResultType, string Message)
            : this(IsSuccess, ResultType, string.Empty, default(T), false, Message)
        {
        }

        public Result(bool IsSuccess, ResultTypeEnum ResultType, string Html, T Data, bool IsLastPackage, string Message, int DataCount = 0)
        {
            this.IsSuccess = IsSuccess;
            this.ResultType = ResultType;
            this.Message = Message;
            this.Html = Html;
            this.Data = Data;
            this.IsLastPackage = IsLastPackage;
            this.DataCount = DataCount;
        }

        public void Import(IResult result)
        {
            IsSuccess = result.IsSuccess;
            ResultType = result.ResultType;
            Message = result.Message;
        }
    }
}
