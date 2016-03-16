﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Langwitch
{
    public class ConfigManager
    {
        public IList<int> LanguageSwitchKeyArray { get; set; }
        public Keys LanguageSwitchKeys { get { return ConvertIntsToKeys(LanguageSwitchKeyArray); } }
        public IList<int> LayoutSwitchKeyArray { get; set; }
        public Keys LayoutSwitchKeys { get { return ConvertIntsToKeys(LayoutSwitchKeyArray); } }

        public ConfigManager()
        {
            LanguageSwitchKeyArray = new int[] { (int) Keys.CapsLock };
            LayoutSwitchKeyArray = new int[] { };
        }

        private Keys ConvertIntsToKeys(IList<int> ints)
        {
            var result = (Keys) ints.FirstOrDefault();
            for (var i = 1; i < ints.Count; i++)
            {
                result |= (Keys) ints[i];
            }
            return result;
        }

        private void ReadFromString(string str)
        {
            var arguments = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            arguments.ForEach(x => ReadArgument(x));
        }

        private IList<int> ReadArray(string arrayString)
        {
            return arrayString.Split(new[] { '+' }).Select(x => int.Parse(x)).ToList();
        }

        private void ReadArgument(string argument)
        {
            if (!argument.StartsWith("-"))
                throw new ArgumentException("Arguments must start with '-'");
            var parts = argument.Substring(1).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            var argumentName = parts[0];
            if (parts.Length > 1)
            {
                var argumentValue = parts[1];
                if (argumentName == "LanguageSwitchKeys")
                    LanguageSwitchKeyArray = ReadArray(argumentValue);
                if (argumentName == "LayoutSwitchKeys")
                    LayoutSwitchKeyArray = ReadArray(argumentValue);
            }
        }

        public void ReadFromConfigFile()
        {
            var arguments = ConfigurationManager.AppSettings["app:arguments"];
            ReadFromString(arguments);
        }

        public void ReadFromCommandLineArguments()
        {
            var arguments = string.Join(" ", Environment.GetCommandLineArgs().Skip(1));
            ReadFromString(arguments);
        }
    }
}