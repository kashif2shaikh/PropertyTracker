
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/05/2014 11:57:07
-- Generated from EDMX file: C:\Users\kshaikh\Source\Repos\PropertyTracker\PropertyTracker.Web.Models\PropertyTrackerModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PropertyTracker.Db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserProperty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Properties] DROP CONSTRAINT [FK_UserProperty];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyProperty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Properties] DROP CONSTRAINT [FK_CompanyProperty];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_CompanyUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Properties]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Properties];
GO
IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Fullname] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [CompanyId] int  NOT NULL,
    [Photo] varbinary(max)  NULL
);
GO

-- Creating table 'Properties'
CREATE TABLE [dbo].[Properties] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [StateProvince] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [SquareFeet] float  NOT NULL,
    [Country] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Properties'
ALTER TABLE [dbo].[Properties]
ADD CONSTRAINT [PK_Properties]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Properties'
ALTER TABLE [dbo].[Properties]
ADD CONSTRAINT [FK_UserProperty]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProperty'
CREATE INDEX [IX_FK_UserProperty]
ON [dbo].[Properties]
    ([UserId]);
GO

-- Creating foreign key on [CompanyId] in table 'Properties'
ALTER TABLE [dbo].[Properties]
ADD CONSTRAINT [FK_CompanyProperty]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyProperty'
CREATE INDEX [IX_FK_CompanyProperty]
ON [dbo].[Properties]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_CompanyUser]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyUser'
CREATE INDEX [IX_FK_CompanyUser]
ON [dbo].[Users]
    ([CompanyId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------