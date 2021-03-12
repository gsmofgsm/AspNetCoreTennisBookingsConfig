namespace TennisBookings.Web.Services
{
    public interface IGreetingService
    {
        string GreetingColour { get; }

        string GetRandomGreeting();

        string GetRandomLoginGreeting(string name);
    }
}
