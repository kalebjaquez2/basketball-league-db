-- Create Player
CREATE OR ALTER PROCEDURE Basketball.CreatePlayer
    @GameID INT,
    @JerseyNumber INT,
    @FirstName NVARCHAR(32),
    @LastName NVARCHAR(32),
    @Age INT = NULL,
    @Height NVARCHAR(16) = NULL,
    @Weight INT = NULL,
    @PlayerID INT OUTPUT
AS
INSERT Basketball.Players(GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight)
VALUES(@GameID, @JerseyNumber, @FirstName, @LastName, @Age, @Height, @Weight);
SET @PlayerID = SCOPE_IDENTITY();
GO

-- Fetch Player by ID
CREATE OR ALTER PROCEDURE Basketball.FetchPlayer
    @PlayerID INT
AS
SELECT PlayerID, GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight
FROM Basketball.Players
WHERE PlayerID = @PlayerID;
GO

-- Retrieve Players by Game
CREATE OR ALTER PROCEDURE Basketball.RetrievePlayersByGame
    @GameID INT
AS
SELECT PlayerID, GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight
FROM Basketball.Players
WHERE GameID = @GameID;
GO

-- Update Player
CREATE OR ALTER PROCEDURE Basketball.UpdatePlayer
    @PlayerID INT,
    @JerseyNumber INT,
    @Age INT = NULL,
    @Height NVARCHAR(16) = NULL,
    @Weight INT = NULL
AS
UPDATE Basketball.Players
SET JerseyNumber = @JerseyNumber,
    Age = @Age,
    Height = @Height,
    Weight = @Weight
WHERE PlayerID = @PlayerID;

SELECT PlayerID, GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight
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