using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ArcPrime_Marczyk.Communication;

namespace ArcPrime_Marczyk.Test
{
    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void IsParamValid_nonParametricCommand_emptyParam_pass()
        {
            Command restart = Command.Restart;
            Command produce = Command.Produce;
            Assert.AreEqual(true, restart.IsParamValid(""));
            Assert.AreEqual(true, produce.IsParamValid(""));
        }

        [TestMethod]
        public void IsParamValid_nonParametricCommand_nullParam_pass()
        {
            Command restart = Command.Restart;
            Command produce = Command.Produce;
            Assert.AreEqual(true, restart.IsParamValid(null));
            Assert.AreEqual(true, produce.IsParamValid(null));
        }

        [TestMethod]
        public void IsParamValid_nonParametricCommand_anyString_pass()
        {
            Command restart = Command.Restart;
            Command produce = Command.Produce;
            Assert.AreEqual(true, restart.IsParamValid(" ? A a 5 "));
            Assert.AreEqual(true, produce.IsParamValid(" ? A a 5 "));
        }

        [TestMethod]
        public void IsParamValid_nonParametricCommand_anyValue_pass()
        {
            Command restart = Command.Restart;
            Command produce = Command.Produce;
            Assert.AreEqual(true, restart.IsParamValid("1234"));
            Assert.AreEqual(true, produce.IsParamValid("1234"));
        }


        [TestMethod]
        public void IsParamValid_inRange_nonInteger_fail()
        {
            Command restart = Command.ImportFood;;
            Assert.AreEqual(false, restart.IsParamValid("123.456"));
        }

        [TestMethod]
        public void IsParamValid_inRange_integer_pass()
        {
            Command restart = Command.ImportFood; ;
            Assert.AreEqual(true, restart.IsParamValid("123"));
        }

        [TestMethod]
        public void IsParamValid_overUpperLimit_nonInteger_fail()
        {
            Command restart = Command.ImportFood; ;
            Assert.AreEqual(false, restart.IsParamValid("200.1"));
        }

        [TestMethod]
        public void IsParamValid_overUpperLimit_integer_fail()
        {
            Command restart = Command.ImportFood; ;
            Assert.AreEqual(false, restart.IsParamValid("201"));
        }

        [TestMethod]
        public void IsParamValid_underLowerLimit_nonInteger_fail()
        {
            Command restart = Command.ImportFood; ;
            Assert.AreEqual(false, restart.IsParamValid("0.1"));
        }

        [TestMethod]
        public void IsParamValid_underLowerLimit_integer_fail()
        {
            Command restart = Command.ImportFood; 
            Assert.AreEqual(false, restart.IsParamValid("0"));
        }

        [TestMethod]
        public void IsParamValid_atLowerLimit_pass()
        {
            Command restart = Command.ImportFood; 
            Assert.AreEqual(true, restart.IsParamValid("1"));
        }

        [TestMethod]
        public void IsParamValid_atUpperLimit_pass()
        {
            Command restart = Command.ImportFood; 
            Assert.AreEqual(true, restart.IsParamValid("200"));
        }

        [TestMethod]
        public void IsParamValid_negativeNumber_integer_fail()
        {
            Command restart = Command.ImportFood; 
            Assert.AreEqual(false, restart.IsParamValid("-5"));
        }

        [TestMethod]
        public void IsParamValid_negativeNumber_nonInteger_fail()
        {
            Command restart = Command.ImportFood; ;
            Assert.AreEqual(false, restart.IsParamValid("-5.5"));
        }

    }
}
