namespace TarkLogs.Mongo
{
    internal class LogApiDocument
    {
        public string ServiceDescription { get; set; }

        public string FromHost { get; set; }

        public string ToEndPoint { get; set; }

        public string OperationDescription { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public int Status { get; set; }

        public long ElapsedMilliseconds { get; set; }
    }
}
