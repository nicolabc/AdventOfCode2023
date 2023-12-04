using Installer;
using Microsoft.Extensions.DependencyInjection;
using Solutions;

var service = IoCInstaller.GetService();

using (IServiceScope scope = service.CreateScope())
{
    var adventCalendar = scope.ServiceProvider.GetRequiredService<IAdventSolution>();
    Console.WriteLine("Welcome to advent of code");
    Console.WriteLine($"Currently displaying solution to {adventCalendar.GetType().Name}");

    Console.WriteLine($"The solution to the first problem is {adventCalendar.FirstQuestion()}");
    Console.WriteLine($"The solution to the second problem is {adventCalendar.SecondQuestion()}");
}