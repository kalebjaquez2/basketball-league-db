/*******************************************************
 * CreateDatabase.sql
 * Recreational Basketball League - Full Setup Script
 * Includes: Schema, Tables, Constraints, Seed Data
 *******************************************************/

USE master;
GO

/****************************
 * Create Database
 ****************************/
IF DB_ID(N'BasketballDB') IS NULL
BEGIN
    CREATE DATABASE BasketballDB;
END;
GO

USE BasketballDB;
GO

/****************************
 * Create Schema
 ****************************/
IF SCHEMA_ID(N'Basketball') IS NULL
    EXEC(N'CREATE SCHEMA [Basketball];');
GO

/*******************************************************
 * SEED DATA
 *******************************************************/

/****************************
 * Basketball.Location
 * 4 cities across the US
 ****************************/
INSERT INTO Basketball.Location (City, State, Country)
VALUES
    ('Springfield',     'Illinois',     'USA'),
    ('Riverside',       'California',   'USA'),
    ('Lakewood',        'Colorado',     'USA'),
    ('Greenville',      'South Carolina','USA');
GO

/****************************
 * Basketball.League
 * 2 recreational leagues, one per region
 ****************************/
INSERT INTO Basketball.League (LeagueName, LocationID)
VALUES
    ('Midwest Rec Basketball League',   1), -- Springfield, IL
    ('West Coast Amateur Hoops League', 2); -- Riverside, CA
GO

/****************************
 * Basketball.Seasons
 * 2 seasons per league = 4 seasons total
 * Each season must have a unique StartDate and EndDate
 ****************************/
INSERT INTO Basketball.Seasons (LeagueID, StartDate, EndDate)
VALUES
    (1, '2022-09-01', '2022-12-15'),  -- Midwest Season 1
    (1, '2023-09-01', '2023-12-15'),  -- Midwest Season 2
    (2, '2022-10-01', '2023-01-20'),  -- West Coast Season 1
    (2, '2023-10-01', '2024-01-20');  -- West Coast Season 2
GO

/****************************
 * Basketball.Teams
 * 4 teams per season = 16 teams total
 * Each team name must be unique across all seasons
 ****************************/
INSERT INTO Basketball.Teams (SeasonID, TeamName)
VALUES
    -- Midwest Season 1 (SeasonID 1)
    (1, 'Springfield Ballers'),
    (1, 'Decatur Dunkers'),
    (1, 'Peoria Pivots'),
    (1, 'Rockford Rockets'),

    -- Midwest Season 2 (SeasonID 2)
    (2, 'Springfield Slammers'),
    (2, 'Decatur Dribblers'),
    (2, 'Peoria Panthers'),
    (2, 'Rockford Rims'),

    -- West Coast Season 1 (SeasonID 3)
    (3, 'Riverside Runners'),
    (3, 'Anaheim Aces'),
    (3, 'Pasadena Pistons'),
    (3, 'Torrance Titans'),

    -- West Coast Season 2 (SeasonID 4)
    (4, 'Riverside Raptors'),
    (4, 'Anaheim Arrows'),
    (4, 'Pasadena Prowlers'),
    (4, 'Torrance Thunder');
GO

/****************************
 * Basketball.Games
 * ~13 games per season = 52 games total
 * Round-robin style scheduling within each season
 *
 * Midwest Season 1: TeamIDs 1-4
 * Midwest Season 2: TeamIDs 5-8
 * West Coast Season 1: TeamIDs 9-12
 * West Coast Season 2: TeamIDs 13-16
 ****************************/
INSERT INTO Basketball.Games (HomeTeamID, AwayTeamID, HomeTeamScore, AwayTeamScore, CourtNumber, OvertimeCount, Date)
VALUES
    -- Midwest Season 1
    (1, 2,  68, 74,  1, 0, '2022-09-10'),
    (3, 4,  81, 79,  2, 0, '2022-09-10'),
    (1, 3,  55, 60,  1, 0, '2022-09-17'),
    (2, 4,  88, 85,  2, 1, '2022-09-17'),
    (1, 4,  72, 69,  1, 0, '2022-09-24'),
    (2, 3,  63, 63,  2, 1, '2022-09-24'),
    (4, 1,  77, 80,  1, 0, '2022-10-01'),
    (3, 2,  91, 84,  2, 0, '2022-10-01'),
    (4, 2,  66, 70,  1, 0, '2022-10-08'),
    (3, 1,  58, 74,  2, 0, '2022-10-08'),
    (1, 2,  83, 79,  1, 0, '2022-10-15'),
    (3, 4,  70, 68,  2, 0, '2022-10-15'),
    (2, 1,  76, 90,  1, 2, '2022-10-22'),

    -- Midwest Season 2
    (5, 6,  64, 71,  1, 0, '2023-09-09'),
    (7, 8,  88, 82,  2, 0, '2023-09-09'),
    (5, 7,  77, 73,  1, 0, '2023-09-16'),
    (6, 8,  59, 67,  2, 0, '2023-09-16'),
    (5, 8,  80, 80,  1, 1, '2023-09-23'),
    (6, 7,  74, 68,  2, 0, '2023-09-23'),
    (8, 5,  66, 72,  1, 0, '2023-09-30'),
    (7, 6,  85, 79,  2, 0, '2023-09-30'),
    (8, 6,  90, 88,  1, 1, '2023-10-07'),
    (7, 5,  61, 75,  2, 0, '2023-10-07'),
    (5, 6,  78, 82,  1, 0, '2023-10-14'),
    (7, 8,  69, 65,  2, 0, '2023-10-14'),
    (6, 5,  73, 77,  1, 0, '2023-10-21'),

    -- West Coast Season 1
    (9,  10, 84, 76,  3, 0, '2022-10-08'),
    (11, 12, 70, 74,  4, 0, '2022-10-08'),
    (9,  11, 66, 61,  3, 0, '2022-10-15'),
    (10, 12, 79, 83,  4, 0, '2022-10-15'),
    (9,  12, 88, 85,  3, 1, '2022-10-22'),
    (10, 11, 72, 69,  4, 0, '2022-10-22'),
    (12, 9,  65, 78,  3, 0, '2022-10-29'),
    (11, 10, 90, 87,  4, 1, '2022-10-29'),
    (12, 10, 74, 80,  3, 0, '2022-11-05'),
    (11, 9,  68, 71,  4, 0, '2022-11-05'),
    (9,  10, 77, 73,  3, 0, '2022-11-12'),
    (11, 12, 83, 79,  4, 0, '2022-11-12'),
    (10, 9,  86, 91,  3, 0, '2022-11-19'),

    -- West Coast Season 2
    (13, 14, 75, 82,  3, 0, '2023-10-07'),
    (15, 16, 68, 65,  4, 0, '2023-10-07'),
    (13, 15, 90, 88,  3, 1, '2023-10-14'),
    (14, 16, 77, 72,  4, 0, '2023-10-14'),
    (13, 16, 63, 70,  3, 0, '2023-10-21'),
    (14, 15, 84, 80,  4, 0, '2023-10-21'),
    (16, 13, 71, 69,  3, 0, '2023-10-28'),
    (15, 14, 78, 85,  4, 0, '2023-10-28'),
    (16, 14, 92, 89,  3, 1, '2023-11-04'),
    (15, 13, 66, 74,  4, 0, '2023-11-04'),
    (13, 14, 80, 76,  3, 0, '2023-11-11'),
    (15, 16, 73, 71,  4, 0, '2023-11-11'),
    (14, 13, 88, 84,  3, 0, '2023-11-18');
GO

/****************************
 * Basketball.Players
 * 5 players per game = 260 total player-game records
 * GameIDs 1-52, players per game assigned jersey numbers 4-8
 ****************************/

-- Game 1
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(1, 4,  'Marcus',  'Webb',     28, '6''2"', 195),
(1, 5,  'Derek',   'Hollins',  31, '6''0"', 180),
(1, 6,  'Tyrone',  'Bass',     25, '6''4"', 210),
(1, 7,  'Calvin',  'Morrow',   29, '5''11"',175),
(1, 8,  'Jerome',  'Patel',    33, NULL,    NULL);

-- Game 2
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(2, 4,  'Andre',   'Simmons',  27, '6''3"', 200),
(2, 5,  'Lamar',   'Hughes',   30, '6''1"', 185),
(2, 6,  'Darnell', 'Cruz',     24, '6''5"', 220),
(2, 7,  'Kenny',   'Foster',   35, '6''0"', 178),
(2, 8,  'Reggie',  'Banks',    22, NULL,    NULL);

-- Game 3
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(3, 4,  'Terrence','Price',    26, '6''2"', 192),
(3, 5,  'Malik',   'Stone',    29, '6''0"', 183),
(3, 6,  'Darius',  'Cole',     31, '6''6"', 230),
(3, 7,  'Victor',  'James',    28, '5''10"',170),
(3, 8,  'Alvin',   'Grant',    23, NULL,    NULL);

-- Game 4
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(4, 4,  'Shawn',   'Murphy',   32, '6''1"', 188),
(4, 5,  'Curtis',  'Bell',     27, '6''3"', 205),
(4, 6,  'Ronnie',  'Dixon',    25, '6''4"', 215),
(4, 7,  'Larry',   'Shaw',     34, '5''11"',172),
(4, 8,  'Eddie',   'Torres',   29, NULL,    NULL);

-- Game 5
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(5, 4,  'Byron',   'King',     26, '6''2"', 194),
(5, 5,  'Cedric',  'Ross',     30, '6''0"', 181),
(5, 6,  'Nathan',  'Perry',    28, '6''5"', 218),
(5, 7,  'Elijah',  'Reed',     24, '6''1"', 190),
(5, 8,  'Harvey',  'Cook',     33, NULL,    NULL);

-- Game 6
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(6, 4,  'Oscar',   'Rivera',   27, '6''3"', 202),
(6, 5,  'Willis',  'Morgan',   31, '6''1"', 186),
(6, 6,  'Quincy',  'Powell',   25, '6''6"', 225),
(6, 7,  'Roland',  'Hayes',    29, '5''10"',168),
(6, 8,  'Sidney',  'Bryant',   22, NULL,    NULL);

-- Game 7
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(7, 4,  'Walter',  'Coleman',  30, '6''2"', 196),
(7, 5,  'Eugene',  'Jenkins',  28, '6''0"', 182),
(7, 6,  'Francis', 'Sanders',  26, '6''4"', 212),
(7, 7,  'Herbert', 'Long',     35, '5''11"',174),
(7, 8,  'Irving',  'Patterson',24, NULL,    NULL);

-- Game 8
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(8, 4,  'Julian',  'Alexander',29, '6''3"', 204),
(8, 5,  'Luther',  'Scott',    32, '6''1"', 187),
(8, 6,  'Milton',  'Green',    27, '6''5"', 222),
(8, 7,  'Nelson',  'Adams',    25, '6''0"', 176),
(8, 8,  'Otto',    'Baker',    31, NULL,    NULL);

-- Game 9
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(9, 4,  'Percy',   'Nelson',   28, '6''2"', 193),
(9, 5,  'Quinn',   'Carter',   26, '6''0"', 180),
(9, 6,  'Russell', 'Mitchell', 30, '6''4"', 208),
(9, 7,  'Stanley', 'Perez',    33, '5''11"',171),
(9, 8,  'Theron',  'Roberts',  22, NULL,    NULL);

-- Game 10
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(10, 4, 'Ulysses', 'Turner',   27, '6''3"', 201),
(10, 5, 'Vernon',  'Phillips', 31, '6''1"', 184),
(10, 6, 'Winston', 'Campbell', 25, '6''6"', 228),
(10, 7, 'Xavier',  'Parker',   29, '5''10"',167),
(10, 8, 'Yosef',   'Evans',    24, NULL,    NULL);

-- Game 11
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(11, 4, 'Zachary', 'Edwards',  30, '6''2"', 197),
(11, 5, 'Aaron',   'Collins',  28, '6''0"', 183),
(11, 6, 'Bernard', 'Stewart',  26, '6''4"', 213),
(11, 7, 'Chester', 'Sanchez',  35, '5''11"',173),
(11, 8, 'Donald',  'Morris',   23, NULL,    NULL);

-- Game 12
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(12, 4, 'Edward',  'Rogers',   29, '6''3"', 205),
(12, 5, 'Floyd',   'Reed',     32, '6''1"', 188),
(12, 6, 'George',  'Cook',     27, '6''5"', 220),
(12, 7, 'Harold',  'Morgan',   25, '6''0"', 177),
(12, 8, 'Ivan',    'Bell',     31, NULL,    NULL);

-- Game 13
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(13, 4, 'Jack',    'Murphy',   28, '6''2"', 194),
(13, 5, 'Kevin',   'Rivera',   26, '6''0"', 181),
(13, 6, 'Leonard', 'Torres',   30, '6''4"', 209),
(13, 7, 'Manuel',  'Flores',   33, '5''11"',172),
(13, 8, 'Norman',  'Washington',22,NULL,    NULL);

-- Game 14
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(14, 4, 'Patrick', 'Lee',      27, '6''3"', 202),
(14, 5, 'Raymond', 'Harris',   31, '6''1"', 185),
(14, 6, 'Samuel',  'Clark',    25, '6''6"', 226),
(14, 7, 'Timothy', 'Lewis',    29, '5''10"',169),
(14, 8, 'Uriah',   'Robinson', 24, NULL,    NULL);

-- Game 15
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(15, 4, 'Vincent', 'Walker',   30, '6''2"', 198),
(15, 5, 'William', 'Hall',     28, '6''0"', 184),
(15, 6, 'Alton',   'Allen',    26, '6''4"', 214),
(15, 7, 'Bennie',  'Young',    35, '5''11"',174),
(15, 8, 'Carlos',  'Hernandez',23, NULL,    NULL);

-- Game 16
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(16, 4, 'Darryl',  'King',     29, '6''3"', 206),
(16, 5, 'Earnest', 'Wright',   32, '6''1"', 189),
(16, 6, 'Freddie', 'Lopez',    27, '6''5"', 221),
(16, 7, 'Gilbert', 'Hill',     25, '6''0"', 178),
(16, 8, 'Horace',  'Scott',    31, NULL,    NULL);

-- Game 17
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(17, 4, 'Isaac',   'Green',    28, '6''2"', 195),
(17, 5, 'Jasper',  'Adams',    26, '6''0"', 182),
(17, 6, 'Kelvin',  'Baker',    30, '6''4"', 210),
(17, 7, 'Leroy',   'Nelson',   33, '5''11"',171),
(17, 8, 'Marvin',  'Carter',   22, NULL,    NULL);

-- Game 18
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(18, 4, 'Nathan',  'Mitchell', 27, '6''3"', 203),
(18, 5, 'Oliver',  'Perez',    31, '6''1"', 186),
(18, 6, 'Phillip', 'Roberts',  25, '6''6"', 227),
(18, 7, 'Quinton', 'Turner',   29, '5''10"',168),
(18, 8, 'Randall', 'Phillips', 24, NULL,    NULL);

-- Game 19
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(19, 4, 'Stephen', 'Campbell', 30, '6''2"', 199),
(19, 5, 'Thomas',  'Parker',   28, '6''0"', 185),
(19, 6, 'Ulric',   'Evans',    26, '6''4"', 215),
(19, 7, 'Vaughn',  'Edwards',  35, '5''11"',175),
(19, 8, 'Warren',  'Collins',  23, NULL,    NULL);

-- Game 20
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(20, 4, 'Xavier',  'Stewart',  29, '6''3"', 207),
(20, 5, 'Yancy',   'Sanchez',  32, '6''1"', 190),
(20, 6, 'Zane',    'Morris',   27, '6''5"', 222),
(20, 7, 'Albert',  'Rogers',   25, '6''0"', 179),
(20, 8, 'Bobby',   'Reed',     31, NULL,    NULL);

-- Game 21
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(21, 4, 'Clyde',   'Cook',     28, '6''2"', 196),
(21, 5, 'Dennis',  'Morgan',   26, '6''0"', 183),
(21, 6, 'Emmett',  'Bell',     30, '6''4"', 211),
(21, 7, 'Felix',   'Murphy',   33, '5''11"',172),
(21, 8, 'Gary',    'Rivera',   22, NULL,    NULL);

-- Game 22
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(22, 4, 'Hank',    'Torres',   27, '6''3"', 204),
(22, 5, 'Ira',     'Flores',   31, '6''1"', 187),
(22, 6, 'Jake',    'Washington',25,'6''6"', 228),
(22, 7, 'Kirk',    'Lee',      29, '5''10"',169),
(22, 8, 'Lane',    'Harris',   24, NULL,    NULL);

-- Game 23
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(23, 4, 'Miles',   'Clark',    30, '6''2"', 200),
(23, 5, 'Neil',    'Lewis',    28, '6''0"', 186),
(23, 6, 'Owen',    'Robinson', 26, '6''4"', 216),
(23, 7, 'Paul',    'Walker',   35, '5''11"',176),
(23, 8, 'Rex',     'Hall',     23, NULL,    NULL);

-- Game 24
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(24, 4, 'Seth',    'Allen',    29, '6''3"', 208),
(24, 5, 'Todd',    'Young',    32, '6''1"', 191),
(24, 6, 'Uri',     'Hernandez',27, '6''5"', 223),
(24, 7, 'Vince',   'King',     25, '6''0"', 180),
(24, 8, 'Wade',    'Wright',   31, NULL,    NULL);

-- Game 25
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(25, 4, 'Alec',    'Lopez',    28, '6''2"', 197),
(25, 5, 'Brad',    'Hill',     26, '6''0"', 184),
(25, 6, 'Craig',   'Scott',    30, '6''4"', 212),
(25, 7, 'Dale',    'Green',    33, '5''11"',173),
(25, 8, 'Earl',    'Adams',    22, NULL,    NULL);

-- Game 26
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(26, 4, 'Frank',   'Baker',    27, '6''3"', 205),
(26, 5, 'Glen',    'Nelson',   31, '6''1"', 188),
(26, 6, 'Hugh',    'Carter',   25, '6''6"', 229),
(26, 7, 'Ian',     'Mitchell', 29, '5''10"',170),
(26, 8, 'Joel',    'Perez',    24, NULL,    NULL);

-- Game 27
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(27, 4, 'Karl',    'Roberts',  30, '6''2"', 201),
(27, 5, 'Leon',    'Turner',   28, '6''0"', 187),
(27, 6, 'Matt',    'Phillips', 26, '6''4"', 217),
(27, 7, 'Nick',    'Campbell', 35, '5''11"',177),
(27, 8, 'Omar',    'Parker',   23, NULL,    NULL);

-- Game 28
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(28, 4, 'Pete',    'Evans',    29, '6''3"', 209),
(28, 5, 'Russ',    'Edwards',  32, '6''1"', 192),
(28, 6, 'Scott',   'Collins',  27, '6''5"', 224),
(28, 7, 'Troy',    'Stewart',  25, '6''0"', 181),
(28, 8, 'Upton',   'Sanchez',  31, NULL,    NULL);

-- Game 29
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(29, 4, 'Vern',    'Morris',   28, '6''2"', 198),
(29, 5, 'Wes',     'Rogers',   26, '6''0"', 185),
(29, 6, 'Alex',    'Reed',     30, '6''4"', 213),
(29, 7, 'Blake',   'Cook',     33, '5''11"',174),
(29, 8, 'Cole',    'Morgan',   22, NULL,    NULL);

-- Game 30
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(30, 4, 'Drew',    'Bell',     27, '6''3"', 206),
(30, 5, 'Eric',    'Murphy',   31, '6''1"', 189),
(30, 6, 'Flynn',   'Rivera',   25, '6''6"', 230),
(30, 7, 'Greg',    'Torres',   29, '5''10"',171),
(30, 8, 'Hans',    'Flores',   24, NULL,    NULL);

-- Game 31
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(31, 4, 'Igor',    'Washington',30,'6''2"', 202),
(31, 5, 'James',   'Lee',      28, '6''0"', 188),
(31, 6, 'Knox',    'Harris',   26, '6''4"', 218),
(31, 7, 'Luca',    'Clark',    35, '5''11"',178),
(31, 8, 'Marco',   'Lewis',    23, NULL,    NULL);

-- Game 32
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(32, 4, 'Noel',    'Robinson', 29, '6''3"', 210),
(32, 5, 'Otis',    'Walker',   32, '6''1"', 193),
(32, 6, 'Paco',    'Hall',     27, '6''5"', 225),
(32, 7, 'Rhys',    'Allen',    25, '6''0"', 182),
(32, 8, 'Sven',    'Young',    31, NULL,    NULL);

-- Game 33
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(33, 4, 'Tate',    'Hernandez',28, '6''2"', 199),
(33, 5, 'Uma',     'King',     26, '6''0"', 186),
(33, 6, 'Vito',    'Wright',   30, '6''4"', 214),
(33, 7, 'Walt',    'Lopez',    33, '5''11"',175),
(33, 8, 'Xander',  'Hill',     22, NULL,    NULL);

-- Game 34
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(34, 4, 'Yul',     'Scott',    27, '6''3"', 207),
(34, 5, 'Zach',    'Green',    31, '6''1"', 190),
(34, 6, 'Abe',     'Adams',    25, '6''6"', 231),
(34, 7, 'Beau',    'Baker',    29, '5''10"',172),
(34, 8, 'Cain',    'Nelson',   24, NULL,    NULL);

-- Game 35
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(35, 4, 'Dane',    'Carter',   30, '6''2"', 203),
(35, 5, 'Evan',    'Mitchell', 28, '6''0"', 189),
(35, 6, 'Ford',    'Perez',    26, '6''4"', 219),
(35, 7, 'Gage',    'Roberts',  35, '5''11"',179),
(35, 8, 'Holt',    'Turner',   23, NULL,    NULL);

-- Game 36
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(36, 4, 'Idris',   'Phillips', 29, '6''3"', 211),
(36, 5, 'Jace',    'Campbell', 32, '6''1"', 194),
(36, 6, 'Kane',    'Parker',   27, '6''5"', 226),
(36, 7, 'Liam',    'Evans',    25, '6''0"', 183),
(36, 8, 'Mack',    'Edwards',  31, NULL,    NULL);

-- Game 37
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(37, 4, 'Nash',    'Collins',  28, '6''2"', 200),
(37, 5, 'Owen',    'Stewart',  26, '6''0"', 187),
(37, 6, 'Penn',    'Sanchez',  30, '6''4"', 215),
(37, 7, 'Reid',    'Morris',   33, '5''11"',176),
(37, 8, 'Saul',    'Rogers',   22, NULL,    NULL);

-- Game 38
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(38, 4, 'Trey',    'Reed',     27, '6''3"', 208),
(38, 5, 'Ugo',     'Cook',     31, '6''1"', 191),
(38, 6, 'Vin',     'Morgan',   25, '6''6"', 232),
(38, 7, 'Webb',    'Bell',     29, '5''10"',173),
(38, 8, 'Xavi',    'Murphy',   24, NULL,    NULL);

-- Game 39
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(39, 4, 'Yale',    'Rivera',   30, '6''2"', 204),
(39, 5, 'Zeke',    'Torres',   28, '6''0"', 190),
(39, 6, 'Ajax',    'Flores',   26, '6''4"', 216),
(39, 7, 'Bram',    'Washington',35,'5''11"',177),
(39, 8, 'Cruz',    'Lee',      23, NULL,    NULL);

-- Game 40
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(40, 4, 'Dex',     'Harris',   29, '6''3"', 212),
(40, 5, 'Enzo',    'Clark',    32, '6''1"', 195),
(40, 6, 'Finn',    'Lewis',    27, '6''5"', 227),
(40, 7, 'Gus',     'Robinson', 25, '6''0"', 184),
(40, 8, 'Hal',     'Walker',   31, NULL,    NULL);

-- Game 41
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(41, 4, 'Ike',     'Hall',     28, '6''2"', 201),
(41, 5, 'Jay',     'Allen',    26, '6''0"', 188),
(41, 6, 'Kai',     'Young',    30, '6''4"', 217),
(41, 7, 'Leo',     'Hernandez',33, '5''11"',178),
(41, 8, 'Max',     'King',     22, NULL,    NULL);

-- Game 42
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(42, 4, 'Ned',     'Wright',   27, '6''3"', 209),
(42, 5, 'Obi',     'Lopez',    31, '6''1"', 192),
(42, 6, 'Pat',     'Hill',     25, '6''6"', 233),
(42, 7, 'Raj',     'Scott',    29, '5''10"',174),
(42, 8, 'Sam',     'Green',    24, NULL,    NULL);

-- Game 43
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(43, 4, 'Ted',     'Adams',    30, '6''2"', 205),
(43, 5, 'Ulf',     'Baker',    28, '6''0"', 191),
(43, 6, 'Van',     'Nelson',   26, '6''4"', 218),
(43, 7, 'Wil',     'Carter',   35, '5''11"',179),
(43, 8, 'Xen',     'Mitchell', 23, NULL,    NULL);

-- Game 44
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(44, 4, 'Yogi',    'Perez',    29, '6''3"', 213),
(44, 5, 'Zac',     'Roberts',  32, '6''1"', 196),
(44, 6, 'Aldo',    'Turner',   27, '6''5"', 228),
(44, 7, 'Bert',    'Phillips', 25, '6''0"', 185),
(44, 8, 'Colt',    'Campbell', 31, NULL,    NULL);

-- Game 45
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(45, 4, 'Dean',    'Parker',   28, '6''2"', 202),
(45, 5, 'Emil',    'Evans',    26, '6''0"', 189),
(45, 6, 'Fitz',    'Edwards',  30, '6''4"', 219),
(45, 7, 'Gore',    'Collins',  33, '5''11"',180),
(45, 8, 'Hiro',    'Stewart',  22, NULL,    NULL);

-- Game 46
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(46, 4, 'Ingo',    'Sanchez',  27, '6''3"', 210),
(46, 5, 'Jeb',     'Morris',   31, '6''1"', 193),
(46, 6, 'Kurt',    'Rogers',   25, '6''6"', 234),
(46, 7, 'Lars',    'Reed',     29, '5''10"',175),
(46, 8, 'Milo',    'Cook',     24, NULL,    NULL);

-- Game 47
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(47, 4, 'Nero',    'Morgan',   30, '6''2"', 206),
(47, 5, 'Otto',    'Bell',     28, '6''0"', 192),
(47, 6, 'Pell',    'Murphy',   26, '6''4"', 220),
(47, 7, 'Quil',    'Rivera',   35, '5''11"',181),
(47, 8, 'Remy',    'Torres',   23, NULL,    NULL);

-- Game 48
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(48, 4, 'Sage',    'Flores',   29, '6''3"', 214),
(48, 5, 'Thor',    'Washington',32,'6''1"', 197),
(48, 6, 'Uwe',     'Lee',      27, '6''5"', 229),
(48, 7, 'Vern',    'Harris',   25, '6''0"', 186),
(48, 8, 'Wolf',    'Clark',    31, NULL,    NULL);

-- Game 49
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(49, 4, 'Xing',    'Lewis',    28, '6''2"', 203),
(49, 5, 'York',    'Robinson', 26, '6''0"', 190),
(49, 6, 'Zeus',    'Walker',   30, '6''4"', 221),
(49, 7, 'Axel',    'Hall',     33, '5''11"',182),
(49, 8, 'Bode',    'Allen',    22, NULL,    NULL);

-- Game 50
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(50, 4, 'Cash',    'Young',    27, '6''3"', 211),
(50, 5, 'Dirk',    'Hernandez',31, '6''1"', 194),
(50, 6, 'Elio',    'King',     25, '6''6"', 235),
(50, 7, 'Flint',   'Wright',   29, '5''10"',176),
(50, 8, 'Grey',    'Lopez',    24, NULL,    NULL);

-- Game 51
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(51, 4, 'Hunt',    'Hill',     30, '6''2"', 207),
(51, 5, 'Jett',    'Scott',    28, '6''0"', 193),
(51, 6, 'Kobe',    'Green',    26, '6''4"', 222),
(51, 7, 'Lane',    'Adams',    35, '5''11"',183),
(51, 8, 'Mace',    'Baker',    23, NULL,    NULL);

-- Game 52
INSERT INTO Basketball.Players (GameID, JerseyNumber, FirstName, LastName, Age, Height, Weight) VALUES
(52, 4, 'Nile',    'Nelson',   29, '6''3"', 215),
(52, 5, 'Orion',   'Carter',   32, '6''1"', 198),
(52, 6, 'Pace',    'Mitchell', 27, '6''5"', 230),
(52, 7, 'Quade',   'Perez',    25, '6''0"', 187),
(52, 8, 'Reed',    'Roberts',  31, NULL,    NULL);
GO

/****************************
 * Basketball.PlayerGameStats
 * One row per player (PlayerIDs 1-260)
 * Realistic recreational league stat ranges:
 *   Points:      0-28
 *   PlayingTime: 10-40 minutes
 *   Turnovers:   0-6
 *   Rebounds:    0-12
 *   Assists:     0-8
 ****************************/
INSERT INTO Basketball.PlayerGameStats (PlayerID, Points, PlayingTime, Turnovers, Rebounds, Assists)
VALUES
(1,  14, 28, 2, 5, 3), (2,  18, 32, 3, 4, 6), (3,  22, 35, 1, 8, 2), (4,   8, 20, 4, 3, 1), (5,  10, 18, 2, 2, 4),
(6,  16, 30, 2, 6, 5), (7,  20, 34, 1, 7, 3), (8,  12, 22, 3, 4, 2), (9,   6, 15, 4, 2, 1), (10, 24, 38, 0, 9, 4),
(11, 18, 31, 2, 5, 6), (12, 10, 21, 3, 3, 2), (13, 26, 40, 1, 10, 3),(14,  8, 17, 4, 2, 1), (15, 14, 27, 2, 4, 5),
(16, 20, 33, 1, 7, 4), (17, 16, 29, 2, 6, 3), (18, 12, 23, 3, 3, 2), (19,  4, 12, 5, 1, 0), (20, 22, 36, 1, 8, 5),
(21, 18, 30, 2, 5, 4), (22, 14, 26, 3, 4, 3), (23, 10, 20, 4, 2, 2), (24, 28, 40, 0, 11, 6),(25,  8, 16, 4, 3, 1),
(26, 16, 29, 2, 6, 3), (27, 20, 34, 1, 7, 5), (28, 12, 22, 3, 4, 2), (29,  6, 14, 5, 1, 1), (30, 18, 31, 2, 5, 4),
(31, 22, 36, 1, 8, 3), (32, 10, 19, 4, 3, 2), (33, 14, 25, 3, 4, 4), (34, 24, 38, 0, 9, 5), (35,  8, 15, 5, 2, 1),
(36, 16, 28, 2, 6, 3), (37, 20, 33, 1, 7, 4), (38, 12, 21, 3, 3, 2), (39,  4, 11, 6, 1, 0), (40, 26, 40, 0, 10, 6),
(41, 18, 30, 2, 5, 4), (42, 14, 26, 3, 4, 3), (43, 10, 19, 4, 2, 2), (44, 22, 35, 1, 8, 5), (45,  8, 16, 4, 3, 1),
(46, 16, 28, 2, 6, 3), (47, 20, 33, 1, 7, 4), (48, 12, 22, 3, 4, 2), (49,  6, 13, 5, 1, 1), (50, 18, 31, 2, 5, 4),
(51, 24, 38, 0, 9, 5), (52, 10, 19, 4, 3, 2), (53, 14, 25, 3, 4, 4), (54, 28, 40, 0, 12, 6),(55,  8, 15, 5, 2, 1),
(56, 16, 28, 2, 6, 3), (57, 20, 33, 1, 7, 4), (58, 12, 21, 3, 3, 2), (59,  4, 10, 6, 1, 0), (60, 22, 36, 1, 8, 5),
(61, 18, 30, 2, 5, 4), (62, 14, 26, 3, 4, 3), (63, 10, 19, 4, 2, 2), (64, 26, 40, 0, 10, 6),(65,  8, 16, 4, 3, 1),
(66, 16, 29, 2, 6, 3), (67, 20, 34, 1, 7, 5), (68, 12, 22, 3, 4, 2), (69,  6, 14, 5, 1, 1), (70, 18, 31, 2, 5, 4),
(71, 22, 36, 1, 8, 3), (72, 10, 19, 4, 3, 2), (73, 14, 25, 3, 4, 4), (74, 24, 38, 0, 9, 5), (75,  8, 15, 5, 2, 1),
(76, 16, 28, 2, 6, 3), (77, 20, 33, 1, 7, 4), (78, 12, 21, 3, 3, 2), (79,  4, 11, 6, 1, 0), (80, 28, 40, 0, 11, 6),
(81, 18, 30, 2, 5, 4), (82, 14, 26, 3, 4, 3), (83, 10, 19, 4, 2, 2), (84, 22, 35, 1, 8, 5), (85,  8, 16, 4, 3, 1),
(86, 16, 28, 2, 6, 3), (87, 20, 33, 1, 7, 4), (88, 12, 22, 3, 4, 2), (89,  6, 13, 5, 1, 1), (90, 18, 31, 2, 5, 4),
(91, 24, 38, 0, 9, 5), (92, 10, 19, 4, 3, 2), (93, 14, 25, 3, 4, 4), (94, 26, 40, 0, 10, 6),(95,  8, 15, 5, 2, 1),
(96, 16, 28, 2, 6, 3), (97, 20, 33, 1, 7, 4), (98, 12, 21, 3, 3, 2), (99,  4, 10, 6, 1, 0),(100, 22, 36, 1, 8, 5),
(101,18, 30, 2, 5, 4),(102,14, 26, 3, 4, 3),(103,10, 19, 4, 2, 2),(104,28, 40, 0, 12, 6),(105, 8, 16, 4, 3, 1),
(106,16, 29, 2, 6, 3),(107,20, 34, 1, 7, 5),(108,12, 22, 3, 4, 2),(109, 6, 14, 5, 1, 1),(110,18, 31, 2, 5, 4),
(111,22, 36, 1, 8, 3),(112,10, 19, 4, 3, 2),(113,14, 25, 3, 4, 4),(114,24, 38, 0, 9, 5),(115, 8, 15, 5, 2, 1),
(116,16, 28, 2, 6, 3),(117,20, 33, 1, 7, 4),(118,12, 21, 3, 3, 2),(119, 4, 11, 6, 1, 0),(120,26, 40, 0, 10, 6),
(121,18, 30, 2, 5, 4),(122,14, 26, 3, 4, 3),(123,10, 19, 4, 2, 2),(124,22, 35, 1, 8, 5),(125, 8, 16, 4, 3, 1),
(126,16, 28, 2, 6, 3),(127,20, 33, 1, 7, 4),(128,12, 22, 3, 4, 2),(129, 6, 13, 5, 1, 1),(130,18, 31, 2, 5, 4),
(131,24, 38, 0, 9, 5),(132,10, 19, 4, 3, 2),(133,14, 25, 3, 4, 4),(134,28, 40, 0, 11, 6),(135, 8, 15, 5, 2, 1),
(136,16, 28, 2, 6, 3),(137,20, 33, 1, 7, 4),(138,12, 21, 3, 3, 2),(139, 4, 10, 6, 1, 0),(140,22, 36, 1, 8, 5),
(141,18, 30, 2, 5, 4),(142,14, 26, 3, 4, 3),(143,10, 19, 4, 2, 2),(144,26, 40, 0, 10, 6),(145, 8, 16, 4, 3, 1),
(146,16, 29, 2, 6, 3),(147,20, 34, 1, 7, 5),(148,12, 22, 3, 4, 2),(149, 6, 14, 5, 1, 1),(150,18, 31, 2, 5, 4),
(151,22, 36, 1, 8, 3),(152,10, 19, 4, 3, 2),(153,14, 25, 3, 4, 4),(154,24, 38, 0, 9, 5),(155, 8, 15, 5, 2, 1),
(156,16, 28, 2, 6, 3),(157,20, 33, 1, 7, 4),(158,12, 21, 3, 3, 2),(159, 4, 11, 6, 1, 0),(160,28, 40, 0, 12, 6),
(161,18, 30, 2, 5, 4),(162,14, 26, 3, 4, 3),(163,10, 19, 4, 2, 2),(164,22, 35, 1, 8, 5),(165, 8, 16, 4, 3, 1),
(166,16, 28, 2, 6, 3),(167,20, 33, 1, 7, 4),(168,12, 22, 3, 4, 2),(169, 6, 13, 5, 1, 1),(170,18, 31, 2, 5, 4),
(171,24, 38, 0, 9, 5),(172,10, 19, 4, 3, 2),(173,14, 25, 3, 4, 4),(174,26, 40, 0, 10, 6),(175, 8, 15, 5, 2, 1),
(176,16, 28, 2, 6, 3),(177,20, 33, 1, 7, 4),(178,12, 21, 3, 3, 2),(179, 4, 10, 6, 1, 0),(180,22, 36, 1, 8, 5),
(181,18, 30, 2, 5, 4),(182,14, 26, 3, 4, 3),(183,10, 19, 4, 2, 2),(184,28, 40, 0, 11, 6),(185, 8, 16, 4, 3, 1),
(186,16, 29, 2, 6, 3),(187,20, 34, 1, 7, 5),(188,12, 22, 3, 4, 2),(189, 6, 14, 5, 1, 1),(190,18, 31, 2, 5, 4),
(191,22, 36, 1, 8, 3),(192,10, 19, 4, 3, 2),(193,14, 25, 3, 4, 4),(194,24, 38, 0, 9, 5),(195, 8, 15, 5, 2, 1),
(196,16, 28, 2, 6, 3),(197,20, 33, 1, 7, 4),(198,12, 21, 3, 3, 2),(199, 4, 11, 6, 1, 0),(200,26, 40, 0, 10, 6),
(201,18, 30, 2, 5, 4),(202,14, 26, 3, 4, 3),(203,10, 19, 4, 2, 2),(204,22, 35, 1, 8, 5),(205, 8, 16, 4, 3, 1),
(206,16, 28, 2, 6, 3),(207,20, 33, 1, 7, 4),(208,12, 22, 3, 4, 2),(209, 6, 13, 5, 1, 1),(210,18, 31, 2, 5, 4),
(211,24, 38, 0, 9, 5),(212,10, 19, 4, 3, 2),(213,14, 25, 3, 4, 4),(214,28, 40, 0, 12, 6),(215, 8, 15, 5, 2, 1),
(216,16, 28, 2, 6, 3),(217,20, 33, 1, 7, 4),(218,12, 21, 3, 3, 2),(219, 4, 10, 6, 1, 0),(220,22, 36, 1, 8, 5),
(221,18, 30, 2, 5, 4),(222,14, 26, 3, 4, 3),(223,10, 19, 4, 2, 2),(224,26, 40, 0, 10, 6),(225, 8, 16, 4, 3, 1),
(226,16, 29, 2, 6, 3),(227,20, 34, 1, 7, 5),(228,12, 22, 3, 4, 2),(229, 6, 14, 5, 1, 1),(230,18, 31, 2, 5, 4),
(231,22, 36, 1, 8, 3),(232,10, 19, 4, 3, 2),(233,14, 25, 3, 4, 4),(234,24, 38, 0, 9, 5),(235, 8, 15, 5, 2, 1),
(236,16, 28, 2, 6, 3),(237,20, 33, 1, 7, 4),(238,12, 21, 3, 3, 2),(239, 4, 11, 6, 1, 0),(240,28, 40, 0, 11, 6),
(241,18, 30, 2, 5, 4),(242,14, 26, 3, 4, 3),(243,10, 19, 4, 2, 2),(244,22, 35, 1, 8, 5),(245, 8, 16, 4, 3, 1),
(246,16, 28, 2, 6, 3),(247,20, 33, 1, 7, 4),(248,12, 22, 3, 4, 2),(249, 6, 13, 5, 1, 1),(250,18, 31, 2, 5, 4),
(251,24, 38, 0, 9, 5),(252,10, 19, 4, 3, 2),(253,14, 25, 3, 4, 4),(254,26, 40, 0, 10, 6),(255, 8, 15, 5, 2, 1),
(256,16, 28, 2, 6, 3),(257,20, 33, 1, 7, 4),(258,12, 21, 3, 3, 2),(259, 4, 10, 6, 1, 0),(260,22, 36, 1, 8, 5);
GO

PRINT 'BasketballDB created and seeded successfully.';
PRINT 'Totals: 4 Locations, 2 Leagues, 4 Seasons, 16 Teams, 52 Games, 260 Players, 260 PlayerGameStats rows.';
GO