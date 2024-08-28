namespace TallentexResult.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string? ErrorMessage { get; set; }
        public string? Source { get; set; }
        public string? StackTrace { get; set; }
        public string? ErrorPath { get; set; }
        public string? InnerException { get; set; }

        public override string ToString()
        {
            return String.Format(Environment.NewLine +
                                "Request ID: {1}{0}" +
                                "Error Message: {2}{0}" +
                                "Source: {3}{0}" +
                                "ErrorPath: {4}{0}" +
                                "InnerException: {5}{0}" +
                                "StackTrace: {6}",
                                Environment.NewLine + Environment.NewLine,    // 0
                                RequestId,    // 1
                                ErrorMessage, // 2
                                Source,   // 3
                                ErrorPath,    // 4
                                InnerException,   // 5
                                StackTrace    // 6
                            );
        }
    }
}
