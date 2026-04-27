-- Create PlayerGameStats
CREATE OR ALTER PROCEDURE Basketball.CreatePlayerGameStats
    @PlayerID INT,
    @GameID INT,
    @TeamID INT,
    @Points INT = 0,
    @PlayingTime INT = 0,
    @Turnovers INT = 0,
    @Rebounds INT = 0,
    @Assists INT = 0
AS
INSERT Basketball.PlayerGameStats(PlayerID, GameID, TeamID, Points, 
    PlayingTime, Turnovers, Rebounds, Assists)
VALUES(@PlayerID, @GameID, @TeamID, @Points, 
    @PlayingTime, @Turnovers, @Rebounds, @Assists);
GO

-- Fetch PlayerGameStats by PlayerID and GameID
CREATE OR ALTER PROCEDURE Basketball.FetchPlayerGameStats
    @PlayerID INT,
    @GameID INT
AS
SELECT PlayerID, GameID, TeamID, Points, PlayingTime, 
    Turnovers, Rebounds, Assists
FROM Basketball.PlayerGameStats
WHERE PlayerID = @PlayerID AND GameID = @GameID;
GO

-- Retrieve Stats by Game
CREATE OR ALTER PROCEDURE Basketball.RetrieveStatsByGame
    @GameID INT
AS
SELECT PlayerID, GameID, TeamID, Points, PlayingTime,
    Turnovers, Rebounds, Assists
FROM Basketball.PlayerGameStats
WHERE GameID = @GameID;
GO

-- Retrieve Stats by Player
CREATE OR ALTER PROCEDURE Basketball.RetrieveStatsByPlayer
    @PlayerID INT
AS
SELECT PlayerID, GameID, TeamID, Points, PlayingTime,
    Turnovers, Rebounds, Assists
FROM Basketball.PlayerGameStats
WHERE PlayerID = @PlayerID;
GO

-- Update PlayerGameStats
CREATE OR ALTER PROCEDURE Basketball.UpdatePlayerGameStats
    @PlayerID INT,
    @GameID INT,
    @Points INT,
    @PlayingTime INT,
    @Turnovers INT,
    @Rebounds INT,
    @Assists INT
AS
UPDATE Basketball.PlayerGameStats
SET Points = @Points,
    PlayingTime = @PlayingTime,
    Turnovers = @Turnovers,
    Rebounds = @Rebounds,
    Assists = @Assists
WHERE PlayerID = @PlayerID AND GameID = @GameID;

SELECT PlayerID, GameID, TeamID, Points, PlayingTime,
    Turnovers, Rebounds, Assists
FROM Basketball.PlayerGameStats
WHERE PlayerID = @PlayerID AND GameID = @GameID;
GO

-- Delete PlayerGameStats
CREATE OR ALTER PROCEDURE Basketball.DeletePlayerGameStats
    @PlayerID INT,
    @GameID INT
AS
DELETE FROM Basketball.PlayerGameStats
WHERE PlayerID = @PlayerID AND GameID = @GameID;
GO