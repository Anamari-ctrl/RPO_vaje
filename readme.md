# Three Owls Bookstore

Three Owls Book Store is an application created as an academic project. It includes user registration and login, product browsing with filtering and sorting, a detailed product view, a functional shopping cart, order placement, multilingual support, and user profile management.
This project represents the foundation of an online bookstore platform built with a modern tech stack.

<img width="1916" height="893" alt="slika" src="https://github.com/user-attachments/assets/45e30be9-53ed-4e89-bec9-4ae17a031b93" />


---

## Features

Implemented:

- User management: registration, login, edit user data, change user password
- Product catalog:
    - filter products based on price, availability, brand, supplier ...
    - search products by title
    - sort products by title, price, create date ...

To be implemented:

- User management: settings, order history, ..
- Product catalog:
    - filter products based on price, availability, category, genre ...
- Branches,
- Shopping carts and orders
- User reviews for products
- Change language option

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

## Configuration
###  Backend
1. Set database connection string in appsetting.json.
    - One way to get it:
      1. In Visual studio open 'SQL Server Object Explorer'.
      2. Create database. Right click on database and select Options. Find ConnectionString and add it to appsettings.json.
3.  Open 'Package Manager Console' (View -> Other Windows).
4.  Select 'Default project': WebStore.Entities (make sure that startup project is 'WebStore.API')
5.  Run next commands:

```powershell
    Add-Migration NameOfMigration
    Update-database
```

Note that: 
1. For every model structure change in folders Models and Identity you have to run those both commands so that database is updated.
2. When commiting to repository, ignore folder Migrations (in project WebStore.Entities)

### Frontend
- Requirements: Node.js 16+ and npm
- From PowerShell, install and run the frontend:

```powershell
npm install
npm run serve
```

- Open the app (usually `http://localhost:8080/`)
---


### Authors and support
- Miša Rožman Atelšek: misarozmana@gmail.com
- Anamari Orehar: anamari.orehar@gmail.com
- Miha Potočnik: mihapot@gmail.com

---

