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
            FOREIGN KEY (HomeTeamID)
            REFERENCES Basketball.Teams(TeamID),
        CONSTRAINT FK_Basketball_Games_AwayTeamID_Basketball_Teams
            FOREIGN KEY (AwayTeamID)
            REFERENCES Basketball.Teams(TeamID)
    );
END;