# Three Owls Bookstore

Three Owls Book Store is an application created as an academic project. It includes user registration and login, product browsing with filtering and sorting, a detailed product view, a functional shopping cart, order placement, multilingual support, and user profile management.
This project represents the foundation of an online bookstore platform built with a modern tech stack.

<img width="1891" height="943" alt="image" src="https://github.com/user-attachments/assets/3a301222-2606-4d47-a97f-2663c5512659" />



---

## Features

- User management: registration, login, edit user data, settings, change user password, order history, ..
- Product catalog:
    - browse products based on category (book, audio book, e-book) and genre (horror, thriller, romance, ...), ...
    - filter products based on price, availability, language, category, genre, ...
- Branches,
- Shopping carts and orders
- User reviews for products
- Change language

---

## 🧰 Technologies Used

### Backend
- **C# (.NET Core)** – API, business logic, and server-side functionality

### Frontend
- **Vue.js** – Responsive, component-based user interface

---

### Prerequisites
- .NET SDK (.NET 8.0 or higher)
- Vue 3
- SQL Server

---

### Configuration BE
1. Set database connection string in appsetting.json.
    - One way to get it:
      1. In Visual studio open 'SQL Server Object Explorer'.
      2. Create database. Right click on database and select Options. Find ConnectionString and add it to appsettings.json.
3.  Open 'Package Manager Console' (View -> Other Windows).
4.  Select 'Default project': WebStore.Entities (make sure that startup project is 'WebStore.API')
5.  Run next commands:
    1. Add-Migration NameOfMigration
    2. Update-database

Note that: 
    1. For every model structure change in folders Models and Identity you have to run those two commands so that database is updated.
    2. When commiting to repository, ignore folder Migrations (in project WebStore.Entities)

#### Configuration FE
- Requirements: Node.js 16+ and npm
- From PowerShell, install and run the frontend:

```powershell
npm install
npm run serve
```

- Open the app (usually `http://localhost:8081/`)
---

### Authors
- Miša Rožman Atelšek
- Anamari Orehar
- Miha Potočnik

---

