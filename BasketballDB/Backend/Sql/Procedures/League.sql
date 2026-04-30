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
           (Loc.City + ', ' + Loc.State) AS [Location], -- Merges city/state for the UI
           L.LocationID
    FROM Basketball.League L
    INNER JOIN Basketball.Location Loc ON L.LocationID = Loc.LocationID
    WHERE ISNULL(L.IsDeleted, 0) = 0;
END
GO

-- Fetch League by ID
CREATE OR ALTER PROCEDURE Basketball.FetchLeague
    @LeagueID INT
AS
BEGIN
    SELECT L.LeagueID, L.LeagueName, 
           (Loc.City + ', ' + Loc.State) AS [Location], 
           L.LocationID
    FROM Basketball.League L
    INNER JOIN Basketball.Location Loc ON L.LocationID = Loc.LocationID
    WHERE L.LeagueID = @LeagueID;
END
GO

CREATE OR ALTER PROCEDURE Basketball.DeleteLeague
    @LeagueID INT
AS
BEGIN
    DELETE PGS
    FROM Basketball.PlayerGameStats PGS
    WHERE PGS.TeamID IN (
        SELECT T.TeamID FROM Basketball.Teams T
        INNER JOIN Basketball.Seasons S ON T.SeasonID = S.SeasonID
        WHERE S.LeagueID = @LeagueID
    );

    DELETE G
    FROM Basketball.Games G
    WHERE G.HomeTeamID IN (
        SELECT T.TeamID FROM Basketball.Teams T
        INNER JOIN Basketball.Seasons S ON T.SeasonID = S.SeasonID
        WHERE S.LeagueID = @LeagueID
    );

    DELETE P
    FROM Basketball.Players P
    WHERE P.TeamID IN (
        SELECT T.TeamID FROM Basketball.Teams T
        INNER JOIN Basketball.Seasons S ON T.SeasonID = S.SeasonID
        WHERE S.LeagueID = @LeagueID
    );

    DELETE T
    FROM Basketball.Teams T
    WHERE T.SeasonID IN (
        SELECT S.SeasonID FROM Basketball.Seasons S
        WHERE S.LeagueID = @LeagueID
    );

    DELETE FROM Basketball.Seasons WHERE LeagueID = @LeagueID;
    DELETE FROM Basketball.League  WHERE LeagueID = @LeagueID;
END
GO

-- Fix Update League
CREATE OR ALTER PROCEDURE Basketball.UpdateLeague
    @LeagueID INT,
    @LeagueName NVARCHAR(64),
    @LocationID INT -- Changed from NVARCHAR to INT
AS
BEGIN
    UPDATE Basketball.League
    SET LeagueName = @LeagueName,
        LocationID = @LocationID -- Changed from Location to LocationID
    WHERE LeagueID = @LeagueID;

    -- Return the updated record
    EXEC Basketball.FetchLeague @LeagueID;
END
GO