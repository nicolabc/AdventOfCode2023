using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Solutions;

namespace UnitTests
{
    public class Day15Tests
    {
        private IAdventSolution adventDaySolution;
        private string folderName = @"\Day15Examples";
        
        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataRetriever, TestDataRetriever>()
                .AddSingleton<IAdventSolution, Day15>()
                .BuildServiceProvider(validateScopes: true);
            var scope = serviceProvider.CreateScope();

            adventDaySolution = scope.ServiceProvider.GetRequiredService<IAdventSolution>();
        }

        [TestCase("HASH")]
        public void HashString_ShouldPass_Example(string stringToHash)
        {
            var day15 = new Day15(A.Fake<IDataRetriever>());

            var value = day15.HashString(stringToHash);

            value.Should().Be(52);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example()
        {
            var filename = folderName + @"\example1.txt";
            
            var solution = adventDaySolution.FirstQuestion(filename);
            
            solution.Should().Be(1320);
        }


        [Test]
        public void SecondQuestion_ShouldPass_Example()
        {
            var filename = folderName + @"\example1.txt";

            var solution = adventDaySolution.SecondQuestion(filename);

            solution.Should().Be(145);
        }
    }
}