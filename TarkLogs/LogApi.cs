using System.Diagnostics;
using TarkLogs.Constants;
using TarkLogs.Mongo;

namespace TarkLogs
{
    public class LogApi
    {
        public LogApi()
        {
            Watch = new Stopwatch();
        }

        public string ServiceDescription { get; set; }

        public string FromHost { get; set; }

        public string ToEndPoint { get; set; }

        public string OperationDescription { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public int Status { get; set; }

        public Stopwatch Watch { get; set; }

        public void Persist()
        {
            if (Watch.IsRunning)
                Watch.Stop();

            var mongo = new MongoAdapter();

            var document = new LogApiDocument
            {
                ServiceDescription = ServiceDescription,
                FromHost = FromHost,
                ToEndPoint = ToEndPoint,
                OperationDescription = OperationDescription,
                Request = Request,
                Response = Response,
                Status = Status,
                ElapsedMilliseconds = Watch.ElapsedMilliseconds
            };

            mongo.Persist(DatabaseConstants.DATABASE_NAME, DatabaseConstants.COLLECTION_API, document);
        }
    }
}
