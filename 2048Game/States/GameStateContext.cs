using System;
using _2048Game.Core;

namespace _2048Game.States
{
    public class GameStateContext
    {
        private IGameState _currentState;
        private IGameState _menuState;
        private IGameState _gameState;
        private IGameState _pauseState;
        private IGameState _gameOverState;

        public GameStateContext()
        {
            _menuState = new MenuState(this);
            _gameState = new GameState(this);
            _pauseState = new PauseState(this);
            _gameOverState = new GameOverState(this);

            _currentState = _menuState;
            _currentState.Enter();
        }

        public void SetState(IGameState newState)
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState.Update();
        }

        public void HandleInput(ConsoleKey key)
        {
            _currentState.HandleInput(key);
        }

        public IGameState MenuState => _menuState;
        public IGameState GameState => _gameState;
        public IGameState PauseState => _pauseState;
        public IGameState GameOverState => _gameOverState;

        public IGameState CurrentState => _currentState;
    }
}