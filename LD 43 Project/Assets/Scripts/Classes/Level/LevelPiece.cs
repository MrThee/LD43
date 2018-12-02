using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a piece of the level map, but not actually tied to a specific game object
/// </summary>
public class LevelPiece
{
    public enum EdgeType
    {
        Field
    }

    public struct Edge {
        public EdgeType type;
        public Vector2 dir;
    }

    public bool[,] map;
    public List<EdgeType> edges;

    public LevelPiece()
    {
    }
}
