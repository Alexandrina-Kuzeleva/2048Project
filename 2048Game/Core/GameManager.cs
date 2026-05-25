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
        private SaveManager _saveManager;
        private HighScoreRepository _highScoreRepository;

        public int MapSize { get; set; }
        public Difficulty GameDifficulty { get; set; }
        public string CurrentPlayerName { get; set; } = "Anonymous";

        private GameManager()
        {
            MapSize = 4;
            GameDifficulty = Difficulty.Normal;
            _stateContext = new GameStateContext();
            _saveManager = new SaveManager();
            _highScoreRepository = new HighScoreRepository();
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
            ShowWelcomeMessage();
            _stateContext.CurrentState.Enter();

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

        private void ShowWelcomeMessage()
        {
            Console.Clear();
            var content = new List<string>
            {
                "Welcome to 2048 Game!",
                "Combine tiles, activate bonuses, and break obstacles.",
                "",
                "Use arrow keys to move and ESC to return to menu.",
                "Save with F5 and load with F9.",
                "",
                "Press any key to continue..."
            };

            FrameRenderer.DrawFrame("WELCOME", content, ConsoleColor.Green);
            Console.ReadKey(true);
        }

        public void ShowSettings()
        {
            Console.WriteLine($"Settings: size={MapSize}, difficulty={GameDifficulty}");
        }

        public void ShowHighScores()
        {
            var tempHUD = new ConsoleHUD(_stateContext.ScoreManager);
            tempHUD.DisplayHighScores(_highScoreRepository);
        }

        public void SaveGame(Board board, ScoreManager scoreManager)
        {
            var saveData = _saveManager.SaveGame(board, scoreManager);
            Console.WriteLine($"\nGame saved! ({saveData.SaveTime:HH:mm:ss})");
            Console.ReadKey(true);
        }

        public SaveData? LoadGame()
        {
            return _saveManager.LoadGame();
        }

        public bool HasSave()
        {
            return _saveManager.SaveExists();
        }
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}