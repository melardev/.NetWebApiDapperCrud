If(db_id(N'TodoCrud') IS NULL)
    BEGIN
        CREATE DATABASE [TodoCrud]
    END;
USE [TodoCrud]

go
CREATE TABLE [dbo].[Todo]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Title] VARCHAR(255) NOT NULL,
    [Description] TEXT NULL,
    [Completed] BIT NOT NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME NOT NULL DEFAULT GETDATE()
);


GO
Create procedure [dbo].[GetAllTodos]
as
Begin
    select Id, Title, Completed, CreatedAt, UpdatedAt
    from Todo
    order by CreatedAt
End

GO
Create procedure [dbo].[GetPending]
as
Begin
    select Id, Title, Completed, CreatedAt, UpdatedAt
    from Todo
    Where Completed = 0
    order by CreatedAt
End

GO
Create procedure [dbo].[GetCompleted]
as
Begin
    select Id, Title, Completed, CreatedAt, UpdatedAt
    from Todo
    Where Completed = 1
    order by CreatedAt
End

GO
Create procedure [dbo].[GetTodoById]
(
    @Id INTEGER
)
as
Begin
    select *
    from Todo
    Where Id = @Id
End

GO
Create procedure [dbo].[GetTodoProxyById]
(
    @Id INTEGER
)
as
Begin
    select id
    from Todo
    Where Id = @Id
End

GO
Create procedure [dbo].[CreateTodo]
(
    @Title VARCHAR(255),
    @Description TEXT,
    @Completed BIT
)
as
Begin
    Insert into Todo (Title, Description, Completed)
    Values (@Title,@Description,@Completed); SELECT SCOPE_IDENTITY();
End

GO
Create procedure [dbo].[UpdateTodo]
(
    @Id INTEGER ,
    @Title VARCHAR(255),
    @Description TEXT,
    @Completed BIT,
    @UpdatedAt DATETIME
)
as
begin
    Update Todo
    set Title=@Title,
        Description=@Description,
        Completed=@Completed,
        UpdatedAt=@UpdatedAt
    where Id=@Id
End

go

Create procedure [dbo].[DeleteTodo]
(
    @Id INTEGER
)
as
begin
    Delete from Todo where Id=@Id
End

go
Create procedure [dbo].[DeleteAllTodos]
as
begin
    Delete from Todo
End