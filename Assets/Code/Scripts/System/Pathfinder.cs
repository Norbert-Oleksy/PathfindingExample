using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    #region Methods
    public static List<Tile> FindPath(Tile start, Tile target)
    {
        if (start == null || target == null) return null;

        MapManager map = SystemManager.Instance.MapManager;

        var openSet = new List<TileNode>();
        var closedSet = new HashSet<TileNode>();

        TileNode startNode = new TileNode(start, null, 0, Heuristic(start, target));
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => a.F.CompareTo(b.F));
            TileNode current = openSet[0];
            openSet.RemoveAt(0);

            if (current.Tile == target)
                return ReconstructPath(current);

            closedSet.Add(current);

            foreach (Tile neighbour in map.GetNeighbours(current.Tile))
            {
                if (neighbour.Type != TileType.Traversable)
                    continue;

                TileNode neighbourNode = new TileNode(neighbour, current,
                    current.G + 1, Heuristic(neighbour, target));

                if (closedSet.Contains(neighbourNode))
                    continue;

                TileNode existing = openSet.Find(n => n.Tile == neighbour);
                if (existing == null || neighbourNode.G < existing.G)
                {
                    if (existing != null)
                        openSet.Remove(existing);

                    openSet.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    private static int Heuristic(Tile a, Tile b)
    {
        return Mathf.Abs(a.Position.X - b.Position.X) + Mathf.Abs(a.Position.Y - b.Position.Y);
    }

    private static List<Tile> ReconstructPath(TileNode node)
    {
        List<Tile> path = new List<Tile>();
        while (node != null)
        {
            path.Add(node.Tile);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }

    #endregion

    private class TileNode
    {
        public Tile Tile;
        public TileNode Parent;
        public int G;
        public int H;
        public int F => G + H;

        public TileNode(Tile tile, TileNode parent, int g, int h)
        {
            Tile = tile;
            Parent = parent;
            G = g;
            H = h;
        }

        public override bool Equals(object obj) => obj is TileNode n && n.Tile == Tile;
        public override int GetHashCode() => Tile.GetHashCode();
    }
}