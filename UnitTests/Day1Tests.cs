using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Solutions;

namespace UnitTests
{
    public class Day1Tests
    {
        private IAdventSolution adventDaySolution;
        private string folderName = @"\Day1Examples";
        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataRetriever, TestDataRetriever>()
                .AddSingleton<IAdventSolution, Day1>()
                .BuildServiceProvider(validateScopes: true);
            var scope = serviceProvider.CreateScope();

            adventDaySolution = scope.ServiceProvider.GetRequiredService<IAdventSolution>();
        }

        [Test]
        public void FirstQuestion_ShouldPass_FirstExample()
        {
            var filename = folderName + @"\example1.txt";
            
            var solution = adventDaySolution.FirstQuestion(filename);
            
            solution.Should().Be(142);
        }

        [Test]
        public void SecondQuestion_ShouldPass_SecondExample()
        {
            var filename = folderName + @"\example2.txt";

            var solution = adventDaySolution.SecondQuestion(filename);

            solution.Should().Be(281);
        }

        [Test]
        public void SecondQuestion_ShouldPass_SpecialExamples()
        {
            var filename = folderName + @"\example3.txt";

            var solution = adventDaySolution.SecondQuestion(filename);

            solution.Should().Be(11+38);
        }
    }
}