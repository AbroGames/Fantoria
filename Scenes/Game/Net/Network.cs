using Godot;

namespace Fantoria.Scenes.Game.Net;

public class Network
{
    
    public MultiplayerApi Api { get; }
    public NetworkStateMachine StateMachine { get; } = new();
    
    public Network(MultiplayerApi api)
    {
        Api = api;
        Api.ConnectedToServer += ConnectedToServerEvent;
        Api.PeerConnected += PeerConnectedEvent;
        Api.ConnectionFailed += ConnectionFailedEvent;
        Api.PeerDisconnected += PeerDisconnectedEvent;
        Api.ServerDisconnected += ServerDisconnectedEvent;
    }

    /// <summary>Try to connect to the server</summary>
    /// <returns>
    /// Returns Godot.Error.Ok if a client was created
    /// Godot.Error.AlreadyInUse if this ENetMultiplayerPeer instance already has an open connection
    /// Godot.Error.CantCreate if the client could not be created
    /// Godot.Error.AlreadyInUse if the client already connected
    /// </returns>
    public Error ConnectToServer(string host, int port)
    {
        if (!StateMachine.CanInitialize)
        {
            Log.Error($"Can't initialize network in current state: {StateMachine.CurrentState}");
            return Error.AlreadyInUse;
        }
        
        Log.Info($"Connecting to the server at {host}:{port}");

        StateMachine.SetState(NetworkStateMachine.State.Connecting);
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateClient(host, port);
        Api.MultiplayerPeer = peer;
		
        if (error != Error.Ok)
        {
            Log.Error($"Failed to connect to the server: {error}");
        }
        return error; 
    }
    
    /// <summary>Try to host server</summary>
    /// <returns>
    /// Returns Godot.Error.Ok if a server was created
    /// Godot.Error.AlreadyInUse if this ENetMultiplayerPeer instance already has an open connection
    /// Godot.Error.CantCreate if the server could not be created
    /// Godot.Error.AlreadyInUse if the server already hosted
    /// </returns>
    public Error HostServer(int port, int maxClients = 32)
    {
        if (!StateMachine.CanInitialize)
        {
            Log.Error($"Can't initialize network in current state: {StateMachine.CurrentState}");
            return Error.AlreadyInUse;
        }
        
        Log.Info($"Starting server on port {port}");
        
        StateMachine.SetState(NetworkStateMachine.State.Hosting);
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(port, maxClients);
        Api.MultiplayerPeer = peer;

        if (error == Error.Ok)
        {
            StateMachine.SetState(NetworkStateMachine.State.Hosted);
            Log.Info("Started server successfully");
        }
        else
        {
            Log.Error($"Failed to start server: {error}");
        }
        
        return error;
    }
    
    public void Shutdown()
    {
        //TODO gracefully отключение
    }

    private void ConnectedToServerEvent()
    {
        StateMachine.SetState(NetworkStateMachine.State.Connected);
        Log.Info("Connected to the server successfully");
    }

    private void ConnectionFailedEvent()
    {
        StateMachine.SetState(NetworkStateMachine.State.Disconnected);
        Log.Error("Connection to the server failed");
    }

    private void ServerDisconnectedEvent()
    {
        StateMachine.SetState(NetworkStateMachine.State.Disconnected);
        Log.Info("Server disconnected");
    }
    
    private void PeerConnectedEvent(long id)
    {
        Log.Debug($"Network peer connected: {id}");
    }
    
    private void PeerDisconnectedEvent(long id)
    {
        Log.Debug($"Network peer disconnected: {id}");
    }
}