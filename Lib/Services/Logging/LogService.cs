using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantoria.Lib.Services.Logging.Loggers;
using Godot;
using Environment = System.Environment;

namespace Fantoria.Lib.Services.Logging;

internal enum PrefixType
{
    Debug,
    Info,
    Warning,
    Error,
    Critical
}

[Service(order: -1)]
public class LogService
{
    
    private readonly HashSet<ILogger> _loggers = [];
    private readonly string[] _prefixes;
    private int _PID;
    
    public LogService()
    {
        _PID = OS.GetProcessId();
        var rawPrefixes = Enum.GetNames<PrefixType>();
        var prefixes = new List<string>();
        
        var longestPrefix = rawPrefixes.Max(p => p.Length);
        
        foreach (var rawPrefix in rawPrefixes)
        {
            var sb = new StringBuilder();
            sb.Append('|');
            sb.Append(' ');
            sb.Append(rawPrefix.ToUpper());
            for (int i = 0; i < longestPrefix - rawPrefix.Length + 1; i++)
            {
                sb.Append(' ');
            }
            sb.Append('|');
            prefixes.Add(sb.ToString());
        }

        _prefixes = prefixes.ToArray();
        
        AddLogger(new DefaultLogger());
        AddLogger(new FileLogger("/custom-logs"));
    }

    public void AddLogger(ILogger logger)
    {
        _loggers.Add(logger);
    }

    public void Debug(object msg = null)
    {
        foreach (var logger in _loggers) logger.Debug(Format(msg, PrefixType.Debug));
    }

    public void Info(object msg = null)
    {
        foreach (var logger in _loggers) logger.Info(Format(msg, PrefixType.Info));
    }

    public void Warning(object msg = null, bool printStackTrace = false)
    {
        foreach (var logger in _loggers) logger.Warning(Format(msg, PrefixType.Warning, printStackTrace));
    }

    public void Error(object msg = null, bool printStackTrace = true)
    {
        foreach (var logger in _loggers) logger.Error(Format(msg, PrefixType.Error, printStackTrace));
    }

    public void Critical(object msg = null, bool printStackTrace = true)
    {
        foreach (var logger in _loggers) logger.Critical(Format(msg, PrefixType.Critical, printStackTrace));
    }

    private string Format(object msg = null, PrefixType prefix = PrefixType.Info, bool printStackTrace = false)
    {
        if (msg is null && !printStackTrace) return null;
        string text = msg.ToString() + (printStackTrace ? "\n" + Environment.StackTrace : "");
        
        var now = DateTime.Now;
        return $"[{_PID:D6}] {now:dd.MM.yyyy HH:mm:ss.fff} {_prefixes[(int)prefix]} {text}";
    }
}

public interface ILogger
{
    void Debug(object msg = null);
    void Info(object msg = null);
    void Warning(object msg = null, Exception exception = null);
    void Error(object msg = null, Exception exception = null);
    void Critical(object msg = null, Exception exception = null);
}
