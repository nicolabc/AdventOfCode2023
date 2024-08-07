using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Solutions;

namespace UnitTests
{
    public class Day16Tests
    {
        private IAdventSolution adventDaySolution;
        private string folderName = @"\Day16Examples";
        
        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataRetriever, TestDataRetriever>()
                .AddSingleton<IAdventSolution, Day16>()
                .BuildServiceProvider(validateScopes: true);
            var scope = serviceProvider.CreateScope();

            adventDaySolution = scope.ServiceProvider.GetRequiredService<IAdventSolution>();
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example()
        {
            var filename = folderName + @"\example1.txt";
            
            var solution = adventDaySolution.FirstQuestion(filename);
            
            solution.Should().Be(46);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example2()
        {
            var filename = folderName + @"\example2.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(5);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example3()
        {
            var filename = folderName + @"\example3.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(41);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example4()
        {
            var filename = folderName + @"\example4.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(16);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example5()
        {
            var filename = folderName + @"\example5.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(18);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example6()
        {
            var filename = folderName + @"\example6.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(9);
        }

        [Test]
        public void FirstQuestion_ShouldPass_Example7()
        {
            var filename = folderName + @"\example7.txt";

            var solution = adventDaySolution.FirstQuestion(filename);

            solution.Should().Be(101);
        }

        [Test]
        public void SecondQuestion_ShouldPass_Example()
        {
            var filename = folderName + @"\example1.txt";

            var solution = adventDaySolution.SecondQuestion(filename);

            solution.Should().Be(51);
        }
    }
}