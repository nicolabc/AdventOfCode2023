using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Solutions;

namespace UnitTests
{
    public class Day8Tests
    {
        private IAdventSolution adventDaySolution;
        private string folderName = @"\Day8Examples";
        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataRetriever, TestDataRetriever>()
                .AddSingleton<IAdventSolution, Day8>()
                .BuildServiceProvider(validateScopes: true);
            var scope = serviceProvider.CreateScope();

            adventDaySolution = scope.ServiceProvider.GetRequiredService<IAdventSolution>();
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example()
        {
            var filename = folderName + @"\example1.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(2);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example2()
        {
            var filename = folderName + @"\example2.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(6);
        }

        [Test]
        public void SecondQuestion_ShouldPass_Example3()
        {
            var filename = folderName + @"\example3.txt";

            var solution = adventDaySolution.SecondQuestion(filename);

            solution.Should().Be(6);
        }

        [TestCase(48, 18, 6)]
        [TestCase(18, 48, 6)]
        [TestCase(-18, -48, 6)]
        public void GreatesCommonDivisor_ShouldPass(long a, long b, long expected)
        {
            var actual = new Day8(A.Fake<IDataRetriever>()).GetGreatestCommonDivsor(a, b);

            actual.Should().Be(expected);
        }

        [TestCase(21, 6, 42)]
        [TestCase(6, 21, 42)]
        [TestCase(-6, -21, 42)]
        public void LeastCommonMultiple_ShouldPass(long a, long b, long expected)
        {
            var actual = new Day8(A.Fake<IDataRetriever>()).GetLeastCommonMultiple(a, b);

            actual.Should().Be(expected);
        }
    }
}