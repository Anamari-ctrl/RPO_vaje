# Three Owls Bookstore

Three Owls Book Store is an application created as an academic project. It includes user registration and login, product browsing with filtering and sorting, a detailed product view, a functional shopping cart, order placement, multilingual support, and user profile management.
This project represents the foundation of an online bookstore platform built with a modern tech stack.

<img width="1024" height="576" alt="image" src="https://github.com/user-attachments/assets/8ebcabbf-d818-42e3-90af-6e4178643947" />


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
- Set database connection string in appsetting.json.
- In SQL Server Object Explorer, create database and add it's connection string. 
- Run in Package Manager Console (for project WebStore.Entities):
- Add-Migration Initial
- Update-database

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

