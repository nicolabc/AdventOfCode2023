using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Solutions;

namespace UnitTests
{
    public class Day10Tests
    {
        private IAdventSolution adventDaySolution;
        private string folderName = @"\Day10Examples";
        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataRetriever, TestDataRetriever>()
                .AddSingleton<IAdventSolution, Day10>()
                .BuildServiceProvider(validateScopes: true);
            var scope = serviceProvider.CreateScope();

            adventDaySolution = scope.ServiceProvider.GetRequiredService<IAdventSolution>();
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example()
        {
            var filename = folderName + @"\example1.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(4);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example2()
        {
            var filename = folderName + @"\example2.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(8);
        }
    }
}