/*
Copyright 2015 VAE, Inc.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
using System.Linq;
using System.Collections.Generic;

namespace VAE.CLI.Flags
{
    /// <summary>
    /// Flags Command Line Parsing library for C#.
    /// 
    /// This library is in large part inspired or even ripped off from http://golang.org/pkg/flags in design
    /// and usage.
    /// 
    /// </summary>
    /// <example>
    /// using VAE.CLI;
    /// 
    /// ...
    /// 
    /// var flags = new Flags();
    /// 
    /// var intFlag = flags.AddIntFlag("myintflag", 0, "An integer flag"); // defaults to 0
    /// var stringFlag = flags.AddStringFlag("mystringflag", "nothing", "A string flag"); // defaults to "nothing"
    /// var boolFlag = flags.AddBoolFlag("myboolFlag", false, "A bool flag"); // defaults to false.
    /// 
    /// flags.Parse(args);
    /// 
    /// ...
    /// 
    /// if (boolFlag.Value) {
    ///   // do something if the boolFlag was set to null
    /// }
    /// </example>
    public class Parser
    {
        private List<string> _args = new List<string>();
        private Dictionary<string, FlagValue> _flags = new Dictionary<string, FlagValue>();
        private bool _HasErrors;
        private string _ErrorText = "";

        /// <summary>
        /// Add your own Custom FlagValue handler for flag parsing.
        /// </summary>
        public FlagValue<T> Add<T>(string name, FlagValue<T> val)
        {
            _flags[name] = val;
            return val;
        }

        /// <summary>
        /// Add an Integer command line flag to be parsed.
        /// </summary>
        public FlagValue<int> AddIntFlag(string name, int default_value, string description="")
        {
            return Add(name, new Val<int>(default_value, 
                (s) => {
                    int i;
                    bool result = int.TryParse(s, out i);
                    return Tuple.Create(result, i);
                }, description: description));
        }

        /// <summary>
        /// Adds a boolean command line flag to be parsed.
        /// </summary>
        /// <param name="description">Description.</param>
        public FlagValue<bool> AddBoolFlag(string name, bool default_value, string description="")
        {
            return Add(name, new Val<bool>(default_value, 
                (s) => {
                    bool b;
                    bool result = bool.TryParse(s, out b);
                    return Tuple.Create(result, b);
                }, description: description, is_bool: true));
        }

        /// <summary>
        /// Add a string command line flag to be parsed.
        /// </summary>
        public FlagValue<string> AddStringFlag(string name, string default_value, string description="")
        {
            return Add(name, new Val<string>(default_value, 
                (s) => {
                    return Tuple.Create(true, s);
                }, description: description));
        }

        /// <summary>
        /// Parse a set of command line args.
        /// </summary>
        /// <remarks>
        /// This method will stop at the first error encountered while parsing flags.
        /// Check the HasErrors property to see if there was an error while parsing.
        /// If HasErrors is true then the ErrorText property will contain the error message.
        /// </remarks>
        public void Parse(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                var flag = args[i];
                if (flag[0] == '-')
                {
                    var start = 1;
                    if (flag[1] == '-') { start = 2; }
                    flag = args[i].Substring(start);
                }
                else {
                    _args.Add(flag);
                    continue;
                }
                string val = "";
                bool had_equals = false;
                if (flag.Contains("="))
                {
                    had_equals = true;
                    var results = flag.Split(new char[] { '=' }, 2);
                    flag = results[0];
                    val = results[1];
                }
                else if (args.Length > (i+1))
                {
                    val = args[i + 1];
                }

                if (_flags.ContainsKey(flag))
                {
                    var fv = _flags[flag];
                    if (val != "" && had_equals)
                    {
                        fv.Set(val);
                    }
                    else if (fv.IsBool)
                    {
                        fv.Set(bool.TrueString);
                    }
                    else if (val != "")
                    {

                        if (!fv.Set(val))
                        {
                            _ErrorText = "\"" + val + "\" is not a valid " + flag + ".";
                            _HasErrors = true;
                        }
                        i++;
                    }
                    else
                    {
                        // This would be a parse error.
                        _ErrorText = flag + " expects an argument.";
                        _HasErrors = true;
                    }
                }
                else
                {
                    // This is an unparsed arg.
                    _args.Add(args[i]);
                }
                if (_HasErrors)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns the text of the first error encountered while parsing.
        /// </summary>
        public string ErrorText
        {
            get { return _ErrorText; }
        }

        /// <summary>
        /// Returns true if the parser encountered an Error while parsing.
        /// </summary>
        public bool HasErrors
        {
            get { return _HasErrors; }
        }

        /// <summary>
        /// Returns any left over arguments not parsed by the command line parser.
        /// </summary>
        public List<string> Args
        {
            get { return _args; }
        }

        /// <summary>
        /// Returns helptext for these arguments in the form:
        /// 
        /// - flagName <flag description>
        /// 
        /// The flag description is built from the ToString() method of the FlagValue.
        /// </summary>
        /// <value>The help text.</value>
        public List<string> HelpText
        {
            get
            {

                var help = new List<string>();
                foreach (var kv in _flags)
                {
                    help.Add("-" + kv.Key + " " + kv.Value.ToString());
                }
                return help;
            }
        }
    }
}

