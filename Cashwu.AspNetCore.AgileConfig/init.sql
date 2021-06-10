
create database AgileConfig
go

create table AgileConfigValue
(
    [Key]         nvarchar(64)                   not null
        constraint PK_ConfigurationValues
            primary key,
    LastUpdatedOn datetime2 default getutcdate() not null,
    LastUpdatedBy varchar(20)                    null,
    Value         nvarchar(max)
)
go

create
    index IX_AgileConfigValue_LastUpdatedOn
    on AgileConfigValue (LastUpdatedOn)
go


CREATE TRIGGER Trigger_AgileConfigValue_UpdateTimeEntry
    ON AgileConfigValue
    AFTER
        UPDATE
    AS
    UPDATE c
    SET LastUpdatedOn = GETUTCDATE()
    FROM AgileConfigValue c
             JOIN Inserted i
                  ON c.[Key] = i.[Key]
go
