namespace Entity
{
    public class SysLog : BaseEntity
    {
        public string LogId { get; set; }
        public string LogType { get; set; }
        public string LogMessage { get; set; }
        public string LogIp { get; set; }
        public string LogUrl { get; set; }
        public string LogStackTrace { get; set; }
    }
}