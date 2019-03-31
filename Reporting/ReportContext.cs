using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reporting
{
    public sealed class ReportContext : IDisposable
    {
       
        public ReportContext(ILoggerFactory loggerFactory, string tempPath)
        {
            LoggerFactory = loggerFactory;
            TempFiles = new Dictionary<string, IEnumerable<string>>();
            TempPath = tempPath;
            CreateSessionId();
        }
        public string SessionId { get; private set; }
        public string TempPath { get; private set; }
        public ILoggerFactory LoggerFactory { get; private set; }
        public Dictionary<string,IEnumerable<string>> TempFiles { get; private set; }

        public void AddTempFiles(string engineName, IEnumerable<string> files)
        {
            TempFiles.Add(engineName, files);
        }
        
        public string GetSessionPath()
        {
            return $"{TempPath}\\{SessionId}";
        }
        private void CreateSessionId()
        {
            SessionId = Guid.NewGuid().ToString();
            Directory.CreateDirectory(GetSessionPath());
        }

        #region IDisposable Support
        private bool disposedValue = false; 
        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Clean up directory
                    Directory.Delete(GetSessionPath(), true);
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
