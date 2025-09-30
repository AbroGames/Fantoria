using System.Collections.Generic;
using Fantoria.Scenes.World.Data;
using Fantoria.Scenes.World.Data.MapPoint;
using Fantoria.Scenes.World.Services;
using Fantoria.Scenes.World.Services.StartStop;
using Godot;
using MessagePack;

namespace Fantoria.Scenes.World.Tree.Entity.Building;

public partial class MapPoint : Node2D
{
    
    public MapPointData Data { get; private set; }

    public void UpdatePosition(Vector2 position)
    {
        Position = position;
        Data.PositionX = Position.X;
        Data.PositionY = Position.Y;
    } 

    public class Loader : IWorldTreeLoader
    {
        public const string Name = "MapPoint";
        public string GetName() => Name;
        
        private readonly Dictionary<long, MapPoint> _mapPointById = new();

        public void Create(World world)
        {
            foreach (MapPointData mapPointData in world.Data.MapPoint.MapPointById.Values)
            {
                MapPoint mapPoint = world.PackedScenes.MapPoint.Instantiate<MapPoint>();
                world.Tree.MapSurface.AddChildWithUniqueName(mapPoint, "MapPoint");
                _mapPointById.Add(mapPointData.Id, mapPoint);
            }
        }

        public void Init(World world)
        {
            foreach (MapPointData mapPointData in world.Data.MapPoint.MapPointById.Values)
            {
                MapPoint mapPoint = _mapPointById[mapPointData.Id];
                mapPoint.Position = Vec(mapPointData.PositionX, mapPointData.PositionY);
                mapPoint.Data = mapPointData;
            }
        }
    }
}