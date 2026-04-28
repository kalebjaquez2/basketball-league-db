/*******************************************************
 * Queries.sql
 * Aggregating Queries for Basketball League
 *******************************************************/

/****************************
 * Query 1: Most Active Players
 * Show players ranked by total games played and
 * average points per game for a given season
 * Parameters: @SeasonID
 ****************************/
DECLARE @SeasonID INT = 1;

SELECT
    P.PlayerID,
    P.FirstName + N' ' + P.LastName AS PlayerName,
    P.TeamID,
    COUNT(PGS.GameID) AS GamesPlayed,
    SUM(PGS.Points) AS TotalPoints,
    CAST(AVG(CAST(PGS.Points AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS PointsPerGame,
    RANK() OVER(
        ORDER BY COUNT(PGS.GameID) DESC, 
        AVG(CAST(PGS.Points AS DECIMAL(10,2))) DESC
    ) AS SeasonRank
FROM Basketball.Players P
    INNER JOIN Basketball.Teams T ON T.TeamID = P.TeamID
    INNER JOIN Basketball.PlayerGameStats PGS ON PGS.PlayerID = P.PlayerID
WHERE T.SeasonID = @SeasonID
GROUP BY P.PlayerID, P.FirstName, P.LastName, P.TeamID
ORDER BY SeasonRank ASC;
GO

/****************************
 * Query 2: Top Scorers Per Team
 * Show each player's total points, average points
 * per game, and rank them within their team
 * Parameters: @SeasonID
 ****************************/
DECLARE @SeasonID INT = 1;

SELECT
    P.TeamID,
    P.PlayerID,
    P.FirstName + N' ' + P.LastName AS PlayerName,
    COUNT(PGS.GameID) AS GamesPlayed,
    SUM(PGS.Points) AS TotalPoints,
    CAST(AVG(CAST(PGS.Points AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AveragePointsPerGame,
    RANK() OVER(
        PARTITION BY P.TeamID
        ORDER BY SUM(PGS.Points) DESC
    ) AS TeamRank
FROM Basketball.Players P
    INNER JOIN Basketball.Teams T ON T.TeamID = P.TeamID
    INNER JOIN Basketball.PlayerGameStats PGS ON PGS.PlayerID = P.PlayerID
WHERE T.SeasonID = @SeasonID
GROUP BY P.TeamID, P.PlayerID, P.FirstName, P.LastName
ORDER BY P.TeamID ASC, TeamRank ASC;
GO

/****************************
 * Query 3: Team Performance In A Season
 * Show each team's total wins, losses, and
 * average score per game for a given season
 * Parameters: @SeasonID
 ****************************/
DECLARE @SeasonID INT = 1;

WITH TeamGames(TeamID, TeamName, IsWin, Score) AS
(
    -- Home games
    SELECT
        T.TeamID,
        T.TeamName,
        CASE WHEN G.HomeTeamScore > G.AwayTeamScore THEN 1 ELSE 0 END AS IsWin,
        G.HomeTeamScore AS Score
    FROM Basketball.Teams T
        INNER JOIN Basketball.Games G ON G.HomeTeamID = T.TeamID
    WHERE T.SeasonID = @SeasonID

    UNION ALL

    -- Away games
    SELECT
        T.TeamID,
        T.TeamName,
        CASE WHEN G.AwayTeamScore > G.HomeTeamScore THEN 1 ELSE 0 END AS IsWin,
        G.AwayTeamScore AS Score
    FROM Basketball.Teams T
        INNER JOIN Basketball.Games G ON G.AwayTeamID = T.TeamID
    WHERE T.SeasonID = @SeasonID
)
SELECT
    TeamID,
    TeamName,
    SUM(IsWin) AS Wins,
    SUM(1 - IsWin) AS Losses,
    CAST(AVG(CAST(Score AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AverageScorePerGame
FROM TeamGames
GROUP BY TeamID, TeamName
ORDER BY Wins DESC, AverageScorePerGame DESC;
GO

/****************************
 * Query 4: Game Stats Summary
 * Show average team performance stats per game
 * for a given date range including average points,
 * rebounds, assists, and turnovers across all
 * players in each game
 * Parameters: @StartDate, @EndDate
 ****************************/
DECLARE @StartDate DATE = '2022-09-01';
DECLARE @EndDate DATE = '2022-12-31';

SELECT
    G.GameID,
    G.Date AS GameDate,
    HT.TeamName AS HomeTeam,
    AT.TeamName AS AwayTeam,
    CAST(AVG(CAST(PGS.Points AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AveragePoints,
    CAST(AVG(CAST(PGS.Rebounds AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AverageRebounds,
    CAST(AVG(CAST(PGS.Assists AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AverageAssists,
    CAST(AVG(CAST(PGS.Turnovers AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AverageTurnovers
FROM Basketball.Games G
    INNER JOIN Basketball.Teams HT ON HT.TeamID = G.HomeTeamID
    INNER JOIN Basketball.Teams AT ON AT.TeamID = G.AwayTeamID
    INNER JOIN Basketball.PlayerGameStats PGS ON PGS.GameID = G.GameID
WHERE G.Date BETWEEN @StartDate AND @EndDate
GROUP BY G.GameID, G.Date, HT.TeamName, AT.TeamName
ORDER BY G.Date ASC;
GO