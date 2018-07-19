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
    /// The interface a CommandLine Flag must support to be used by the command line parser.
    /// </summary>
    public interface FlagValue
    {
        /// <summary>
        /// Returns a string version of the command line flag's current value and description.
        /// </summary>
        string ToString();
        /// <summary>
        /// Set the specified flag from the provided string value.
        /// </summary>
        /// <returns> true if the flag was successfully set from the string, false otherwise.</returns>
        bool Set(string flag);
        /// <summary>
        /// This method tells the parser if the method is boolean in it's value.
        /// It's a convenience to make parsing bool flags that have no value to set easier.
        /// </summary>
        bool IsBool { get; }
    }

    /// <summary>
    /// A Typed FlagValue.
    /// </summary>
    public interface FlagValue<T> : FlagValue
    {

        /// <summary>
        /// Gets the current value for this FlagValue.
        /// </summary>
        T Value { get; }
    }
}

