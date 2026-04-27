IF OBJECT_ID(N'Basketball.PlayerGameStats') IS NULL
BEGIN
    CREATE TABLE Basketball.PlayerGameStats
    (
        PlayerID INT NOT NULL,
        GameID INT NOT NULL,
        TeamID INT NOT NULL,
        Points INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Points DEFAULT(0),
        PlayingTime INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_PlayingTime DEFAULT(0),
        Turnovers INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Turnovers DEFAULT(0),
        Rebounds INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Rebounds DEFAULT(0),
        Assists INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Assists DEFAULT(0),
        CONSTRAINT PK_Basketball_PlayerGameStats PRIMARY KEY CLUSTERED
        (
            PlayerID ASC, GameID ASC
        ),
        CONSTRAINT FK_Basketball_PlayerGameStats_PlayerID_Basketball_Players
            FOREIGN KEY (PlayerID)
            REFERENCES Basketball.Players(PlayerID),
        CONSTRAINT FK_Basketball_PlayerGameStats_GameID_Basketball_Games
            FOREIGN KEY (GameID)
            REFERENCES Basketball.Games(GameID),
        CONSTRAINT FK_Basketball_PlayerGameStats_TeamID_Basketball_Teams
            FOREIGN KEY (TeamID)
            REFERENCES Basketball.Teams(TeamID)
    );
END;