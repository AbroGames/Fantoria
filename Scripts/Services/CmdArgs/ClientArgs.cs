namespace Fantoria.Scripts.Services.CmdArgs;

public readonly record struct ClientArgs(bool AutoConnect, string AutoConnectIp, int? AutoConnectPort)
{
    public static readonly string AutoConnectFlag = "--auto-connect";
    public static readonly string AutoConnectIpFlag = "--auto-connect-ip";
    public static readonly string AutoConnectPortFlag = "--auto-connect-port";
    
    public static ClientArgs GetFromCmd(Lib.Services.CmdArgs.CmdArgsService argsService)
    {
        return new ClientArgs(
            argsService.ContainsInCmdArgs(AutoConnectFlag),
            argsService.GetStringFromCmdArgs(AutoConnectIpFlag),
            argsService.GetIntFromCmdArgs(AutoConnectPortFlag)
        );
    }
}
