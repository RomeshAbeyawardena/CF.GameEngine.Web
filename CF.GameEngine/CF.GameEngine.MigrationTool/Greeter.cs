namespace CF.GameEngine.MigrationTool;

internal interface ITimeOfDayProvider
{
    string GetTimeOfDay();
}

internal class TimeOfDayProvider(TimeProvider timeProvider) : ITimeOfDayProvider
{
    public string GetTimeOfDay()
    {
        var currentHour = timeProvider.GetLocalNow().Hour;
        if (currentHour < 12)
        {
            return "Good morning!";
        }
        else if (currentHour < 18)
        {
            return "Good afternoon!";
        }
        else
        {
            return "Good evening!";
        }
    }
}
