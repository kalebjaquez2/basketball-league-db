-- Create PlayerGameStats
CREATE OR ALTER PROCEDURE Basketball.CreatePlayerGameStats
    @PlayerID INT, @GameID INT, @TeamID INT,
    @PlayingTime INT = 0, @Turnovers INT = 0,
    @Rebounds INT = 0, @Assists INT = 0, @Steals INT = 0, @Blocks INT = 0
AS
INSERT Basketball.PlayerGameStats(PlayerID, GameID, TeamID, 
    PlayingTime, Turnovers, Rebounds, Assists, Steals, Blocks)
VALUES(@PlayerID, @GameID, @TeamID, 
    @PlayingTime, @Turnovers, @Rebounds, @Assists, @Steals, @Blocks);
GO

-- Fetch PlayerGameStats by PlayerID and GameID
CREATE OR ALTER PROCEDURE Basketball.FetchPlayerGameStats
    @PlayerID INT,
    @GameID INT
AS
BEGIN
    SELECT 
        S.PlayerID, 
        S.GameID, 
        S.TeamID, 
        S.PlayingTime, 
        S.Turnovers, 
        S.Rebounds, 
        S.Assists, 
        S.Steals, 
        S.Blocks,
        S.FieldGoalsMade,
        S.FieldGoalsTaken,
        S.ThreePointersMade,
        S.ThreePointersTaken,
        S.PersonalFouls, 
        (P.FirstName + N' ' + P.LastName) AS PlayerName
    FROM Basketball.PlayerGameStats S
    INNER JOIN Basketball.Players P ON S.PlayerID = P.PlayerID
    WHERE S.PlayerID = @PlayerID AND S.GameID = @GameID;
END;
GO
-- Get the player stats by game
CREATE OR ALTER PROCEDURE Basketball.RetrieveStatsByGame
    @GameID INT
AS
BEGIN
    SELECT 
        S.PlayerID, S.GameID, S.TeamID, S.PlayingTime, 
        S.Rebounds, S.Assists, S.Steals, S.Blocks, S.Turnovers,
        S.FieldGoalsMade, S.FieldGoalsTaken, 
        S.ThreePointersMade, S.ThreePointersTaken,
        S.PersonalFouls,
        P.FirstName + ' ' + P.LastName AS PlayerName,
        P.JerseyNumber, 
        P.Position     
    FROM Basketball.PlayerGameStats S
    INNER JOIN Basketball.Players P ON S.PlayerID = P.PlayerID
    WHERE S.GameID = @GameID;
END;
GO

-- Retrieve Stats by Player
CREATE OR ALTER PROCEDURE Basketball.RetrieveStatsByPlayer
    @PlayerID INT
AS
SELECT S.PlayerID, S.GameID, S.TeamID, S.PlayingTime,
       S.Turnovers, S.Rebounds, S.Assists, S.Steals, S.Blocks,
       (P.FirstName + ' ' + P.LastName) AS PlayerName
FROM Basketball.PlayerGameStats S
INNER JOIN Basketball.Players P ON S.PlayerID = P.PlayerID
WHERE S.PlayerID = @PlayerID;
GO

-- Update PlayerGameStats
CREATE OR ALTER PROCEDURE Basketball.UpdatePlayerGameStats
    @PlayerID INT, @GameID INT,
    @PlayingTime INT, @Turnovers INT,
    @Rebounds INT, @Assists INT, @Steals INT, @Blocks INT,
    @FieldGoalsMade INT = 0, @FieldGoalsTaken INT = 0,
    @ThreePointersMade INT = 0, @ThreePointersTaken INT = 0,
    @PersonalFouls INT = 0
AS
UPDATE Basketball.PlayerGameStats
SET
    PlayingTime = @PlayingTime,
    Turnovers = @Turnovers,
    Rebounds = @Rebounds,
    Assists = @Assists,
    Steals = @Steals,
    Blocks = @Blocks,
    FieldGoalsMade = @FieldGoalsMade,
    FieldGoalsTaken = @FieldGoalsTaken,
    ThreePointersMade = @ThreePointersMade,
    ThreePointersTaken = @ThreePointersTaken,
    PersonalFouls = @PersonalFouls
WHERE PlayerID = @PlayerID AND GameID = @GameID;

SELECT
    S.PlayerID, S.GameID, S.TeamID, S.PlayingTime,
    S.Turnovers, S.Rebounds, S.Assists, S.Steals, S.Blocks,
    S.FieldGoalsMade, S.FieldGoalsTaken,
    S.ThreePointersMade, S.ThreePointersTaken,
    S.PersonalFouls,
    P.FirstName + ' ' + P.LastName AS PlayerName
FROM Basketball.PlayerGameStats S
INNER JOIN Basketball.Players P ON S.PlayerID = P.PlayerID
WHERE S.PlayerID = @PlayerID AND S.GameID = @GameID;
GO

-- Delete PlayerGameStats
CREATE OR ALTER PROCEDURE Basketball.DeletePlayerGameStats
    @PlayerID INT,
    @GameID INT
AS
DELETE FROM Basketball.PlayerGameStats
WHERE PlayerID = @PlayerID AND GameID = @GameID;
GO