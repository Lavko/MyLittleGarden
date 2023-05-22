using Domain.Entities;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface IActionRuleRepository : IGenericRepository<ActionRule>
{
    Task<IList<ActionRule>> GetAllSchedulesAsync(CancellationToken cancellationToken = default);
}

public interface IMessenger
{
    void SendMessage(string text);
}
public class Messenger : IMessenger
{
    public void SendMessage(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            Console.WriteLine("Text is missing :(");
        }
        else
        {
            Console.WriteLine($"We got the text: {text}");
        }
    }
}

public class MyClass
{
    private readonly IMessenger _messenger;

    public MyClass(IMessenger messenger)
    {
        _messenger = messenger;
    }

    public void DoSomething()
    {
        _messenger.SendMessage("I'm doing something");
    }
}