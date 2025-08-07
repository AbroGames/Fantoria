using System.Collections.Generic;

namespace Fantoria.Scripts.Services.CmdArgs;

public readonly record struct DedicatedServerArgs(bool IsHeadless, int? Port, string Admin, int? ParentPid)
{
    public static readonly string DedicatedServerFlag = "--server";
    
    public static readonly string HeadlessFlag = "--headless";
    public static readonly string PortParam = "--port";
    public static readonly string AdminParam = "--admin";
    public static readonly string ParentPidParam = "--parent-pid";
    
    public static DedicatedServerArgs GetFromCmd(Lib.Services.CmdArgs.CmdArgsService argsService)
    {
        return new DedicatedServerArgs(
            argsService.ContainsInCmdArgs(HeadlessFlag),
            argsService.GetIntFromCmdArgs(PortParam),
            argsService.GetStringFromCmdArgs(AdminParam),
            argsService.GetIntFromCmdArgs(ParentPidParam)
        );
    }

    public string[] GetArrayToStartDedicatedServer()
    {
        List<string> listParams = [];
        
        listParams.Add(DedicatedServerFlag);
        listParams.AddRange([PortParam, Port.ToString()]);
        
        if (IsHeadless) listParams.Add(HeadlessFlag);
        if (Admin != null) listParams.AddRange([AdminParam, Admin]);
        if (ParentPid.HasValue) listParams.AddRange([ParentPidParam, ParentPid.ToString()]);

        return listParams.ToArray();
    }
}
