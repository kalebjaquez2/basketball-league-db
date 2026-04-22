IF SCHEMA_ID(N'Basketball') IS NULL
    EXEC(N'CREATE SCHEMA [Basketball];');
GO
DROP TABLE IF EXISTS Basketball.PlayerGameStats;
DROP TABLE IF EXISTS Basketball.Players;
DROP TABLE IF EXISTS Basketball.Games;
DROP TABLE IF EXISTS Basketball.Teams;
DROP TABLE IF EXISTS Basketball.Seasons;
DROP TABLE IF EXISTS Basketball.League;
DROP TABLE IF EXISTS Basketball.[Location];
GO

CREATE TABLE Basketball.[Location]
(
    LocationID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    City NVARCHAR(64) NOT NULL,
    State NVARCHAR(64) NOT NULL,
    Country NVARCHAR(64) NOT NULL,
    UNIQUE(City, State, Country)
);

CREATE TABLE Basketball.League
(
    LeagueID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    LeagueName NVARCHAR(64) NOT NULL UNIQUE,
    LocationID INT NOT NULL REFERENCES Basketball.Location(LocationID)
);

CREATE TABLE Basketball.Seasons
(
    SeasonID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    LeagueID INT NOT NULL
        REFERENCES Basketball.League(LeagueID),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    UNIQUE(StartDate),
    UNIQUE(EndDate)
);

CREATE TABLE Basketball.Teams
(
    TeamID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    SeasonID INT NOT NULL REFERENCES Basketball.Seasons(SeasonID),
    TeamName NVARCHAR(64) NOT NULL UNIQUE
);

CREATE TABLE Basketball.Games
(
    GameID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    HomeTeamID INT NOT NULL REFERENCES Basketball.Teams(TeamID),
    AwayTeamID INT NOT NULL REFERENCES Basketball.Teams(TeamID),
    HomeTeamScore INT NOT NULL,
    AwayTeamScore INT NOT NULL,
    CourtNumber INT NOT NULL,
    OvertimeCount INT NOT NULL DEFAULT(0),
    Date DATE NOT NULL
);

CREATE TABLE Basketball.Players
(
    PlayerID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    GameID INT NOT NULL REFERENCES Basketball.Games(GameID),
    JerseyNumber INT NOT NULL,
    FirstName NVARCHAR(32) NOT NULL,
    LastName NVARCHAR(32) NOT NULL,
    Age INT NULL,
    Height NVARCHAR(16) NULL,
    Weight INT NULL
);

CREATE TABLE Basketball.PlayerGameStats
(
    PlayerID INT NOT NULL REFERENCES Basketball.Players(PlayerID),
    GameID INT NOT NULL REFERENCES Basketball.Games(GameID),
    Points INT NOT NULL DEFAULT(0),
    PlayingTime INT NOT NULL DEFAULT(0),
    Turnovers INT NOT NULL DEFAULT(0),
    Rebounds INT NOT NULL DEFAULT(0),
    Assists INT NOT NULL DEFAULT(0),
    PRIMARY KEY(PlayerID)
);