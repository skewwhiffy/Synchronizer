using System;

namespace Synchronizer.Client.ArgsParser
{
    public class Args : IArgs
    {
        private Args(string[] args)
        {
            var i = 0;
            Action<string> nextAction = null;
            while (i < args.Length)
            {
                var current = args[i];
                if (nextAction != null)
                {
                    nextAction(current);
                    nextAction = null;
                }
                else if (current.StartsWith("-"))
                {
                    nextAction = GetNextAction(current);
                }
                else
                {
                    throw new ArgumentException($"I do not understand the argument {current}");
                }
                i++;
            }
            if (nextAction != null)
            {
                throw new ArgumentException("The last argument is not a value. Surely some mistake?");
            }
        }

        private Action<string> GetNextAction(string argument)
        {
            var argumentText = argument.TrimStart('-').ToLowerInvariant();
            switch (argumentText)
            {
                case "root":
                    return v => RootDirectory = v;
                default:
                    throw new ArgumentException($"I do not understand the argument {argument}");
            }
        }

        public string RootDirectory { get; private set; }

        public static IArgs For(string[] args)
        {
            return new Args(args);
        }
    }
}