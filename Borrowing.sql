
create table Borrowing(
Id int primary key identity (1,1) , 
BookId int not null ,
MemberId int not null ,
IsReturned bit default 0 ,
BorrowDate datetime not null ,
ExpectedReturnDate datetime not null ,
ActualReturnDate datetime null  ,
 -- when wirte constraint her depend on this column not another can not compare with another column
constraint CK_Expected_After_Borrow  check (ExpectedReturnDate>BorrowDate), 
constraint CHK_Actual_After_Borrow check (ActualReturnDate >= BorrowDate),-- when i put constraint her
--"on table level"   i can compare to column with each other 

constraint FK_Borrowing_Book foreign key (BookId) references Book(Id),
constraint FK_Borrowing_Users foreign key (MemberId) references Users(Id)
)


-------                      Stored procedure 

--1   BorrowBook  

 alter procedure spBorrowBook  
  @MemberId int , @BookId int , @BorrowDate dateTime , @ExpectedReturnDate datetime,
  @BorrowId int OUTPUT
   as
    begin

	set nocount on;
	 if dbo.fn_IsBookAvailableForRental(@BookId) = 0
	  begin
	   set @BorrowId=-1
	   return
	  End
     
	 if not exists(select 1 from Users where Id=@MemberId)
	 begin
	   set @BorrowId=-2
	   return
	  End

	  if (select count(*) from Borrowing where MemberId=@MemberId and IsReturned=0)>=3
	  begin
	   set @BorrowId=-3
	   return
	  End

	  IF exists(select 1 from Borrowing  where MemberId = @MemberId  and BookId = @BookId  and IsReturned = 0)
		begin
			set @BorrowId = -4  -- Member already has this book borrowed
			return
		end

	  insert into Borrowing(BookId,MemberId,IsReturned,BorrowDate,ExpectedReturnDate)
	  values (@BookId,@MemberId,0,@BorrowDate,@ExpectedReturnDate)

	  set @BorrowId=SCOPE_IDENTITY()
	End



-- ReturnBook
 
  alter procedure spReturnBook
   @MemberId int,  @BookId int,  @ActualReturnDate datetime, @updateBorrowId int output
    as
	 begin
		declare @result int;
		set nocount on;
   
		if not exists(select 1 from Users where Id = @MemberId) 
		begin
			set @updateBorrowId = -2;
			return;
		end
    
	   if not exists(select 1 from Borrowing where BookId = @BookId and MemberId = @MemberId and IsReturned = 0)
		begin
			set @updateBorrowId = -1;
			return;
		end
    
		update Borrowing 
		set IsReturned = 1, ActualReturnDate = @ActualReturnDate
		 where BookId = @BookId  and MemberId = @MemberId  and IsReturned = 0;
    
		select @updateBorrowId=Id  from Borrowing  where BookId = @BookId  and MemberId = @MemberId 
	end



--  GetCurrentBorrowedBooks

  alter procedure spGetCurrentBorrowedBooks  
  @PageNumber int =1 , @PageSize int =20   ,@searchTearm varchar(50) = null  , @totalCount int output
   as
    begin
	set nocount on;
	 select @totalCount= count(*) from Borrowing bo inner join Book b on bo.BookId = b.Id 
	                                                  inner join Users u on u.Id = bo.MemberId
	 where IsReturned =0  and (@searchTearm is null or b.Name like '%'+@searchTearm+'%'or
	                             u.FullName like '%'+@searchTearm+'%')


	 select bo.Id as BorrowId,  b.Id as BookId, b.Name, bo.BorrowDate,
	         bo.ExpectedReturnDate, bo.MemberId , u.FullName 
	  from Borrowing  bo inner join Book b on bo.BookId = b.Id 
	  inner join Users u on u.Id = bo.MemberId
      where  IsReturned =0  and (@searchTearm is null or b.Name like '%'+@searchTearm+'%' or
	                             u.FullName like '%'+@searchTearm+'%')
	  order by b.Id 
	  offset (@PageNumber-1)*@PageSize rows 
	  fetch next @pageSize rows only 
	end


---   GetOverdueBooks
 
 alter procedure spGetOverdueBooks
   @PageNumber int =1 , @PageSize int =20   ,@searchTearm varchar(50) = null  , @totalCount int output
    as
    begin
	set nocount on;
	 declare @Today  datetime =GETDATE()
	 select @totalCount= count(*) from Borrowing bo inner join Book b on bo.BookId = b.Id 
	                                                  inner join Users u on u.Id = bo.MemberId
	 where IsReturned =0  and @Today >bo.ExpectedReturnDate  and
	                      (@searchTearm is null or b.Name like '%'+@searchTearm+'%' 
	                        or u.FullName like '%'+@searchTearm+'%')


	 select bo.Id as BorrowId,  b.Id as BookId, b.Name, bo.BorrowDate,
	         bo.ExpectedReturnDate, bo.MemberId , u.FullName 
	  from Borrowing  bo inner join Book b on bo.BookId = b.Id 
	  inner join Users u on u.Id = bo.MemberId
      where  IsReturned =0    and @Today >bo.ExpectedReturnDate
	   and  (@searchTearm is null or b.Name like '%'+@searchTearm+'%' or u.FullName like '%'+@searchTearm+'%')
	  order by b.Id 
	  offset (@PageNumber-1)*@PageSize rows 
	  fetch next @pageSize rows only 
	end

--- GetMemberBorrowingHistory
  
  create procedure spGetMemberBorrowingHistory 
   @MemberId INT 
    as
	 begin
	  select  bo.Id as BorrowId,  b.Id as BookId, b.Name, bo.BorrowDate,
	         bo.ExpectedReturnDate, bo.ActualReturnDate , bo.MemberId , u.FullName 
	  from Borrowing  bo inner join Book b on bo.BookId = b.Id 
	  inner join Users u on u.Id = bo.MemberId where MemberId= @MemberId 
	 end
   


--  GetMostBorrowedBooks
  
  create procedure spGetMostBorrowedBooks
   as
    begin
	   select   b.Id as BookId,b.Name  , COUNT(*) as BorrowedBooks
	  from Borrowing  bo inner join Book b on bo.BookId = b.Id 
	  group by  b.Id , b.Name
	  order by COUNT(*) desc
	end

  
 


 create procedure spCountCurrentBorrows
   @MemberId int 
    as
	 begin

	  Declare @BorrowCount int;
	  select @BorrowCount = dbo.fn_CountCurrentBorrows(@MemberId)
	  select @BorrowCount 
	 end
  
 create function fn_CountCurrentBorrows(@MemberId INT)
returns int
as
begin
    Declare @Count int = 0;
    select @Count = count(*) from Borrowing  where MemberId = @MemberId  and IsReturned = 0;  
    return @Count;
end




--   GetActiveMembers ////////////

 create procedure spGetActiveMembers
  @PageNumber int =1 , @PageSize int =20 , @totalCount int output
   as
    begin
	  select @totalCount =count(*) 
	  from Borrowing  bo inner join Book b on bo.BookId = b.Id 
	  inner join Users u on u.Id = bo.MemberId 
	   where IsReturned =0 

	   select  bo.Id as BorrowId,  b.Id as BookId, b.Name, bo.BorrowDate,
	         bo.ExpectedReturnDate, bo.MemberId , u.FullName 
	  from Borrowing  bo inner join Book b on bo.BookId = b.Id 
	  inner join Users u on u.Id = bo.MemberId 
	   where IsReturned =0 
	   order by bo.Id
	   offset (@PageNumber-1)*@PageSize rows
	   fetch next @pageSize rows only
	end


	select *from Users  



	select*from users
	select*from Borrowing