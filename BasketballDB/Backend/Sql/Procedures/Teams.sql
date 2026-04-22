-- Create Team
CREATE OR ALTER PROCEDURE Basketball.CreateTeam
    @SeasonID INT,
    @TeamName NVARCHAR(64),
    @TeamID INT OUTPUT
AS
INSERT Basketball.Teams(SeasonID, TeamName)
VALUES(@SeasonID, @TeamName);
SET @TeamID = SCOPE_IDENTITY();
GO

-- Fetch Team by ID
CREATE OR ALTER PROCEDURE Basketball.FetchTeam
    @TeamID INT
AS
SELECT TeamID, SeasonID, TeamName
FROM Basketball.Teams
WHERE TeamID = @TeamID;
GO

-- Retrieve Teams by Season
CREATE OR ALTER PROCEDURE Basketball.RetrieveTeamsBySeason
    @SeasonID INT
AS
SELECT TeamID, SeasonID, TeamName
FROM Basketball.Teams
WHERE SeasonID = @SeasonID;
GO

-- Update Team
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

-- Delete Team
CREATE OR ALTER PROCEDURE Basketball.DeleteTeam
    @TeamID INT
AS
DELETE FROM Basketball.Teams
WHERE TeamID = @TeamID;
GO