using System;
using System.Linq;
using System.Text;
using Godot;

namespace Fantoria.Lib.Services.Logging.Loggers;

internal class DefaultLogger : ILogger
{

    public static readonly string LogPushCmdArg = "--logpush";
    private bool _defaultPushBehavior = OS.GetCmdlineArgs().Contains(LogPushCmdArg);
    
    public void Debug(object msg = null)
    {
        Print(msg, "gray");
    }

    public void Info(object msg = null)
    {
        Print(msg);
    }

    public void Warning(object msg = null, Exception exception = null)
    {
        Print(msg, "yellow", exception, pushWarning: _defaultPushBehavior);
    }

    public void Error(object msg = null, Exception exception = null)
    {
        Print(msg, "orange", exception, pushError: _defaultPushBehavior);
    }

    public void Critical(object msg = null, Exception exception = null)
    {
        Print(msg, "red", exception, pushError: _defaultPushBehavior);
    }

    private void Print(object msg = null, string color = null, Exception exception = null, bool pushWarning = false, bool pushError = false)
    {
        if (msg is null && exception is null)
        {
            GD.Print();
            return;
        }
        
        var sb = new StringBuilder();
        sb.Append(msg ?? "");
        sb.Append(msg is null || exception is null ? "" : "\n");
        sb.Append(exception?.ToString() ?? "");

        if (color == "" || color == "white" || color is null)
        {
            GD.PrintRich($"{sb}");
            return;
        }
        
        GD.PrintRich($"[color={color}]{sb}[/color]");
        if (pushWarning) GD.PushWarning(sb.ToString());
        if (pushError) GD.PushError(sb.ToString());
    }
}
