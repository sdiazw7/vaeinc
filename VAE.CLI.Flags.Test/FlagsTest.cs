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
using System.Collections.Generic;
using NUnit.Framework;
using VAE.CLI.Flags;

namespace VAE.CLI.Tests
{
    [TestFixture]
    public class FlagsTest
    {
        Parser parser;
        FlagValue<string> stringFlag;
        FlagValue<int> intFlag;
        FlagValue<bool> boolFlag;

        [SetUp]
        public void TestSetup()
        {
            parser = new Parser();
            stringFlag = parser.AddStringFlag("MyStringFlag", "");
            intFlag = parser.AddIntFlag("MyIntFlag", 0);
            boolFlag = parser.AddBoolFlag("MyBoolFlag", false);
        }

        [Test]
        public void ValidStringFlagNameInArgs_ReturnsFlagValue()
        {
            string filePath = @"C:\dev\vaeinctest.txt";
            string[] args = new string[] { @"-MyStringFlag=C:\dev\vaeinctest.txt" };
            parser.Parse(args);
            Assert.AreEqual(filePath, stringFlag.Value);
        }

        [Test]
        public void InvalidStringFlagNameInArgs_ReturnsEmptyFlagValue()
        {
            string[] args = new string[] { @"-MyStringFlagx=C:\dev\vaeinctest.txt" };
            parser.Parse(args);
            Assert.IsEmpty(stringFlag.Value);
        }

        [Test]
        public void ValidIntFlagNameInArgs_ReturnsFlagValue()
        {
            string[] args = new string[] { @"-MyIntFlag=1" };
            parser.Parse(args);
            Assert.AreEqual(1, intFlag.Value);
        }

        [Test]
        public void InvalidIntFlagNameInArgs_ReturnsDefaultFlagValue()
        {
            string[] args = new string[] { @"-MyIntFlagx=1" };
            parser.Parse(args);
            Assert.AreEqual(0, intFlag.Value);
        }

        [Test]
        public void InvalidIntFlagValueInArgs_ReturnsDefaultFlagValue()
        {
            string[] args = new string[] { @"-MyIntFlag=aaaa" };
            parser.Parse(args);
            Assert.AreEqual(0, intFlag.Value);
        }

        [Test]
        public void ValidBoolFlagNameInArgs_ReturnsFlagValue()
        {
            string[] args = new string[] { @"-MyBoolFlag=true" };
            parser.Parse(args);
            Assert.AreEqual(true, boolFlag.Value);
        }

        [Test]
        public void InvalidBoolFlagNameInArgs_ReturnsDefaultFlagValue()
        {
            string[] args = new string[] { @"-MyBoolFlagx=false" };
            parser.Parse(args);
            Assert.AreEqual(false, boolFlag.Value);
        }

        [Test]
        public void InvalidBoolFlagValueInArgs_ReturnsDefaultFlagValue()
        {
            string[] args = new string[] { @"-MyBoolFlag=aaaa" };
            parser.Parse(args);
            Assert.AreEqual(false, boolFlag.Value);
        }

        [Test]
        public void NoArgs_ReturnsEmptyStringFlagValue()
        {
            string[] args = new string[0];
            parser.Parse(args);
            Assert.IsEmpty(stringFlag.Value);
        }

        [Test]
        public void NoArgs_ReturnsDefaultIntFlagValue()
        {
            string[] args = new string[0];
            parser.Parse(args);
            Assert.AreEqual(0, intFlag.Value);
        }

        [Test]
        public void NoArgs_ReturnsDefaultBoolFlagValue()
        {
            string[] args = new string[0];
            parser.Parse(args);
            Assert.AreEqual(false, boolFlag.Value);
        }

    }
}

