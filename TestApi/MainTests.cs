using System;
using Xunit;
using FluentCli;

namespace TestApi
{
    public class MainTests
    {
        [Fact]
        public void ExampleTest()
        {
            Assert.Equal(true, true);
        }

        [Fact]
        public void FlagTest()
        {
            var app = App()
                .AddFlag("-t, --true", "retruns true")
                .Run(new string[]{"-t"});
            
            Assert.Equal(app.Is("true"), true);
        }

        [Fact]
        public void NoLongFlagTest()
        {
            Assert.Throws<ArgumentException>(() =>             
                App()
                .AddFlag("-short", "it is short")
                .Run(new string[] { }));

        }

        [Fact]
        public void AddsFlaglessToArguments()
        {
            var app = App()
            .Run(new string[] {"apples"});
            Assert.Equal(app.Arguments()[0], "apples");
        }

        [Fact]
        public void ArgumentsCanBeSeparated(){
            var app = App().AddFlag("-b, --bool", "boolean")
            .Run(new string[] {"apples", "-b", "bananas"});
            //Console.WriteLine(app.Arguments()[1]);
            Console.WriteLine(app.Arguments()[0]);
            Assert.True(app.Arguments().Contains("apples"));
            Assert.True(app.Arguments().Contains("bananas"));
        }
        
        private Program App()
        {
            return new Program();
        }
    }
    

}