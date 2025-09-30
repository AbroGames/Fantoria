using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fantoria.Scenes.World.Services.StartStop;

public class WorldLoadService
{
    public void RunAllLoaders(World world)
    {
        // Get all classes implementing IWorldLoader
        List<IWorldLoader> loaders = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IWorldLoader).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .Select(t => (IWorldLoader) Activator.CreateInstance(t))
            .ToList();

        // Sequentially execute phases
        ExecutePhase(loaders, l => l.GetCreateRequirements(), l => l.Create(world));
        ExecutePhase(loaders, l => l.GetInitRequirements(), l => l.Init(world));
        ExecutePhase(loaders, l => l.GetFinishRequirements(), l => l.Finish(world));
    }

    private void ExecutePhase(List<IWorldLoader> loaders, Func<IWorldLoader, List<string>> getRequirements, Action<IWorldLoader> action)
    {
        // Build graph: nodes = loader name
        Dictionary<string, IWorldLoader> dict = loaders.ToDictionary(l => l.GetName(), l => l);

        // Topological sort
        List<string> sorted = TopologicalSort(
            loaders.Select(l => l.GetName()).ToList(),
            name => getRequirements(dict[name])
        );

        // Execute in correct order
        foreach (string name in sorted)
        {
            IWorldLoader loader = dict[name];
            action(loader);
        }
    }

    private List<string> TopologicalSort(List<string> nodes, Func<string, List<string>> getDeps)
    {
        List<string> result = new List<string>();
        Dictionary<string, int> visited = new Dictionary<string, int>(); // 0 - not visited, 1 - in stack, 2 - done

        void Dfs(string node)
        {
            if (visited.TryGetValue(node, out var state))
            {
                if (state == 1)
                {
                    throw new Exception($"Cyclic dependency at IWorldLoader: {node}");
                }
                if (state == 2) return;
            }

            visited[node] = 1;
            foreach (var dep in getDeps(node)) Dfs(dep);
            
            visited[node] = 2;
            result.Add(node);
        }

        foreach (var node in nodes)
        {
            if (!visited.ContainsKey(node)) Dfs(node);
        }

        return result;
    }

}