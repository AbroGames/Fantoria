using System;
using System.Linq;
using Godot;

namespace Fantoria.Lib.Nodes.Process;

public partial class ProcessShutdowner : Node
{
    private static long[] _processShutdownNotificationTypes =
    [
        NotificationWMCloseRequest, 
        NotificationCrash, 
        NotificationDisabled, 
        NotificationPredelete,
        NotificationExitTree
    ];
    
    public int? ProcessPid { get; set; }
    public Func<int, string> LogMessageGenerator { get; set; } = pid => $"Kill process {pid}.";

    public ProcessShutdowner Init(int processPid, Func<int, string> logMessageGenerator)
    {
        ProcessPid = processPid;
        LogMessageGenerator = logMessageGenerator;
        return this;
    }
    
    public override void _Notification(int id)
    {
        if (ProcessPid.HasValue && _processShutdownNotificationTypes.Contains(id) && OS.IsProcessRunning(ProcessPid.Value))
        {
            Shutdown();
        }
    }

    public void Shutdown()
    {
        Log.Info(LogMessageGenerator(ProcessPid.Value));
        OS.Kill(ProcessPid.Value);
    }
}
