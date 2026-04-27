-- Create Game
CREATE OR ALTER PROCEDURE Basketball.CreateGame
    @HomeTeamID INT,
    @AwayTeamID INT,
    @HomeTeamScore INT,
    @AwayTeamScore INT,
    @CourtNumber INT,
    @OvertimeCount INT,
    @Date DATE,
    @GameID INT OUTPUT
AS
INSERT Basketball.Games(HomeTeamID, AwayTeamID, HomeTeamScore, 
    AwayTeamScore, CourtNumber, OvertimeCount, Date)
VALUES(@HomeTeamID, @AwayTeamID, @HomeTeamScore, 
    @AwayTeamScore, @CourtNumber, @OvertimeCount, @Date);
SET @GameID = SCOPE_IDENTITY();
GO

-- Fetch Game by ID
CREATE OR ALTER PROCEDURE Basketball.FetchGame
    @GameID INT
AS
SELECT G.GameID, G.HomeTeamID, G.AwayTeamID, G.HomeTeamScore,
    G.AwayTeamScore, G.CourtNumber, G.OvertimeCount, G.Date,
    HT.TeamName AS HomeTeamName,
    AT.TeamName AS AwayTeamName
FROM Basketball.Games G
    INNER JOIN Basketball.Teams HT ON HT.TeamID = G.HomeTeamID
    INNER JOIN Basketball.Teams AT ON AT.TeamID = G.AwayTeamID
WHERE G.GameID = @GameID;
GO

-- Retrieve Games by Team
CREATE OR ALTER PROCEDURE Basketball.RetrieveGamesByTeam
    @TeamID INT
AS
SELECT G.GameID, G.HomeTeamID, G.AwayTeamID, G.HomeTeamScore,
    G.AwayTeamScore, G.CourtNumber, G.OvertimeCount, G.Date,
    HT.TeamName AS HomeTeamName,
    AT.TeamName AS AwayTeamName
FROM Basketball.Games G
    INNER JOIN Basketball.Teams HT ON HT.TeamID = G.HomeTeamID
    INNER JOIN Basketball.Teams AT ON AT.TeamID = G.AwayTeamID
WHERE G.HomeTeamID = @TeamID OR G.AwayTeamID = @TeamID;
GO

-- Retrieve Games by Season
CREATE OR ALTER PROCEDURE Basketball.RetrieveGamesBySeason
    @SeasonID INT
AS
SELECT G.GameID, G.HomeTeamID, G.AwayTeamID, G.HomeTeamScore,
    G.AwayTeamScore, G.CourtNumber, G.OvertimeCount, G.Date,
    HT.TeamName AS HomeTeamName,
    AT.TeamName AS AwayTeamName
FROM Basketball.Games G
    INNER JOIN Basketball.Teams HT ON HT.TeamID = G.HomeTeamID
    INNER JOIN Basketball.Teams AT ON AT.TeamID = G.AwayTeamID
WHERE HT.SeasonID = @SeasonID
ORDER BY G.Date ASC;
GO

-- Update Game
CREATE OR ALTER PROCEDURE Basketball.UpdateGame
    @GameID INT,
    @HomeTeamScore INT,
    @AwayTeamScore INT,
    @OvertimeCount INT
AS
UPDATE Basketball.Games
SET HomeTeamScore = @HomeTeamScore,
    AwayTeamScore = @AwayTeamScore,
    OvertimeCount = @OvertimeCount
WHERE GameID = @GameID;

SELECT G.GameID, G.HomeTeamID, G.AwayTeamID, G.HomeTeamScore,
    G.AwayTeamScore, G.CourtNumber, G.OvertimeCount, G.Date,
    HT.TeamName AS HomeTeamName,
    AT.TeamName AS AwayTeamName
FROM Basketball.Games G
    INNER JOIN Basketball.Teams HT ON HT.TeamID = G.HomeTeamID
    INNER JOIN Basketball.Teams AT ON AT.TeamID = G.AwayTeamID
WHERE G.GameID = @GameID;
GO