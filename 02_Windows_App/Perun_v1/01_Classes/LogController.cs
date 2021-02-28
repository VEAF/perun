using System;
using System.IO;

class LogController
{
    private static LogController _instance = new LogController();

    public static LogController instance
    {
        get
        {
            return _instance;
        }
    }

    public int level
    {
        get;
        set;
    }

    public void LogError(string strLog, string content = null)
    {
        this.WriteLog(0, strLog, content);
    }

    public void LogWarning(string strLog, string content = null)
    {
        this.WriteLog(1, strLog, content);
    }

    public void LogInfo(string strLog, string content = null)
    {
        this.WriteLog(2, strLog, content);
    }

    public void LogDebug(string strLog, string content = null)
    {
        this.WriteLog(3, strLog, content);
    }

    // TBD - done via https://stackoverflow.com/questions/20185015/how-to-write-log-file-in-c
    public void WriteLog(int logLevel, string strLog, string content = null)
    {
        if (logLevel > this.level) return;

        StreamWriter LogStreamWriter;
        FileStream LogFileStream = null;
        DirectoryInfo LogDirectoryInfo = null;
        FileInfo LogFileInfo;
        string message = strLog;

        string LogFileRootpath = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "\\Perun\\";
        string LogFilePath = LogFileRootpath + "Perun_Log_" + System.DateTime.Today.ToString("yyyyddMM") + "." + "txt";
        LogFileInfo = new FileInfo(LogFilePath);
        LogDirectoryInfo = new DirectoryInfo(LogFileInfo.DirectoryName);
        if (!LogDirectoryInfo.Exists) LogDirectoryInfo.Create();
        if (!LogFileInfo.Exists)
        {
            LogFileStream = LogFileInfo.Create();
        }
        else
        {
            try
            {
                LogFileStream = new FileStream(LogFilePath, FileMode.Append);
            }
            catch
            {
                // Do nothing 
            }
        }

        try
        {
            if (content != null)
            {
                // write the content to a file and add the name of this file to the message
                string contentFilename = "Perun_LogContent_" + System.DateTime.Today.ToString("o").Replace('T', '-').Replace(':', '-').Replace('+','-') + "." + "txt";
                string contentFilepath = LogFileRootpath + contentFilename;
                File.WriteAllText(contentFilepath, content);
                message = strLog + " - content stored in " + contentFilename;
            }
            LogStreamWriter = new StreamWriter(LogFileStream);
            LogStreamWriter.WriteLine(message);
            LogStreamWriter.Close();
        }
        catch
        {
            // Do nothing
        }
    }
}
