using System;
using _2048Game.Core;
using _2048Game.Systems;
using _2048Game.UI;

namespace _2048Game.States
{
    public class GameStateContext
    {
        private IGameState _currentState;
        private IGameState _menuState;
        private IGameState _gameState;
        private IGameState _pauseState;
        private IGameState _gameOverState;
        private ScoreManager _scoreManager;
        private ConsoleHUD _hud;

        public GameStateContext()
        {
            _scoreManager = new ScoreManager();
            _hud = new ConsoleHUD(_scoreManager);

            _menuState = new MenuState(this);
            _gameState = new GameState(this, _scoreManager, _hud);
            _pauseState = new PauseState(this);
            _gameOverState = new GameOverState(this, _scoreManager, _hud);

            _currentState = _menuState;
        }

        public void SetState(IGameState newState)
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update() => _currentState.Update();
        public void HandleInput(ConsoleKey key) => _currentState.HandleInput(key);

        public IGameState MenuState => _menuState;
        public IGameState GameState => _gameState;
        public IGameState PauseState => _pauseState;
        public IGameState GameOverState => _gameOverState;
        public IGameState CurrentState => _currentState;
        public ScoreManager ScoreManager => _scoreManager;
        public ConsoleHUD HUD => _hud;
    }
}