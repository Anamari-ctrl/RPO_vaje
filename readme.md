<div align="center">
    <code><img width="50" src="https://raw.githubusercontent.com/marwin1991/profile-technology-icons/refs/heads/main/icons/c%23.png" alt="C#" title="C#"/></code>
    <code><img width="50" src="https://raw.githubusercontent.com/marwin1991/profile-technology-icons/refs/heads/main/icons/_net_core.png" alt=".NET Core" title=".NET Core"/></code>
    <code><img width="50" src="https://raw.githubusercontent.com/marwin1991/profile-technology-icons/refs/heads/main/icons/vue_js.png" alt="Vue.js" title="Vue.js"/></code>
    <code><img width="50" src="https://raw.githubusercontent.com/marwin1991/profile-technology-icons/refs/heads/main/icons/html.png" alt="HTML" title="HTML"/></code>
    <code><img width="50" src="https://raw.githubusercontent.com/marwin1991/profile-technology-icons/refs/heads/main/icons/css.png" alt="CSS" title="CSS"/></code>
</div>

# Three Owls Bookstore

Three Owls Book Store is an application created as an academic project. It includes user registration and login, product browsing with filtering and sorting, a detailed product view, a functional shopping cart, order placement, multilingual support, and user profile management.
This project represents the foundation of an online bookstore platform built with a modern tech stack.

#### 🏡 Home page
<img width="1893" height="988" alt="slika" src="https://github.com/user-attachments/assets/222650f8-f48b-4675-a3f6-b2d36b0808f5" />

#### 🔐 Login page
<img width="1916" height="902" alt="slika" src="https://github.com/user-attachments/assets/05add66a-9ab7-4485-94c2-a2a9a38d4786" />

#### 🔒 Register page
<img width="1917" height="903" alt="slika" src="https://github.com/user-attachments/assets/e1f27c4e-88e5-4a61-ad5b-a9aa799f2aa0" />

#### 🛒 Cart
<img width="1258" height="538" alt="slika" src="https://github.com/user-attachments/assets/cbb4f3a5-b5c3-444c-b1b7-f5e2866a3b9f" />

#### 📄 User order history
<img width="1898" height="615" alt="slika" src="https://github.com/user-attachments/assets/e25d4349-0e58-4464-81ec-29403209f4ec" />

#### 🏬 List of branches
<img width="1902" height="801" alt="slika" src="https://github.com/user-attachments/assets/9076c7e8-6277-4b09-8b72-8cab53392d19" />

---

## 🔧 Features

Implemented:

- User management: registration, login, edit user data, change user password, order history,
- Product catalog:
    - filter products based on price, availability, brand, supplier ...
    - search products by title
    - sort products by title, price, create date ...
    - Change language option
    - Shopping carts
- Branches
- Orders
- User reviews for products

To be implemented:

- User management: settings, ..
- Product catalog:
    - filter products based on price, availability, category, genre ...

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

## 🛠 Configuration
###  Backend
1. Set database connection string in appsetting.json.
    - One way to get it:
      1. In Visual studio open 'SQL Server Object Explorer'.
      2. Create database. Right click on database and select Options. Find ConnectionString and add it to appsettings.json (Be careful that connection string has value 'false' for property Encrypt).
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
3. For each migration, other name must be provided.

### Frontend
- Requirements: Node.js 16+ and npm
- From PowerShell, install and run the frontend:

```powershell
npm install
npm run serve
```

- Open the app (usually `http://localhost:8080/`)
---

## 🧬 Project structure

### Backend

```Powershell
    WebStore
    ├───WebStore.API
    │   ├───Endpoints
    │   │   └───v1
    │   ├───MockData
    │   └───Properties
    ├───WebStore.Entities
    │   ├───DatabaseContext
    │   ├───Identity
    │   ├───Migrations
    │   ├───Models
    │   └───RequestFeatures
    ├───WebStore.Repositories
    │   ├───Extensions
    ├───WebStore.RepositoryContracts
    ├───WebStore.ServiceContracts
    │   ├───DTO
    │   │   ├───AuthDTO
    │   │   ├───BranchDTO
    │   │   ├───OrderDTO
    │   │   ├───OrderItemDTO
    │   │   ├───ProductDTO
    │   │   └───RatingDTO
    └───WebStore.Services
        └───Helpers
```

### Frontend

```Powershell
fe/
├── babel.config.js
├── jsconfig.json
├── package.json
├── vue.config.js
├── README.md
├── public/
│   └── index.html
└── src/
    ├── App.vue
    ├── main.js
    ├── styles.css
    ├── assets/
    │   ├── owl_logo.png
    │   ├── logo.png
    │   └── book1.jpg - book24.jpg
    ├── components/
    │   └── HelloWorld.vue
    ├── models/
    │   ├── login-user.js
    │   └── store.js
    ├── services/
    │   ├── account-service.js
    │   ├── books-service.js
    │   ├── mock-books-service.js
    │   ├── cart-service.js
    │   ├── stores-service.js
    │   └── order-service.js
    ├── router/
    │   └── index.js
    └── views/
        ├── LoginView.vue
        ├── RegisterView.vue
        ├── ForgotPasswordView.vue
        ├── ResetPasswordView.vue
        ├── HomeView.vue
        ├── BookDetailView.vue
        ├── CartView.vue
        ├── StoresView.vue
        └── ProfileView.vue

```
---
## 🔥 Testing

Miha Potočnik: Designed and executed unit tests using defined test cases to validate individual components against functional and edge-case requirements.

Anamari Orehar: Performed integration testing to verify correct interactions, data exchange, and interface compatibility between system modules.

Miša Rožman Atelšek: Conducted system-level and acceptance testing to ensure overall functionality, performance, and compliance with project specifications.

### 👼 Testers
- Miša Rožman Atelšek: misarozmana@gmail.com
- Anamari Orehar: anamari.orehar@gmail.com
- Miha Potočnik: mihapot@gmail.com

---
## 🧙‍♂️ Authors and support
- Miša Rožman Atelšek: misarozmana@gmail.com
- Anamari Orehar: anamari.orehar@gmail.com
- Miha Potočnik: mihapot@gmail.com
---
