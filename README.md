# Library Management System

A full-stack web application for managing library operations, including book browsing, borrowing, and administration. Built with ASP.NET Core MVC and Entity Framework Core.

🔗 **Live Demo:** [http://librarymanagement.tryasp.net/](http://librarymanagement.tryasp.net/)

## Overview

This project simulates a real-world library system. Visitors can browse and view books, registered users can borrow, wishlist, cart, and purchase books, and staff (Librarians and Admins) manage the entire book lifecycle through dedicated dashboards.

The project also demonstrates practical relational database design, Entity Framework Core relationship handling, and Role-Based Access Control (RBAC).

## Features

### For Visitors (No Login Required)
- Browse all available books
- View book details, including description, price, and stock availability

### For Registered Users (Login Required)
- Add books to Cart
- Add books to Wishlist
- Move Wishlist items to Cart (from the Wishlist page or the Book Details page)
- Checkout and place orders (Cash on Delivery only, for demo purposes; no real payment gateway yet)
- Send a Borrow Request for a book
- View a personal profile containing:
  - Account details
  - Orders
  - Wishlist
  - Cart
  - Borrow Requests
  - Issued Books
  - Fines
  - Borrowing History

> Cart, Wishlist, Checkout, and Borrow Requests are all restricted to logged-in users.

### Borrowing Workflow
1. A user sends a Borrow Request for a book.
2. A Librarian reviews the request from their dashboard.
3. The Librarian either:
   - **Approves** the request and issues the book with a return date, or
   - **Rejects** the request.
4. Once approved, the book appears under **Issued Books** in the user's profile.

> **Note:** This is a library management demo, not an e-book reader. Once a book is issued, the user's profile shows a card for it with a **Return** option. There's no in-browser reading feature.

### Librarian Dashboard
- View all Issued Books
- View and manage Borrow Requests (Approve / Reject)

### Admin Dashboard
- Full CRUD on Books (Add, View, Update, Delete)
- Full CRUD on Orders
- View all registered Users
- View Borrow Requests and Issued Books
- Promote any user to **Librarian** or **Admin**

### Security & Access Control
- Role-Based Access Control (RBAC) implemented through a custom Authorization Filter
- Regular Users cannot access Librarian or Admin areas
- Librarians cannot access the Admin dashboard, and Admins cannot access the Librarian dashboard
- Only Admins can change a user's role

## Tech Stack
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server
- Bootstrap
- Session-based Authentication

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/Mansoor-110/LibraryManagement.git
```

### 2. Set up the database
A ready-to-use SQL script is included in the repo as **`db.sql`**. Run it on your SQL Server instance. It will:
- Create all the required tables
- Insert seed data for three default accounts (Admin, Librarian, User). See the Demo Accounts section below.

> ⚠️ Importing `db.sql` is required. The application relies on the seed data for its role logic to work correctly (see the note below).

#### Using a different database name
The script creates a database named `LibraryManagement` by default. If you'd like to use a different name, update `db.sql` before running it. This is easy to do with the help of any AI tool:
1. Give it the `db.sql` file.
2. Ask it to update the script to use your preferred database name.
3. Run the script it generates directly on your SQL Server.

Just make sure the `Database` value in your connection string (next step) matches the name you choose.

### 3. Configure the connection string
Open `appsettings.json` and update the connection string with your own SQL Server details:

```json
"ConnectionStrings": {
  "connect": "Server=[ServerName];Database=LibraryManagement;User Id=[UserId];Password=[Password];TrustServerCertificate=True"
}
```

Replace `[ServerName]`, `[UserId]`, and `[Password]` with your own SQL Server credentials.

### 4. Run the project
Once the database is set up and the connection string is configured, run the project from Visual Studio or with:
```bash
dotnet run
```

## Demo Accounts

The seed data in `db.sql` creates one account for each role:

| Role      | Username   | Email                   | Password    |
|-----------|------------|--------------------------|-------------|
| Admin     | Admin      | admin@gmail.com          | admin       |
| Librarian | Librarian  | librarian@gmail.com      | librarian   |
| User      | User       | user@gmail.com           | user        |

### A note on the main Admin account
The very first Admin account (User ID = 1) is protected by design:
- It **cannot be deleted** by anyone, including itself.
- No one can become an Admin through the website itself. Only an existing Admin can promote another user to Admin or Librarian.

This keeps at least one Admin account permanently in control of the system.

## Roadmap
- Real payment gateway integration (currently Cash on Delivery only)

## Contact
- **Email:** m.mansoorahmed1000@gmail.com
- **GitHub:** [Mansoor-110](https://github.com/Mansoor-110)
- **LinkedIn:** [mansoorahmed1000](https://www.linkedin.com/in/mansoorahmed1000/)
