
create database LibraryDB

use LibraryDB


create Table Author(

Id int primary key identity(1,1),
Name  nvarchar(50) not null,
CreateOn datetime not null constraint CK_Author_CreateOn check(CreateOn>'1900-01-01') 
 
)
--

Alter Table Author drop constraint CK_Author_CreateOn
Alter Table Author Add Constraint DF_Author_CreateOn Default  GETDATE() FOR CreateOn


-- Stored Procedure

alter procedure spGetAllAuthor
 @SearchTerm NVARCHAR(50) = NULL
 as
  begin
    select*from Author
	where (@SearchTerm IS NULL OR Name LIKE '%' + @SearchTerm + '%')
	order by Name
  end
 
 --get By Id

 ALTER PROCEDURE spGetAuthorById 
 @AuthorId INT
AS 
BEGIN
    SELECT * FROM Author WHERE Id = @AuthorId
END

--Create Author
 
 create procedure spCreateAuthor
 @AuthorName nvarchar(50) ,@createOn datetime, @NewAuthorId int output
 as
  begin
    set nocount on -- to prevent append this message (1 row(s) affected)
	
     insert into Author(Name,CreateOn) values(@AuthorName,@createOn) 
	
	set @NewAuthorId=SCOPE_IDENTITY() -- to return last id was added , dint used @@Idntity becoues its global 
	                                  -- can return last identity value added on aother execution
									  -- return null , 0 if operation dint success
  end


  --Update Author
  create Procedure spUpdateAuthor
  @AuthorName nvarchar(50) , @NewAuthorId int output
  as
   begin
    set nocount on
	update Author
	set Name=@AuthorName where Id=@NewAuthorId
  end

   