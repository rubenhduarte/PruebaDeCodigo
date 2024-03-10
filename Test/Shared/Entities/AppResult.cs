namespace Test.Shared.Entities
{
    public enum AppResultStatus
    {
        Ok,
        Failed,
        InternalError
    }
    public class AppResult
    {
        public AppResult()
        {
            Message = string.Empty;
            Result = AppResultStatus.Failed;
        }

        public AppResult(string message, AppResultStatus result)
        {
            Message = message;
            Result = result;
        }

        public static AppResult Success = new AppResult("OK", AppResultStatus.Ok);
        public static AppResult Failed = new AppResult("Error", AppResultStatus.Failed);
        public string Message { get; set; }
        public AppResultStatus Result { get; set; }

        public bool IsOk
        {
            get
            {
                return Result == AppResultStatus.Ok;
            }
        }
    }
}
