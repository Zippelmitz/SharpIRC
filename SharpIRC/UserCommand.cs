﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpIRC.Commandlet;

namespace SharpIRC
{

    public sealed class UserCommand
    {
        public const string CommandStart = "/";
        public static readonly List<CommandletEvent> Commandlets = new List<CommandletEvent>(20);

        public struct CommandletEvent
        {
            public string MatchingText;
            public Action<string[]> Event;
        }

        private UserCommand()
        {
        }

        public static void Cook(string commandString)
        {
            if (!commandString.StartsWith(CommandStart))
                return; // do nothing 

            string[] parameters = commandString.Split(' ');
            parameters[0] = parameters[0].Substring(1);

            foreach (var commandlet in Commandlets)
            {
                if (parameters[0] == commandlet.MatchingText)
                {
                    commandlet.Event(parameters.Where((s, i) => !string.IsNullOrEmpty(s) && s != parameters[0]).ToArray());
                    break;
                }
            }

        }

        public static void AddCommandlet(string matchingText, Action<string[]> action)
        {
            Commandlets.Add(new CommandletEvent {MatchingText = matchingText, Event = action});
        }

    }
}
