using FluentAssertions;
using System;
using Xunit;

namespace ConsoleApp1.UnitTests
{
    public class Question1Tests
    {
        private readonly Question1 _sut;

        public Question1Tests()
        {
            _sut = new Question1();
        }

        [Theory]
        [InlineData("eeee", 0)]
        [InlineData("aaaabbbb", 1)]
        [InlineData("ccaaffddecee", 6)]
        [InlineData("example", 4)]
        public void Task1_Test1(string input, int output)
        {
            var result = _sut.Task1(input);

            result.Should().Be(output);
        }
    }
}
