using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Solutions;

namespace UnitTests
{
    public class Day5Tests
    {
        private IAdventSolution adventDaySolution;
        private string folderName = @"\Day5Examples";
        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataRetriever, TestDataRetriever>()
                .AddSingleton<IAdventSolution, Day5>()
                .BuildServiceProvider(validateScopes: true);
            var scope = serviceProvider.CreateScope();

            adventDaySolution = scope.ServiceProvider.GetRequiredService<IAdventSolution>();
        }

        [Test]
        public void FirstQuestion_ShouldPass_FirstExample()
        {
            var filename = folderName + @"\example1.txt";
            
            var solution = adventDaySolution.FirstQuestion(filename);
            
            solution.Should().Be(35);
        }
    }
}