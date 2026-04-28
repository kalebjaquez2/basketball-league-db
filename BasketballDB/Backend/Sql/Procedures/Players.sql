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