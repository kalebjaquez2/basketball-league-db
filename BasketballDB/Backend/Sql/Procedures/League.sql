-- Create League
CREATE OR ALTER PROCEDURE Basketball.CreateLeague
    @LeagueName NVARCHAR(64),
    @Location NVARCHAR(64),
    @LeagueID INT OUTPUT
AS
INSERT Basketball.League(LeagueName, Location)
VALUES(@LeagueName, @Location);
SET @LeagueID = SCOPE_IDENTITY();
GO

-- Fetch League by ID
CREATE OR ALTER PROCEDURE Basketball.FetchLeague
    @LeagueID INT
AS
SELECT LeagueID, LeagueName, Location
FROM Basketball.League
WHERE LeagueID = @LeagueID;
GO

-- Retrieve all Leagues
CREATE OR ALTER PROCEDURE Basketball.RetrieveLeagues
AS
SELECT LeagueID, LeagueName, Location
FROM Basketball.League;
GO

-- Update League
CREATE OR ALTER PROCEDURE Basketball.UpdateLeague
    @LeagueID INT,
    @LeagueName NVARCHAR(64),
    @Location NVARCHAR(64)
AS
UPDATE Basketball.League
SET LeagueName = @LeagueName,
    Location = @Location
WHERE LeagueID = @LeagueID;

SELECT LeagueID, LeagueName, Location
FROM Basketball.League
WHERE LeagueID = @LeagueID;
GO