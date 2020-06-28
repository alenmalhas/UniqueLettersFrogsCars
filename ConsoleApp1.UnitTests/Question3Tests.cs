using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleApp1.UnitTests
{
    public class Question3Tests
    {
        private readonly Question3 _sut;

        public Question3Tests()
        {
            _sut = new Question3();
        }

        [Theory]
        [InlineData(new[] { 2, 8, 4, 3, 2 }, 7, 11, 3, 8)]
        [InlineData(new[] { 5 }, 4, 0, 3, -1)]
        public void Task2_Test1(int[] input, int X, int Y, int Z, int output)
        {
            var result = _sut.Solution(input, X, Y, Z);

            result.Should().Be(output);
        }
    }
}
