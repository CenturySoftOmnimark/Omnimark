using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace UKBeautyLibrary
{
    public class LogErrors : IDisposable
    {
        private readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        
        private bool disposed = false;

        public static void WriteErrorToFile(Exception ex)
        {
            try
            {
                string logFilePath = GetLogFilePath();
                using (FileStream fileStream = new FileStream(logFilePath + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                    {
                        StringBuilder strBuild = new StringBuilder();
                        strBuild.AppendLine("ErrorMessage -->" + ex.Message + "StackTrace --> " + ex.StackTrace);
                        streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        streamWriter.WriteLine(strBuild.ToString());
                        streamWriter.Flush();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }

        private static string GetLogFilePath()
        {
            string logFilePath = @"C:\UKServiceLogs\Error";
            try
            {
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }

                logFilePath = logFilePath == string.Empty ? ".\\" : logFilePath;
                logFilePath = logFilePath.EndsWith("\\") ? logFilePath : logFilePath + "\\";
            }
            catch (Exception)
            {
                throw;
            }

            return logFilePath;
        }
    }
}