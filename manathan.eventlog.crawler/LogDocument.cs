namespace manathan.eventlog.crawler
{
    using System.Diagnostics;
    using find.Crawler;

    public class LogDocument : BaseDocument
    {
        readonly string full;

        public LogDocument(EventLogEntry entry)
        {
            Title = entry.Source;
            Content = entry.Message;
            Date = entry.TimeWritten;
            
            full =
                string.Format("[Index]\t{0}\n[EventID]\t{1}\n[TimeWritten]\t{2}\n[MachineName]\t{3}\n[Source]\t{4}\n[UserName]\t{5}\n[Message]\t{6}", entry.Index, entry.InstanceId, entry.TimeWritten, entry.MachineName, entry.Source, entry.UserName, entry.Message);
        }

        public override string ToSearchable()
        {
            return full;
        }
    }
}
