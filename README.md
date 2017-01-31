
#About Digital Library:
	An online book store app that has two modules: Admin (back end) and Library(front end).
	
##Admin Area:
	Is where the book store is administered

###Admin Area Features:

- Add/update/delete Books, Authors, Publishers, Categories, Admin Users, Admin Roles
- View requests for books
- Issue/return of books
			
####Books:

- Add/update/delete books
	- Currently, a book can have only one author, category, and publisher
			
####Authors, Publishers and Categories:

- Add/update/delete

####Issues:

- View a list of books issued to the members 
- Delete an issue by flagging it returned

####Requests:

- View a list of requests for books from members
- Issue a book to the respective user thereby removing it from the request list

####Admin Users:

- Are those that administer the system 
- Belong to roles that can be defined by the master admin user (the omnipotent user of the system)
- Can carry out activities as per the access rights assigned to the roles they belong to 
- The system automatically creates the folowing admin users:
  "admin@example.com" with the password: "Password+1"
    - The omnipotent user in the system
    - Has access to the entire system
    
  "librarian@example.com" with the password: "Password+1"
	- This user has restricted access rights to the system

####Members:

- Are the public users of the system

####Roles:

- Roles determine the activities that can be performed by a user in the system
- The system comes with the following roles:
	- Admin
		- Omnipotent
		- admin@example.com user belongs to this role
	- Librarian
		- Less powerful than admin
		- Has restricted access to the backend
		- librarian@example.com user belongs to this role
    
  - Member
    - Public users of the system


**Front end:**

	Is the frontend of the application that will be used by members

- Members can:
	- View the books catalog
	- Add books to the request cart
	- Checkout the books thereby sending a request to the backend
		- Only registered users can send request for books
	


####Installtion:

- Clone the project

If you have VS 2015 Installed:
  - Open the project
  - Run the following Package manager Console commands
    - update-database -context DLContext
    - update-database -context IdentityContext    
  - Start the app
    
In case you want to use the .NET Core CLI:
  - Dotnet restore
  - dotnet ef database update -c IdentityContext
  - dotnet ef database update -c DLContext
  - Dotnet run
