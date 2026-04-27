public class Game
{
    public int GameID { get; }
    public int HomeTeamID { get; }
    public int AwayTeamID { get; }

    /// <summary>
    /// while not a column in the game table,
    /// we need a place to store the names such that
    /// we don't have to always query for the team names
    /// </summary>
    public string HomeTeamName { get; } 
    public string AwayTeamName { get; } 
    public int HomeTeamScore { get; }
    public int AwayTeamScore { get; }
    public int CourtNumber { get; }
    public int OvertimeCount { get; }
    public DateOnly Date { get; }

    public Game(int gameID, int homeTeamID, int awayTeamID, string homeTeamName,
                string awayTeamName, int homeTeamScore, int awayTeamScore,
                int courtNumber, int overtimeCount, DateOnly date)
    {
        GameID = gameID;
        HomeTeamID = homeTeamID;
        AwayTeamID = awayTeamID;
        HomeTeamName = homeTeamName;
        AwayTeamName = awayTeamName;
        HomeTeamScore = homeTeamScore;
        AwayTeamScore = awayTeamScore;
        CourtNumber = courtNumber;
        OvertimeCount = overtimeCount;
        Date = date;
    }
}