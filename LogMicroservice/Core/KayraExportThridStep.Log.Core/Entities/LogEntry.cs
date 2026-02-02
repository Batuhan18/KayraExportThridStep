using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Log.Core.Entities
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string? Exception { get; set; }
        public string? StackTrace { get; set; }
        public string? ServiceName { get; set; }
        public string? MachineName { get; set; }
        public string? UserName { get; set; }
        public string? RequestPath { get; set; }
        public string? RequestMethod { get; set; }
        public int? StatusCode { get; set; }
        public string? Properties { get; set; }
    }

    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR,
        CRITICAL
    }
}
