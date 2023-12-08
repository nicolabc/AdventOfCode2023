using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Solutions;

namespace UnitTests
{
    public class Day4Tests
    {
        private IAdventSolution adventDaySolution;
        private string folderName = @"\Day4Examples";
        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataRetriever, TestDataRetriever>()
                .AddSingleton<IAdventSolution, Day4>()
                .BuildServiceProvider(validateScopes: true);
            var scope = serviceProvider.CreateScope();

            adventDaySolution = scope.ServiceProvider.GetRequiredService<IAdventSolution>();
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example()
        {
            var filename = folderName + @"\example1.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(13);
        }

        [Test]
        public void SecondQuestion_ShouldPass_Example()
        {
            var filename = folderName + @"\example1.txt";

            var solution = adventDaySolution.SecondQuestion(filename);

            solution.Should().Be(30);
        }
    }
}