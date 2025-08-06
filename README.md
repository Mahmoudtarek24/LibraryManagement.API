# Library Management System 📚

A comprehensive Library Management System built with ASP.NET Core following Clean Architecture principles and **Database First** approach.

## 🎯 Project Overview

This project is a RESTful API for managing library operations including book management, user management, and borrowing system with role-based access control.

### Business Rules
- **Borrowing Limit**: Each member can borrow maximum 3 books at a time
- **User Roles**: System supports Members and Admins with different permissions
- **Book Availability**: Only available books can be borrowed
- **Role Restrictions**: Only Admins can manage books, authors, and users

## 🏗️ Architecture

The project follows **Clean Architecture** pattern with clear separation of concerns:

```
LibraryManagement/
├── Domain/                 
├── Application/          
├── Infrastructure/       
└── API/                  
```

### Key Features
- **Database First Approach**: Schema-driven development using Entity Framework scaffolding
- **Clean Architecture**: Maintainable and testable code structure
- **JWT Authentication**: Secure user authentication and authorization
- **Role-based Access**: Admin and Member roles with different permissions
- **Advanced Querying**: Search functionality and pagination support
- **RESTful API**: Standard HTTP methods and status codes

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core (Database First)
- **Authentication**: JWT (JSON Web Tokens)
- **Architecture Pattern**: Clean Architecture
- **API Style**: REST API

## 📊 Database Schema

The system manages four main entities:

### Entities
1. **Author**
2. **Book** 
3. **Users**
4. **Borrowing**
   
 ### Key Constraints & Indexes
- **Email Validation**: Users.Email must match pattern '%@%.%'
- **Unique Book-Author**: Unique index on (AuthorId, Name) in Books
- **Publishing Date**: Books cannot have future publishing dates
- **Role Validation**: Users can only be 'Admin' or 'Member'


### Stored Procedures by Entity

#### Users Management
- `spGetUserById` - Get user details by ID
- `spAddUser` - Create new user with role validation
- `spChangePassword` - Update user password securely
- `spGetAllMembers` - Get paginated users with search
- `spSearchMembers` - Search users by name or email
- `spLogin` - User authentication
- `spCheckEmailExists` - Email existence validation

#### Authors Management  
- `spGetAllAuthor` - Get authors with optional search
- `spGetAuthorById` - Get specific author
- `spCreateAuthor` - Add new author
- `spUpdateAuthor` - Update author information

#### Books Management
- `spGetBookById` - Get book details
- `spGetAllBooks` - Paginated books with search and availability filter
- `spAddBook` - Create book with author validation
- `spUpdateBook` - Update book information
- `spGetBooksByAuthor` - Get books by specific author
- `spGetAvailableBooks` - Get only available books
- `spDeleteBook` - Safe delete (only if not borrowed)
- `spIsBookAvailableForRental` - Check book availability

### Database Views
- `vw_BooksWithAuthors` - Books joined with author information
- `vw_AvailableBooks` - Only books available for rental

### Custom Functions
- `fn_IsEmailExists` - Check if email already exists
- `fn_IsBookAvailableForRental` - Validate book availability

