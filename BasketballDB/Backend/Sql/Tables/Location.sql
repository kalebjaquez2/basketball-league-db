IF OBJECT_ID(N'Basketball.Location') IS NULL
BEGIN
    CREATE TABLE Basketball.Location
    (
        LocationID INT NOT NULL IDENTITY(1,1),
        City NVARCHAR(64) NOT NULL,
        State NVARCHAR(64) NOT NULL,
        Country NVARCHAR(64) NOT NULL,
        CONSTRAINT PK_Basketball_Location_LocationID PRIMARY KEY CLUSTERED
        (
            LocationID ASC
        )
    );
END;
/****************************
 * Unique Constraints
 ****************************/
IF NOT EXISTS
    (
        SELECT *
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.Location')
            AND kc.[name] = N'UK_Basketball_Location_City_State_Country'
    )
BEGIN
    ALTER TABLE Basketball.Location
    ADD CONSTRAINT [UK_Basketball_Location_City_State_Country] UNIQUE NONCLUSTERED
    (
        City ASC,
        State ASC,
        Country ASC
    );
END;