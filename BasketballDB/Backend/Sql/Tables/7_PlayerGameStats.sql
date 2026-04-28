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
        Steals INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Steals DEFAULT(0),
        Blocks INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Blocks DEFAULT(0),
        FieldGoalsMade INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_FieldGoalsMade DEFAULT(0),
        FieldGoalsTaken INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_FieldGoalsTaken DEFAULT(0),
        ThreePointersMade INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_ThreePointersMade DEFAULT(0),
        ThreePointersTaken INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_ThreePointersTaken DEFAULT(0),
        PersonalFouls INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_PersonalFouls DEFAULT(0),

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