using Xunit;
using _2048Game.Commands;
using _2048Game.Core;

namespace _2048Game.Tests.Tests
{
    public class CommandTests
    {
        [Fact]
        public void InputHandler_ShouldExecuteBoundCommand()
        {
            var handler = new InputHandler();
            bool executed = false;
            var command = new TestCommand(() => executed = true);

            handler.BindCommand(ConsoleKey.UpArrow, command);
            handler.HandleInput(ConsoleKey.UpArrow);

            Assert.True(executed);
        }

        [Fact]
        public void InputHandler_ShouldNotExecuteUnboundKey()
        {
            var handler = new InputHandler();
            bool executed = false;
            var command = new TestCommand(() => executed = true);

            handler.BindCommand(ConsoleKey.UpArrow, command);
            handler.HandleInput(ConsoleKey.DownArrow);

            Assert.False(executed);
        }

        [Fact]
        public void InputHandler_ShouldRemapCommand()
        {
            var handler = new InputHandler();
            bool oldExecuted = false;
            bool newExecuted = false;

            var oldCommand = new TestCommand(() => oldExecuted = true);
            var newCommand = new TestCommand(() => newExecuted = true);

            handler.BindCommand(ConsoleKey.UpArrow, oldCommand);
            handler.HandleInput(ConsoleKey.UpArrow);
            Assert.True(oldExecuted);

            oldExecuted = false;
            handler.RemapCommand(ConsoleKey.UpArrow, ConsoleKey.W, newCommand);

            handler.HandleInput(ConsoleKey.UpArrow);
            Assert.False(oldExecuted);

            handler.HandleInput(ConsoleKey.W);
            Assert.True(newExecuted);
        }

        [Fact]
        public void InputHandler_ShouldUnbindCommand()
        {
            var handler = new InputHandler();
            bool executed = false;
            var command = new TestCommand(() => executed = true);

            handler.BindCommand(ConsoleKey.UpArrow, command);
            handler.UnbindCommand(ConsoleKey.UpArrow);
            handler.HandleInput(ConsoleKey.UpArrow);

            Assert.False(executed);
        }

        [Fact]
        public void InputHandler_ShouldShowBindings()
        {
            var handler = new InputHandler();
            var command = new TestCommand(() => { });

            handler.BindCommand(ConsoleKey.UpArrow, command);

            // Просто проверяем, что метод не падает
            var exception = Record.Exception(() => handler.ShowBindings());
            Assert.Null(exception);
        }

        private class TestCommand : ICommand
        {
            private Action _execute;

            public TestCommand(Action execute)
            {
                _execute = execute;
            }

            public void Execute() => _execute();
            public string GetDescription() => "Test Command";
        }
    }
}