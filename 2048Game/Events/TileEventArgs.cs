using System;
using _2048Game.Entities;

namespace _2048Game.Events
{
    public class TileEventArgs : EventArgs
    {
        public Tile Tile { get; }
        public int PositionX { get; }
        public int PositionY { get; }
        public string Action { get; }

        public TileEventArgs(Tile tile, int x, int y, string action)
        {
            Tile = tile;
            PositionX = x;
            PositionY = y;
            Action = action;
        }
    }
}