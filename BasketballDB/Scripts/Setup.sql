/*******************************************************
 * Setup.sql
 * Run this file to create and seed the entire
 * BasketballLeague560 database from scratch.
 *******************************************************/

USE master;
GO

IF DB_ID(N'BasketballLeague560') IS NULL
    CREATE DATABASE BasketballLeague560;
GO

USE BasketballLeague560;
GO

DROP TABLE IF EXISTS Basketball.PlayerGameStats;
DROP TABLE IF EXISTS Basketball.Players;
DROP TABLE IF EXISTS Basketball.Games;
DROP TABLE IF EXISTS Basketball.Teams;
DROP TABLE IF EXISTS Basketball.Seasons;
DROP TABLE IF EXISTS Basketball.League;
DROP TABLE IF EXISTS Basketball.Location;
DROP TABLE IF EXISTS Basketball.Users;

/****************************
 * Create Schema
 ****************************/
IF SCHEMA_ID(N'Basketball') IS NULL
    EXEC(N'CREATE SCHEMA [Basketball];');
GO

/****************************
 * 1. Location
 ****************************/
IF OBJECT_ID(N'Basketball.Location') IS NULL
BEGIN
    CREATE TABLE Basketball.Location
    (
        LocationID INT NOT NULL IDENTITY(1,1),
        City NVARCHAR(64) NOT NULL,
        State NVARCHAR(64) NOT NULL,
        Country NVARCHAR(64) NOT NULL,
        CONSTRAINT PK_Basketball_Location_LocationID PRIMARY KEY CLUSTERED
        (
            LocationID ASC
        )
    );
END;

IF NOT EXISTS
    (
        SELECT * FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.Location')
          AND kc.[name] = N'UK_Basketball_Location_City_State_Country'
    )
BEGIN
    ALTER TABLE Basketball.Location
    ADD CONSTRAINT [UK_Basketball_Location_City_State_Country] UNIQUE NONCLUSTERED
    (
        City ASC, State ASC, Country ASC
    );
END;
GO

/****************************
 * 2. League
 ****************************/
IF OBJECT_ID(N'Basketball.League') IS NULL
BEGIN
    CREATE TABLE Basketball.League
    (
        LeagueID INT NOT NULL IDENTITY(1,1),
        LeagueName NVARCHAR(64) NOT NULL,
        LocationID INT NOT NULL,
        CONSTRAINT PK_Basketball_League_LeagueID PRIMARY KEY CLUSTERED
        (
            LeagueID ASC
        ),
        CONSTRAINT FK_Basketball_League_LocationID_Basketball_Location
            FOREIGN KEY (LocationID)
            REFERENCES Basketball.Location(LocationID)
    );
END;

IF NOT EXISTS
    (
        SELECT * FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.League')
          AND kc.[name] = N'UK_Basketball_League_LeagueName'
    )
BEGIN
    ALTER TABLE Basketball.League
    ADD CONSTRAINT [UK_Basketball_League_LeagueName] UNIQUE NONCLUSTERED
    (
        LeagueName ASC
    );
END;
GO

/****************************
 * 3. Seasons
 ****************************/
IF OBJECT_ID(N'Basketball.Seasons') IS NULL
BEGIN
    CREATE TABLE Basketball.Seasons
    (
        SeasonID INT NOT NULL IDENTITY(1,1),
        LeagueID INT NOT NULL,
        StartDate DATE NOT NULL,
        EndDate DATE NOT NULL,
        CONSTRAINT PK_Basketball_Seasons_SeasonID PRIMARY KEY CLUSTERED
        (
            SeasonID ASC
        ),
        CONSTRAINT FK_Basketball_Seasons_LeagueID_Basketball_League
            FOREIGN KEY (LeagueID)
            REFERENCES Basketball.League(LeagueID)
    );
END;


IF NOT EXISTS
    (
        SELECT * FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.Seasons')
          AND kc.[name] = N'UK_Basketball_Seasons_EndDate'
    )
BEGIN
    ALTER TABLE Basketball.Seasons
    ADD CONSTRAINT [UK_Basketball_Seasons_EndDate] UNIQUE NONCLUSTERED (EndDate ASC);
END;
GO

/****************************
 * 4. Teams
 ****************************/
IF OBJECT_ID(N'Basketball.Teams') IS NULL
BEGIN
    CREATE TABLE Basketball.Teams
    (
        TeamID INT NOT NULL IDENTITY(1,1),
        SeasonID INT NOT NULL,
        TeamName NVARCHAR(64) NOT NULL,
        CONSTRAINT PK_Basketball_Teams_TeamID PRIMARY KEY CLUSTERED
        (
            TeamID ASC
        ),
        CONSTRAINT FK_Basketball_Teams_SeasonID_Basketball_Seasons
            FOREIGN KEY (SeasonID)
            REFERENCES Basketball.Seasons(SeasonID)
    );
END;

IF NOT EXISTS
    (
        SELECT * FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.Teams')
          AND kc.[name] = N'UK_Basketball_Teams_TeamName'
    )
BEGIN
    ALTER TABLE Basketball.Teams
    ADD CONSTRAINT [UK_Basketball_Teams_TeamName] UNIQUE NONCLUSTERED (TeamName ASC);
END;
GO

/****************************
 * 5. Games
 ****************************/
IF OBJECT_ID(N'Basketball.Games') IS NULL
BEGIN
    CREATE TABLE Basketball.Games
    (
        GameID INT NOT NULL IDENTITY(1,1),
        HomeTeamID INT NOT NULL,
        AwayTeamID INT NOT NULL,
        HomeTeamScore INT NOT NULL,
        AwayTeamScore INT NOT NULL,
        CourtNumber INT NOT NULL,
        OvertimeCount INT NOT NULL CONSTRAINT DF_Basketball_Games_OvertimeCount DEFAULT(0),
        Date DATE NOT NULL,
        CONSTRAINT PK_Basketball_Games_GameID PRIMARY KEY CLUSTERED
        (
            GameID ASC
        ),
        CONSTRAINT FK_Basketball_Games_HomeTeamID_Basketball_Teams
            FOREIGN KEY (HomeTeamID) REFERENCES Basketball.Teams(TeamID),
        CONSTRAINT FK_Basketball_Games_AwayTeamID_Basketball_Teams
            FOREIGN KEY (AwayTeamID) REFERENCES Basketball.Teams(TeamID)
    );
END;
GO

/****************************
 * 6. Players
 ****************************/
IF OBJECT_ID(N'Basketball.Players') IS NULL
BEGIN
    CREATE TABLE Basketball.Players
    (
        PlayerID INT NOT NULL IDENTITY(1,1),
        TeamID INT NOT NULL,
        JerseyNumber INT NOT NULL,
        FirstName NVARCHAR(32) NOT NULL,
        LastName NVARCHAR(32) NOT NULL,
        Age INT NULL,
        Height NVARCHAR(16) NULL,
        Weight INT NULL,
        Position NVARCHAR(2) NULL,

        CONSTRAINT PK_Basketball_Players_PlayerID PRIMARY KEY CLUSTERED (PlayerID ASC),

        CONSTRAINT FK_Basketball_Players_TeamID
            FOREIGN KEY (TeamID)
            REFERENCES Basketball.Teams(TeamID)
    );
END;

/****************************
 * 7. PlayerGameStats
 ****************************/
IF OBJECT_ID(N'Basketball.PlayerGameStats') IS NULL
BEGIN
    CREATE TABLE Basketball.PlayerGameStats
    (
        PlayerID INT NOT NULL,
        GameID INT NOT NULL,
        TeamID INT NOT NULL,

        PlayingTime INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_PlayingTime DEFAULT(0),

        Turnovers INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_Turnovers DEFAULT(0),

        Rebounds INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_Rebounds DEFAULT(0),

        Assists INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_Assists DEFAULT(0),

        Steals INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_Steals DEFAULT(0),

        Blocks INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_Blocks DEFAULT(0),

        FieldGoalsMade INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_FieldGoalsMade DEFAULT(0),

        FieldGoalsTaken INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_FieldGoalsTaken DEFAULT(0),

        ThreePointersMade INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_ThreePointersMade DEFAULT(0),

        ThreePointersTaken INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_ThreePointersTaken DEFAULT(0),

        PersonalFouls INT NOT NULL 
            CONSTRAINT DF_Basketball_PlayerGameStats_PersonalFouls DEFAULT(0),

        CONSTRAINT PK_Basketball_PlayerGameStats 
            PRIMARY KEY CLUSTERED (PlayerID ASC, GameID ASC),

        CONSTRAINT FK_Basketball_PlayerGameStats_PlayerID_Basketball_Players
            FOREIGN KEY (PlayerID)
            REFERENCES Basketball.Players(PlayerID),

        CONSTRAINT FK_Basketball_PlayerGameStats_GameID_Basketball_Games
            FOREIGN KEY (GameID)
            REFERENCES Basketball.Games(GameID),

        CONSTRAINT FK_Basketball_PlayerGameStats_TeamID_Basketball_Teams
            FOREIGN KEY (TeamID)
            REFERENCES Basketball.Teams(TeamID),

        CONSTRAINT CK_PlayerGameStats_FieldGoals_Valid
            CHECK (FieldGoalsMade <= FieldGoalsTaken),

        CONSTRAINT CK_PlayerGameStats_ThreePointers_Valid
            CHECK (ThreePointersMade <= ThreePointersTaken),

        CONSTRAINT CK_PlayerGameStats_NonNegative
            CHECK (
                PlayingTime >= 0 AND
                Turnovers >= 0 AND
                Rebounds >= 0 AND
                Assists >= 0 AND
                Steals >= 0 AND
                Blocks >= 0 AND
                FieldGoalsMade >= 0 AND
                FieldGoalsTaken >= 0 AND
                ThreePointersMade >= 0 AND
                ThreePointersTaken >= 0 AND
                PersonalFouls >= 0
            )
    );
END;
GO

/*******************************************************
 * STORED PROCEDURES
 *******************************************************/

/****************************
 * Location Procedures
 ****************************/
CREATE OR ALTER PROCEDURE Basketball.CreateLocation
    @City NVARCHAR(64),
    @State NVARCHAR(64),
    @Country NVARCHAR(64),
    @LocationID INT OUTPUT
AS
INSERT Basketball.Location(City, State, Country)
VALUES(@City, @State, @Country);
SET @LocationID = SCOPE_IDENTITY();
GO

CREATE OR ALTER PROCEDURE Basketball.FetchLocation
    @LocationID INT
AS
SELECT LocationID, City, State, Country
FROM Basketball.Location
WHERE LocationID = @LocationID;
GO

CREATE OR ALTER PROCEDURE Basketball.RetrieveLocations
AS
SELECT LocationID, City, State, Country
FROM Basketball.Location;
GO

CREATE OR ALTER PROCEDURE Basketball.UpdateLocation
    @LocationID INT,
    @City NVARCHAR(64),
    @State NVARCHAR(64),
    @Country NVARCHAR(64)
AS
UPDATE Basketball.Location
SET City = @City,
    State = @State,
    Country = @Country
WHERE LocationID = @LocationID;

SELECT LocationID, City, State, Country
FROM Basketball.Location
WHERE LocationID = @LocationID;
GO

/****************************
 * League Procedures
 ****************************/
CREATE OR ALTER PROCEDURE Basketball.CreateLeague
    @LeagueName NVARCHAR(64),
    @LocationID INT,
    @LeagueID INT OUTPUT
AS
BEGIN
    INSERT Basketball.League(LeagueName, LocationID)
    VALUES(@LeagueName, @LocationID);
    SET @LeagueID = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE Basketball.RetrieveLeagues
AS
BEGIN
    SELECT L.LeagueID, L.LeagueName,
           (Loc.City + N', ' + Loc.State) AS [Location],
           L.LocationID
    FROM Basketball.League L
    INNER JOIN Basketball.Location Loc ON L.LocationID = Loc.LocationID;
END
GO

CREATE OR ALTER PROCEDURE Basketball.FetchLeague
    @LeagueID INT
AS
BEGIN
    SELECT L.LeagueID, L.LeagueName,
           (Loc.City + N', ' + Loc.State) AS [Location],
           L.LocationID
    FROM Basketball.League L
    INNER JOIN Basketball.Location Loc ON L.LocationID = Loc.LocationID
    WHERE L.LeagueID = @LeagueID;
END
GO

CREATE OR ALTER PROCEDURE Basketball.UpdateLeague
    @LeagueID INT,
    @LeagueName NVARCHAR(64),
    @LocationID INT
AS
BEGIN
    UPDATE Basketball.League
    SET LeagueName = @LeagueName,
        LocationID = @LocationID
    WHERE LeagueID = @LeagueID;
    EXEC Basketball.FetchLeague @LeagueID;
END
GO

CREATE OR ALTER PROCEDURE Basketball.DeleteLeague
    @LeagueID INT
AS
BEGIN
    DELETE FROM Basketball.League
    WHERE LeagueID = @LeagueID;
END
GO

/****************************
 * Season Procedures
 ****************************/
CREATE OR ALTER PROCEDURE Basketball.CreateSeason
    @LeagueID INT,
    @StartDate DATE,
    @EndDate DATE,
    @SeasonID INT OUTPUT
AS
INSERT Basketball.Seasons(LeagueID, StartDate, EndDate)
VALUES(@LeagueID, @StartDate, @EndDate);
SET @SeasonID = SCOPE_IDENTITY();
GO

CREATE OR ALTER PROCEDURE Basketball.FetchSeason
    @SeasonID INT
AS
SELECT SeasonID, LeagueID, StartDate, EndDate
FROM Basketball.Seasons
WHERE SeasonID = @SeasonID;
GO

CREATE OR ALTER PROCEDURE Basketball.RetrieveSeasonsByLeague
    @LeagueID INT
AS
SELECT SeasonID, LeagueID, StartDate, EndDate
FROM Basketball.Seasons
WHERE LeagueID = @LeagueID;
GO

CREATE OR ALTER PROCEDURE Basketball.UpdateSeason
    @SeasonID INT,
    @StartDate DATE,
    @EndDate DATE
AS
UPDATE Basketball.Seasons
SET StartDate = @StartDate,
    EndDate = @EndDate
WHERE SeasonID = @SeasonID;

SELECT SeasonID, LeagueID, StartDate, EndDate
FROM Basketball.Seasons
WHERE SeasonID = @SeasonID;
GO

/****************************
 * Team Procedures
 ****************************/
CREATE OR ALTER PROCEDURE Basketball.CreateTeam
    @SeasonID INT,
    @TeamName NVARCHAR(64),
    @TeamID INT OUTPUT
AS
INSERT Basketball.Teams(SeasonID, TeamName)
VALUES(@SeasonID, @TeamName);
SET @TeamID = SCOPE_IDENTITY();
GO

CREATE OR ALTER PROCEDURE Basketball.FetchTeam
    @TeamID INT
AS
SELECT TeamID, SeasonID, TeamName
FROM Basketball.Teams
WHERE TeamID = @TeamID;
GO

CREATE OR ALTER PROCEDURE Basketball.RetrieveTeamsBySeason
    @SeasonID INT
AS
SELECT TeamID, SeasonID, TeamName
FROM Basketball.Teams
WHERE SeasonID = @SeasonID;
GO

CREATE OR ALTER PROCEDURE Basketball.UpdateTeam
    @TeamID INT,
    @TeamName NVARCHAR(64)
AS
UPDATE Basketball.Teams
SET TeamName = @TeamName
WHERE TeamID = @TeamID;

SELECT TeamID, SeasonID, TeamName
FROM Basketball.Teams
WHERE TeamID = @TeamID;
GO

CREATE OR ALTER PROCEDURE Basketball.DeleteTeam
    @TeamID INT
AS
DELETE FROM Basketball.Teams
WHERE TeamID = @TeamID;
GO

/****************************
 * Game Procedures
 ****************************/
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

/****************************
 * Player Procedures
 ****************************/
-- Create Player
CREATE OR ALTER PROCEDURE Basketball.CreatePlayer
    @TeamID INT,
    @JerseyNumber INT,
    @FirstName NVARCHAR(32),
    @LastName NVARCHAR(32),
    @Position NVARCHAR(16), -- Added Parameter
    @Age INT = NULL,
    @Height NVARCHAR(16) = NULL,
    @Weight INT = NULL,
    @PlayerID INT OUTPUT
AS
INSERT Basketball.Players(TeamID, JerseyNumber, FirstName, LastName, [Position], Age, Height, Weight)
VALUES(@TeamID, @JerseyNumber, @FirstName, @LastName, @Position, @Age, @Height, @Weight);

SET @PlayerID = SCOPE_IDENTITY();
GO

-- Fetch Player by ID
CREATE OR ALTER PROCEDURE Basketball.FetchPlayer
    @PlayerID INT
AS
SELECT PlayerID, TeamID, JerseyNumber, FirstName, LastName, [Position], Age, Height, Weight
FROM Basketball.Players
WHERE PlayerID = @PlayerID;
GO

-- Retrieve Players by Team
CREATE OR ALTER PROCEDURE Basketball.RetrievePlayersByTeam
    @TeamID INT
AS
SELECT PlayerID, TeamID, JerseyNumber, FirstName, LastName, [Position], Age, Height, Weight
FROM Basketball.Players
WHERE TeamID = @TeamID;
GO

-- Update Player
CREATE OR ALTER PROCEDURE Basketball.UpdatePlayer
    @PlayerID INT,
    @JerseyNumber INT,
    @Position NVARCHAR(16), -- Added Parameter
    @Age INT = NULL,
    @Height NVARCHAR(16) = NULL,
    @Weight INT = NULL
AS
UPDATE Basketball.Players
SET JerseyNumber = @JerseyNumber,
    [Position] = @Position, -- Updated Column
    Age = @Age,
    Height = @Height,
    Weight = @Weight
WHERE PlayerID = @PlayerID;

-- Returning the updated record so C# can refresh the object
SELECT PlayerID, TeamID, JerseyNumber, FirstName, LastName, [Position], Age, Height, Weight
FROM Basketball.Players
WHERE PlayerID = @PlayerID;
GO

-- Delete Player
CREATE OR ALTER PROCEDURE Basketball.DeletePlayer
    @PlayerID INT
AS
DELETE FROM Basketball.Players
WHERE PlayerID = @PlayerID;
GO

/****************************
 * PlayerGameStats Procedures
 ****************************/
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

-- Upsert PlayerGameStats
CREATE OR ALTER PROCEDURE Basketball.UpdatePlayerGameStats
    @PlayerID INT, @GameID INT, @TeamID INT,
    @PlayingTime INT, @Turnovers INT,
    @Rebounds INT, @Assists INT, @Steals INT, @Blocks INT,
    @FieldGoalsMade INT = 0, @FieldGoalsTaken INT = 0,
    @ThreePointersMade INT = 0, @ThreePointersTaken INT = 0,
    @PersonalFouls INT = 0
AS
IF NOT EXISTS (SELECT 1 FROM Basketball.PlayerGameStats WHERE PlayerID = @PlayerID AND GameID = @GameID)
    INSERT Basketball.PlayerGameStats (
        PlayerID, GameID, TeamID, PlayingTime, Turnovers, Rebounds, Assists, Steals, Blocks,
        FieldGoalsMade, FieldGoalsTaken, ThreePointersMade, ThreePointersTaken, PersonalFouls)
    VALUES (@PlayerID, @GameID, @TeamID, @PlayingTime, @Turnovers, @Rebounds, @Assists, @Steals, @Blocks,
        @FieldGoalsMade, @FieldGoalsTaken, @ThreePointersMade, @ThreePointersTaken, @PersonalFouls)
ELSE
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
/****************************
 * 8. Users
 ****************************/
IF OBJECT_ID(N'Basketball.Users') IS NULL
BEGIN
    CREATE TABLE Basketball.Users
    (
        UserID       INT          NOT NULL IDENTITY(1,1),
        Username     NVARCHAR(64) NOT NULL,
        PasswordHash NVARCHAR(64) NOT NULL,
        IsAdmin      BIT          NOT NULL
            CONSTRAINT DF_Basketball_Users_IsAdmin DEFAULT(0),
        CONSTRAINT PK_Basketball_Users PRIMARY KEY CLUSTERED (UserID ASC),
        CONSTRAINT UK_Basketball_Users_Username UNIQUE (Username)
    );
END;
GO

/****************************
 * User Procedures
 ****************************/
CREATE OR ALTER PROCEDURE Basketball.CreateUser
    @Username     NVARCHAR(64),
    @PasswordHash NVARCHAR(64),
    @IsAdmin      BIT,
    @UserID       INT OUTPUT
AS
INSERT Basketball.Users(Username, PasswordHash, IsAdmin)
VALUES(@Username, @PasswordHash, @IsAdmin);
SET @UserID = SCOPE_IDENTITY();
GO

CREATE OR ALTER PROCEDURE Basketball.FetchUserByUsername
    @Username NVARCHAR(64)
AS
SELECT UserID, Username, PasswordHash, IsAdmin
FROM Basketball.Users
WHERE Username = @Username;
GO

CREATE OR ALTER PROCEDURE Basketball.RetrieveUsers
AS
SELECT UserID, Username, IsAdmin
FROM Basketball.Users
ORDER BY Username;
GO

CREATE OR ALTER PROCEDURE Basketball.UpdateUserAdminStatus
    @UserID  INT,
    @IsAdmin BIT
AS
UPDATE Basketball.Users
SET IsAdmin = @IsAdmin
WHERE UserID = @UserID;
GO

CREATE OR ALTER PROCEDURE Basketball.UpdateUserCredentials
    @UserID       INT,
    @Username     NVARCHAR(64),
    @PasswordHash NVARCHAR(64) = NULL
AS
UPDATE Basketball.Users
SET Username     = @Username,
    PasswordHash = CASE WHEN @PasswordHash IS NOT NULL
                        THEN @PasswordHash
                        ELSE PasswordHash END
WHERE UserID = @UserID;
GO

/****************************
 * Aggregating / Stats Procedures
 ****************************/
-- Drop old misspelled procedure if it exists from a previous run
IF OBJECT_ID(N'Basketball.TretreveMostactiveplaers') IS NOT NULL
    DROP PROCEDURE Basketball.TretreveMostactiveplaers;
GO

CREATE OR ALTER PROCEDURE Basketball.RetrieveMostActivePlayers
    @SeasonID INT
AS
SELECT
    P.PlayerID,
    P.FirstName + N' ' + P.LastName AS PlayerName,
    P.TeamID,
    COUNT(PGS.GameID) AS GamesPlayed,
    SUM(PGS.FieldGoalsMade * 2 + PGS.ThreePointersMade * 3) AS TotalPoints,
    CAST(AVG(CAST(PGS.FieldGoalsMade * 2 + PGS.ThreePointersMade * 3
        AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS PointsPerGame,
    RANK() OVER(
        ORDER BY COUNT(PGS.GameID) DESC,
        AVG(CAST(PGS.FieldGoalsMade * 2 + PGS.ThreePointersMade * 3
            AS DECIMAL(10,2))) DESC
    ) AS SeasonRank
FROM Basketball.Players P
    INNER JOIN Basketball.Teams T ON T.TeamID = P.TeamID
    INNER JOIN Basketball.PlayerGameStats PGS ON PGS.PlayerID = P.PlayerID
WHERE T.SeasonID = @SeasonID
GROUP BY P.PlayerID, P.FirstName, P.LastName, P.TeamID
ORDER BY SeasonRank ASC;
GO

CREATE OR ALTER PROCEDURE Basketball.RetrieveTopScorersByTeam
    @SeasonID INT
AS
SELECT
    P.TeamID,
    P.PlayerID,
    P.FirstName + N' ' + P.LastName AS PlayerName,
    COUNT(PGS.GameID) AS GamesPlayed,
    SUM(PGS.FieldGoalsMade * 2 + PGS.ThreePointersMade * 3) AS TotalPoints,
    CAST(AVG(CAST(PGS.FieldGoalsMade * 2 + PGS.ThreePointersMade * 3
        AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AveragePointsPerGame,
    RANK() OVER(
        PARTITION BY P.TeamID
        ORDER BY SUM(PGS.FieldGoalsMade * 2 + PGS.ThreePointersMade * 3) DESC
    ) AS TeamRank
FROM Basketball.Players P
    INNER JOIN Basketball.Teams T ON T.TeamID = P.TeamID
    INNER JOIN Basketball.PlayerGameStats PGS ON PGS.PlayerID = P.PlayerID
WHERE T.SeasonID = @SeasonID
GROUP BY P.TeamID, P.PlayerID, P.FirstName, P.LastName
ORDER BY P.TeamID ASC, TeamRank ASC;
GO

CREATE OR ALTER PROCEDURE Basketball.RetrieveTeamPerformance
    @SeasonID INT
AS
WITH TeamGames(TeamID, TeamName, IsWin, Score) AS
(
    SELECT
        T.TeamID, T.TeamName,
        CASE WHEN G.HomeTeamScore > G.AwayTeamScore THEN 1 ELSE 0 END,
        G.HomeTeamScore
    FROM Basketball.Teams T
        INNER JOIN Basketball.Games G ON G.HomeTeamID = T.TeamID
    WHERE T.SeasonID = @SeasonID
    UNION ALL
    SELECT
        T.TeamID, T.TeamName,
        CASE WHEN G.AwayTeamScore > G.HomeTeamScore THEN 1 ELSE 0 END,
        G.AwayTeamScore
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

CREATE OR ALTER PROCEDURE Basketball.RetrieveGameStatsSummary
    @StartDate DATE,
    @EndDate DATE
AS
SELECT
    G.GameID,
    G.Date AS GameDate,
    HT.TeamName AS HomeTeam,
    AT.TeamName AS AwayTeam,
    CAST(AVG(CAST(PGS.FieldGoalsMade * 2 + PGS.ThreePointersMade * 3
        AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AveragePoints,
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

PRINT 'BasketballLeague560 setup complete.';
PRINT 'Tables: Location, League, Seasons, Teams, Games, Players, PlayerGameStats';
PRINT 'Procedures: Location(4), League(4), Seasons(4), Teams(5), Games(5), Players(5), PlayerGameStats(6) = 33 total';
GO
