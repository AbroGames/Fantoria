using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Fantoria.Lib.Nodes.MpSync;

/// <summary>
/// This is a MultiplayerSynchronizer, which in the Init(Node observableNode) method automatically scans all properties and fields of observableNode marked with the Sync attribute and adds them to the SceneReplicationConfig as synchronized.
/// This node cannot be added from the editor, since in that case it will not work correctly with MultiplayerSpawner. It is required that SceneReplicationConfig is set up before _EnterTree().
/// That means either configuring it through the editor (in which case you should use the regular MultiplayerSynchronizer), or by calling Init(Node observableNode) before AddChild(attributeMultiplayerSynchronizer) (this is our case, and it only works from code).
/// </summary>
public partial class AttributeMultiplayerSynchronizer : MultiplayerSynchronizer
{

    private Node _observableNode;
    
    public AttributeMultiplayerSynchronizer(Node observableNode)
    {
        if (observableNode == null)
        {
            Log.Error("AttributeMultiplayerSynchronizer must have not null _observableNode. Synchronizer path: " + GetPath());
            return;
        }
        _observableNode = observableNode;
        
        // Add all [Sync] properties and fields to ReplicationConfig.Property
        SetReplicationConfig(new SceneReplicationConfig());
        List<MemberInfo> syncedMembers = GetSyncedMembers(observableNode);
        syncedMembers.ForEach(AddMemberToReplicationConfig);
    }

    public override void _Ready()
    {
        SetRootPath(GetPathTo(_observableNode));
    }

    private List<MemberInfo> GetSyncedMembers(Node observableNode)
    {
        Type type = observableNode.GetType();
        List<MemberInfo> result = new();
        
        
        List<MemberInfo> members = new();
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        members.AddRange(type.GetProperties(bindingFlags));
        members.AddRange(type.GetFields(bindingFlags));

        foreach (MemberInfo member in members)
        {
            
            if (Attribute.IsDefined(member, typeof(SyncAttribute)))
            {
                if (!Attribute.IsDefined(member, typeof(ExportAttribute)))
                {
                    Log.Critical($"{member.MemberType} '{member.Name}' in type '{observableNode.GetType()}' has Sync attribute, but doesn't have Export attribute");
                }
                else
                {
                    result.Add(member);
                }
            }
        }

        return result;
    }

    private void AddMemberToReplicationConfig(MemberInfo member)
    {
        SyncAttribute syncAttr = member.GetCustomAttribute<SyncAttribute>();
        if (syncAttr == null)
        {
            Log.Error($"Can't add {member.MemberType} '{member.Name}' to ReplicationConfig of '{GetPath()}' because it don't have Sync attribute");
            return;
        }
        
        string pathToNode = ".";
        string nodeAndMemberSeparator = ":";
        string memberName = member.Name;
        string fullPropertyName = pathToNode + nodeAndMemberSeparator + memberName;
        
        GetReplicationConfig().AddProperty(fullPropertyName);
        GetReplicationConfig().PropertySetReplicationMode(fullPropertyName, syncAttr.ReplicationMode);
        GetReplicationConfig().PropertySetSpawn(fullPropertyName, syncAttr.Spawn);
    }
}