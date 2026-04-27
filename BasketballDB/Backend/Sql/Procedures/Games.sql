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
SELECT GameID, HomeTeamID, AwayTeamID, HomeTeamScore,
    AwayTeamScore, CourtNumber, OvertimeCount, Date
FROM Basketball.Games
WHERE GameID = @GameID;
GO

-- Retrieve Games by Team
CREATE OR ALTER PROCEDURE Basketball.RetrieveGamesByTeam
    @TeamID INT
AS
SELECT GameID, HomeTeamID, AwayTeamID, HomeTeamScore,
    AwayTeamScore, CourtNumber, OvertimeCount, Date
FROM Basketball.Games
WHERE HomeTeamID = @TeamID OR AwayTeamID = @TeamID;
GO

-- Retrieve Games by Season
CREATE OR ALTER PROCEDURE Basketball.RetrieveGamesBySeason
    @SeasonID INT
AS
SELECT 
    G.GameID, G.HomeTeamID, G.AwayTeamID, G.HomeTeamScore,
    G.AwayTeamScore, G.CourtNumber, G.OvertimeCount, G.Date,
    HT.TeamName AS HomeTeamName, 
    AT.TeamName AS AwayTeamName
FROM Basketball.Games G
JOIN Basketball.Teams HT ON G.HomeTeamID = HT.TeamID
JOIN Basketball.Teams AT ON G.AwayTeamID = AT.TeamID
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

SELECT GameID, HomeTeamID, AwayTeamID, HomeTeamScore,
    AwayTeamScore, CourtNumber, OvertimeCount, Date
FROM Basketball.Games
WHERE GameID = @GameID;
GO