-- Create Season
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

-- Fetch Season by ID
CREATE OR ALTER PROCEDURE Basketball.FetchSeason
    @SeasonID INT
AS
SELECT SeasonID, LeagueID, StartDate, EndDate
FROM Basketball.Seasons
WHERE SeasonID = @SeasonID;
GO

-- Retrieve Seasons by League
CREATE OR ALTER PROCEDURE Basketball.RetrieveSeasonsByLeague
    @LeagueID INT
AS
SELECT SeasonID, LeagueID, StartDate, EndDate
FROM Basketball.Seasons
WHERE LeagueID = @LeagueID;
GO

-- Update Season
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