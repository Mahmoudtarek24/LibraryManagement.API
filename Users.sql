Create Table Users(
Id int Primary Key Identity(1,1),
FullName varchar(35) not null ,
Email varchar(50) not null unique,
Password varchar(30) not null,
CreateOn Datetime not null constraint DF_Users_CreatOn default getdate(),
Role varchar(20) not null check (Role in ('Admin','Librarian','Member')),
Constraint CK_User_Email check (Email like '%@%.%')
)
--
alter table Users add CONSTRAINT  CK_Users_roles check(Role in ('Admin','Member'))
alter table Users alter column Role varchar(10) 

-- Index 
 --Create unique NONCLUSTERED Index IX_Users_Email on Users(Email)

--    Stored Procedure 

-- GetAllBooks  

--1 GetUserById

  alter Procedure spGetUserById
  @UserId int 
   as
    begin 
	select Id,FullName,Email,CreateOn,Password,Role from Users where Id=@UserId
	end


--2  AddUser
   
   alter procedure spAddUser
      @FullName varchar(35) , @Email  varchar(50) , @Password varchar(30) ,@CreateOn datetime 
	,@Role varchar(20) ,@NewUserId int output
	as
	 begin 
	  set nocount on 

	   if(@Role not in('Admin','Member'))
	   begin
	     set @NewUserId=-1
	     return ;
	   end

	   insert into Users(FullName,Email,Password,CreateOn,Role) values (@FullName,@Email,@Password,@CreateOn,@Role)
	   set @NewUserId=SCOPE_IDENTITY()
	 end

--3  Change Password
   
   ALTER PROCEDURE spChangePassword
    @NewPassword varchar(30),
    @OldPassword varchar(30), 
    @UserId int,              -- input parameter
    @Result int OUTPUT        -- output parameter منفصل
AS
BEGIN
    SET NOCOUNT ON
    
    IF NOT EXISTS(SELECT Id FROM Users WHERE Password = @OldPassword AND Id = @UserId)
    BEGIN
        SET @Result = -1;  -- فشل التحديث
        RETURN;
    END
    
    UPDATE Users 
    SET Password = @NewPassword 
    WHERE Id = @UserId
    
    SET @Result = @UserId;  -- نجح التحديث
END



--4 SearchMembers

  alter Procedure spSearchMembers @SearchTearm varchar(50) =null
  as 
   begin
    select Id,FullName,Email,CreateOn,Password,Role from Users
	where (@SearchTearm is null or FullName like '%'+@SearchTearm+'%' or Email like '%'+@SearchTearm+'%')
   end

--5  GetAllMembers 
  alter Procedure spGetAllMembers 
   @PageNumber int = 1 , @PageSize int = 20, @SearchTearm varchar(70) = null  
   ,@TotalCount int output 
   AS
   Begin
    set nocount on;
	select @TotalCount=count(*) from Users 
	 where (@SearchTearm is null or FullName like '%'+@SearchTearm+'%' or Email like '%'+@SearchTearm+'%')

	 select Id,FullName,Email,CreateOn,Role from Users
	 where (@SearchTearm is null or FullName like '%'+@SearchTearm+'%' or Email like '%'+@SearchTearm+'%')
	 order By Id
	 offset (@PageNumber-1) * @PageSize rows
	 fetch next @PageSize rows only 
   End


--6 LoginIn
 
  alter procedure spLogin @Email VARCHAR(50),  @Password VARCHAR(30)
	as
	begin
		set nocount on;

		if not exists (select 1 from Users where Email = @Email AND Password = @Password )
		begin
			-- No result will be returned
			return;
		end

		-- Return the user row
		select * from Users
		where Email = @Email AND Password = @Password;
	end

--7
alter procedure spCheckEmailExists
    @Email varchar(50),
    @Exists bit output
  as
	begin
		set @Exists = dbo.fn_IsEmailExists(@Email);
	end



 alter function fn_IsEmailExists(@email varchar(50))
  returns bit
   as
    begin
	 declare @IsValid bit
	  if  exists(select 1 from Users where Email=@email)
		  SET @IsValid = 1;
      else
		  SET @IsValid = 0;
     return @IsValid
	end
