-- Create Location
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

-- Fetch Location by ID
CREATE OR ALTER PROCEDURE Basketball.FetchLocation
    @LocationID INT
AS
SELECT LocationID, City, State, Country
FROM Basketball.Location
WHERE LocationID = @LocationID;
GO

-- Retrieve all Locations
CREATE OR ALTER PROCEDURE Basketball.RetrieveLocations
AS
SELECT LocationID, City, State, Country
FROM Basketball.Location;
GO

-- Update Location
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