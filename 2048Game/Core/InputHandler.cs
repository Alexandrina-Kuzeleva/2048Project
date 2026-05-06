using System;
using System.Collections.Generic;
using _2048Game.Commands;

namespace _2048Game.Core
{
    public class InputHandler
    {
        private Dictionary<ConsoleKey, ICommand> _commands;

        public InputHandler()
        {
            _commands = new Dictionary<ConsoleKey, ICommand>();
        }

        public void BindCommand(ConsoleKey key, ICommand command)
        {
            _commands[key] = command;
        }

        public void UnbindCommand(ConsoleKey key)
        {
            if (_commands.ContainsKey(key))
            {
                _commands.Remove(key);
            }
        }

        public bool HandleInput(ConsoleKey key)
        {
            if (_commands.TryGetValue(key, out ICommand? command))
            {
                command.Execute();
                return true;
            }
            return false;
        }

        public void ShowBindings()
        {
            Console.WriteLine("Current Key Bindings");
            foreach (var binding in _commands)
            {
                Console.WriteLine($"  {binding.Key} -> {binding.Value.GetDescription()}");
            }
        }

        public void RemapCommand(ConsoleKey oldKey, ConsoleKey newKey, ICommand command)
        {
            UnbindCommand(oldKey);
            BindCommand(newKey, command);
            Console.WriteLine($"Remapped: {oldKey} → {newKey} for {command.GetDescription()}");
        }

        public IEnumerable<KeyValuePair<ConsoleKey, ICommand>> GetAllBindings()
        {
            return _commands;
        }
    }
}