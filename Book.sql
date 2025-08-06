
create table Book(
Id int primary key identity(1,1),
Name varchar(60) not null,
Description  varchar(300) not null,
IsAvailableForRental  bit constraint DF_Book_IsAvailableForRental Default 1,
ImageUrl Varchar(100) not null,
PublishingDate datetime not null,
authorId int not null,
Constraint FK_BOOK_Author foreign key (authorId) references Author (Id)  
)

--default dehaviour on delete and update is no action "can not delete untill delete related data "
-- sql dint support word "Restrict" , but no action execute same job 

alter table Book add constraint CK_BOOK_PublishingDate check (PublishingDate<=getdate())
alter table Book alter column IsAvailableForRental bit not null

--Index

CREATE UNIQUE NONCLUSTERED INDEX IX_Book_AuthorId_BookName ON Book(authorId, Name);




--STORED PROCEDURE 

--1 GetBookById

 create procedure spGetBookById 
 @BookId int 
  as 
   begin
    select*from Book where Id=@BookId
   end

--2 GetAllBooks 
  
  alter procedure spGetAllBooks
  @SearchTearm varchar(60) =null ,   @IsAvailableForRental bit =null,
  @PageSize int= 20       , @PageNumber int =1,
  @TotalCount int output
   as
    begin
	 set nocount on;
	 select @TotalCount = count(*) 
     from Book b inner join Author a on b.AuthorId = a.Id
     where (@IsAvailableForRental is null or b.IsAvailableForRental = @IsAvailableForRental)
      and (@SearchTearm is null or  b.Name like '%' + @SearchTearm + '%' or  a.Name like '%' + @SearchTearm + '%');
    
	select  b.Id , b.Name as BookName, b.Description,  b.IsAvailableForRental,
    b.ImageUrl, b.PublishingDate, b.authorId, a.Name AS AuthorName
    from book b
    inner join  Author a on b.AuthorId = a.Id
    where (@IsAvailableForRental is null or b.IsAvailableForRental = @IsAvailableForRental)
      and (@SearchTearm is null or
           b.Name like '%' + @SearchTearm + '%' or 
           a.Name like '%' + @SearchTearm + '%')
    order by b.Id
    offset (@PageNumber - 1) * @PageSize ROWS
    fetch next @PageSize rows only;
	end


--3 AddBook 
 create procedure spAddBook
  @bookName varchar(60) , @Description varchar(300) ,@ImageUrl varchar(100) ,@IsAvailableForRental bit,
  @PublishingDate datetime ,@authorId int ,@newBookId int output
  
  as
   begin 
    set nocount on;
      IF not exists (select Id from Author where Id = @authorId)
		BEGIN
			SET @newBookId = -1; 
			RETURN;
		END
		
	  IF @PublishingDate > GETDATE()
		BEGIN
			SET @newBookId = -2; 
			RETURN;
		END
    
	  insert into book(Name,Description,IsAvailableForRental,ImageUrl,PublishingDate,authorId)
	    values(@bookName,@Description,@IsAvailableForRental,@ImageUrl,@PublishingDate,@authorId)
	
	    set @newBookId=SCOPE_IDENTITY()	
	end

--4 UpdateBook 
 
  create procedure spUpdateBook
  @bookName varchar(60) , @Description varchar(300) ,@ImageUrl varchar(100) ,@IsAvailableForRental bit,
  @PublishingDate datetime  ,@authorId int ,@updateBookId int output
    
	as
	 begin
	  set nocount on

	   if not exists(select Id from Author where Id=@authorId)
	   begin
		   SET @updateBookId = -1; 
				RETURN;
	   end 
	    IF @PublishingDate > GETDATE()
		BEGIN
			SET @updateBookId = -2; 
			RETURN;
		END

	   update book 
	    set ImageUrl=@ImageUrl , Description=@Description , Name=@bookName, PublishingDate=@PublishingDate,
	     IsAvailableForRental=@IsAvailableForRental ,authorId=@authorId  where Id=@updateBookId 
	 end

--5 GetBooksByAuthor
  create procedure spGetBooksByAuthor
   @authorId int 
    as
	 begin
	   set nocount on
	   select*from vw_BooksWithAuthors 
	   where Id=@authorId 
	 end
 --view can not directory accept  paramter , we used this wat to can used paramter with view 

 --6 GetAvailableBooks
  create procedure spGetAvailableBooks
  @IsAvailableForRental bit =1 
   as 
    begin
	 select*from vw_BooksWithAuthors  where IsAvailableForRental =@IsAvailableForRental
	end

--7 

















--                                           Views

create view vw_BooksWithAuthors as
select
    b.Id,
    b.Name AS BookName,
    b.Description,
    b.IsAvailableForRental,
    b.ImageUrl,
    b.PublishingDate,
    a.Id AS AuthorId,
    a.Name AS AuthorName
FROM book b
inner join  Author a ON b.AuthorId = a.Id;

 create  view vw_AvailableBooks as
 select *from vw_BooksWithAuthors where IsAvailableForRental =1



   -- function
   select dbo.fn_IsBookAvailableForRental(1)

create function fn_IsBookAvailableForRental(@bookId int)
 returns Bit
  as
   begin
    declare @result BIT;
	
	if exists (select 1 from book where Id=@bookId and IsAvailableForRental=1)
	 set @result=1

	else
     set @result = 0;

    return @result
   end


   alter procedure spIsBookAvailableForRental
    @bookId int  ,  @IsAvailable BIT OUTPUT
    as
	 begin
	  set nocount on;

      set @IsAvailable = dbo.fn_IsBookAvailableForRental(@BookId)
	 end



   ---delete book only dint have bowring
   --- add is deleted for sot delete
   create procedure spDeleteBook 
   @BookId int ,@deleteResult int output
    as
	 begin  
	  set nocount on;
	   
	   if exists (select 1 from Borrowing where BookId=@BookId and IsReturned =0)
	     begin
		  set @deleteResult=-1;
		  return
		 end

		 begin try
		  delete from Book where Id =@BookId 
		  
		  if(@@ROWCOUNT>0)
		    set @deleteResult=1;

		  else
		    set @deleteResult=0;
		 end try
		 begin catch
		 set @deleteResult=-2; ---database error
		 end catch
	 End


	 ---@@ROWCOUNT     return number of row effect for last (dml)



	 select*from users