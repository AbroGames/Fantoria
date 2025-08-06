using System;
using Godot;

namespace Fantoria.Lib.Utils.Extensions;

public static class NodeNetworkExtensions
{
    
    public static bool IsClient(this Node node)
    {
        return NodeNetworkExtensionsState.IsClientChecker(node);
    }
    
    public static bool IsServer(this Node node)
    {
        return node.GetTree().GetMultiplayer().IsServer();
    }

    public static void DoClient(this Godot.Node node, Action clientAction)
    {
        if (IsClient(node))
        {
            clientAction();
        }
    }
    
    public static void DoServer(this Node node, Action serverAction)
    {
        if (IsServer(node))
        {
            serverAction();
        }
    }
    
    public static void DoClientServer(this Node node, Action clientAction, Action serverAction)
    {
        DoClient(node, clientAction);
        DoServer(node, serverAction);
    }
}

// Required because NodeNetworkExtensions is included via global using, and we don't want to expose the _isClientChecker field there
public static class NodeNetworkExtensionsState
{
    internal static Func<Node, bool> IsClientChecker;

    public static void SetIsClientChecker(Func<Node, bool> isClientChecker)
    {
        IsClientChecker = isClientChecker;
    }
}