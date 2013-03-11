CREATE TABLE [dbo].[account] (
    [name]     VARCHAR (255) NOT NULL,
    [server]   VARCHAR (255) NULL,
    [port]     INT           NULL,
    [username] VARCHAR (255) NULL,
    [password] VARCHAR (255) NULL,
    [use_ssl]  TINYINT       NULL,
    [active]   TINYINT       NULL,
    PRIMARY KEY CLUSTERED ([name] ASC)
);

CREATE TABLE [dbo].[inbox] (
    [idinbox]      UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [account_name] VARCHAR (255)    NOT NULL,
    [sender]       VARCHAR (255)    NULL,
    [subject]      VARCHAR (255)    NULL,
    [body]         TEXT             NULL,
    [date]         VARCHAR (255)    NULL,
    PRIMARY KEY CLUSTERED ([idinbox] ASC),
    CONSTRAINT [FK_inbox_To_account] FOREIGN KEY ([account_name]) REFERENCES [dbo].[account] ([name]) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE VIEW [dbo].[inbox_plain]
	AS SELECT idinbox, account_name,date,sender,subject FROM [inbox]