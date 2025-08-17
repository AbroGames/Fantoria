using System;
using System.Linq;
using Fantoria.Lib.Utils.Cooldown;
using Godot;

namespace Fantoria.Lib.Nodes.Process;

public partial class ProcessDeadChecker : Node
{
    
    public int? ProcessPid { get; set; }
    public Func<int, string> LogMessageGenerator { get; set; } = pid => $"Process {pid} is dead.";
    public Action ActionWhenDead { get; set; }
    
    private AutoCooldown _processDeadCheckCooldown = new(5);
    
    public ProcessDeadChecker Init(int processPid, Action actionWhenDead, Func<int, string> logMessageGenerator = null)
    {
        ProcessPid = processPid;
        ActionWhenDead = actionWhenDead;
        if (logMessageGenerator != null) LogMessageGenerator = logMessageGenerator;
        
        _processDeadCheckCooldown.ActionWhenReady += CheckProcessIsDead;
        return this;
    }

    public override void _Process(double delta)
    {
        _processDeadCheckCooldown.Update(delta);
    }

    public void CheckProcessIsDead()
    {
        if (ProcessPid.HasValue && !System.Diagnostics.Process.GetProcesses().Any(x => x.Id == ProcessPid.Value))
        {
            Log.Info(LogMessageGenerator(ProcessPid.Value));
            ActionWhenDead?.Invoke();
        }
    }
}
