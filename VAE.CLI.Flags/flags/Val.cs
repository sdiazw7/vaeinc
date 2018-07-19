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

namespace VAE.CLI.Flags
{
    /// <summary>
    /// Helper class to create FlagValues to use for <see cref="VAE.CLI.Flags.Parser"/>. 
    /// </summary>
    public class Val<T> : FlagValue<T>
    {
        private T _value;
        private string _description = "";
        private Func<string, Tuple<bool,T>> _parseFn;

        /// <summary>
        /// Initializes a new instance of the <see cref="VAE.CLI.Flags.Val`1"/> class.
        /// </summary>
        /// <param name="defaultValue">The default value for this flag if not set.</param>
        /// <param name="parseFn">A function that takes a string and returns a tuple of a bool value indicating success
        /// and the parsed value.</param>
        /// <param name="description">Description for this flag.</param>
        /// <param name="isBool">If set to <c>true</c> this is a bool flag.</param>
        public Val(T default_value, Func<string, Tuple<bool, T>> parse_fn, string description="", bool is_bool=false)
        {
            _value = default_value;
            _description = description;
            IsBool = is_bool;
            _parseFn = parse_fn;
        }

        public bool Set(string value)
        {
            var tpl = _parseFn(value);
            _value = tpl.Item1 ? tpl.Item2 : _value;
            return tpl.Item1;
        }

        public T Value
        {
            get { return _value; }
        }

        public bool IsBool
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return "[" + _value +  "]" + (_description == "" ? _description : " - " + _description);
        }
    }
}

