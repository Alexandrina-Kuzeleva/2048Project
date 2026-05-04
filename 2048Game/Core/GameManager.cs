using System;
using System.Collections.Generic;
using _2048Game.Systems;
using _2048Game.Entities;
using _2048Game.Factories;
using _2048Game.UI;
using _2048Game.Demos;
using _2048Game.Strategies;
using _2048Game.States;

namespace _2048Game.Core
{
    public class GameManager
    {
        private static GameManager? _instance;
        private GameStateContext _stateContext;

        public int MapSize { get; set; }
        public Difficulty GameDifficulty { get; set; }

        private GameManager()
        {
            MapSize = 4;
            GameDifficulty = Difficulty.Normal;
            _stateContext = new GameStateContext();
        }

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        public void Run()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    _stateContext.HandleInput(key);
                }
                _stateContext.Update();
                System.Threading.Thread.Sleep(50);
            }
        }

        public void ShowSettings()
        {
            Console.WriteLine($"Settings: size={MapSize}, difficulty={GameDifficulty}");
        }
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}