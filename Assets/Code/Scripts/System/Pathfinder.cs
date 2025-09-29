using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    #region Methods
    public static List<Tile> FindPathToMove(Tile start, Tile target)
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

                if (neighbour.IsOccupied)
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

    public static List<Tile> FindPathToAttack(Tile start, Tile enemyTile, int moveRange, int attackRange)
    {
        if (start == null || enemyTile == null) return null;

        Queue<TileNode> queue = new Queue<TileNode>();
        HashSet<Tile> visited = new HashSet<Tile>();

        queue.Enqueue(new TileNode(start, null, 0, 0));
        visited.Add(start);

        MapManager map = SystemManager.Instance.MapManager;

        List<TileNode> candidates = new List<TileNode>();
        TileNode fallback = null;


        while (queue.Count > 0)
        {
            TileNode current = queue.Dequeue();

            if (InAttackLine(current.Tile, enemyTile, attackRange))
            {
                if (current.G <= moveRange)
                    candidates.Add(current);
                else if (fallback == null)
                    fallback = current;
            }

            foreach (Tile neighbour in map.GetNeighbours(current.Tile))
            {
                if (neighbour.Type != TileType.Traversable) continue;
                if (neighbour.IsOccupied && neighbour != enemyTile) continue;


                if (!visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    queue.Enqueue(new TileNode(neighbour, current, current.G + 1, 0));
                }
            }
        }

        if (candidates.Count > 0)
        {
            candidates.Sort((a, b) =>
            {
                int cmp = a.G.CompareTo(b.G);
                if (cmp == 0)
                {
                    int da = Manhattan(a.Tile, enemyTile);
                    int db = Manhattan(b.Tile, enemyTile);
                    return db.CompareTo(da);
                }
                return cmp;
            });


            return ReconstructPath(candidates[0]);
        }

        return fallback != null ? ReconstructPath(fallback) : null;
    }

    public static bool InAttackLine(Tile from, Tile enemy, int attackRange)
    {
        if (from == null || enemy == null) return false;

        int dx = enemy.Position.X - from.Position.X;
        int dy = enemy.Position.Y - from.Position.Y;

        if (dx != 0 && dy != 0) return false;

        int dist = Mathf.Abs(dx) + Mathf.Abs(dy);
        if (dist > attackRange) return false;

        MapManager map = SystemManager.Instance.MapManager;

        int stepX = dx == 0 ? 0 : (dx > 0 ? 1 : -1);
        int stepY = dy == 0 ? 0 : (dy > 0 ? 1 : -1);

        int x = from.Position.X;
        int y = from.Position.Y;

        for (int i = 1; i <= dist; i++)
        {
            x += stepX;
            y += stepY;
            Tile tile = map.GetTile(x, y);
            if (tile == null) return false;

            if (tile == enemy) return true;

            if (tile.Type != TileType.Traversable && tile.Type != TileType.Cover)
                return false;
        }

        return true;
    }


    private static int Manhattan(Tile a, Tile b)
    {
        return Mathf.Abs(a.Position.X - b.Position.X) + Mathf.Abs(a.Position.Y - b.Position.Y);
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