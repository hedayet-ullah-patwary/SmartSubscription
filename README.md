# 💳 Smart Subscription System

A full-featured **Subscription Management System** built with **ASP.NET Core MVC (.NET 8)**, following a clean **3-layer architecture** (API / BLL / DAL) with **Entity Framework Core** and **SQL Server**.

The system allows admins to manage subscription plans, features, roles, and users, while end-users can browse plans, subscribe, make payments, and track their subscription history — all through role-based dashboards.

---

## 🎬 Project Demo

📺 **Watch the full project walkthrough on YouTube** — click the thumbnail below to play:

[![Smart Subscription System - YouTube Demo](https://img.youtube.com/vi/djDyk-_5Cew/maxresdefault.jpg)](https://www.youtube.com/watch?v=djDyk-_5Cew)

---

## 🚀 Features

### 👤 Authentication & Account Management
- User **Registration** and **Login**
- **Forgot Password** flow with reset code verification (enter code → verify → reset password)
- **Remember Me** (persistent login via secure cookie)
- Session-based authentication with inactivity timeout
- Account activation/deactivation control by Admin

### 🧑‍💼 Role-Based Access Control
- **Role Management** (Create/Edit roles — e.g., Admin, User)
- Users are assigned roles via a many-to-many `UserRole` mapping
- Route-level protection — Admin-only pages redirect unauthorized users to Login

### 📦 Subscription Plan Management (Admin)
- Create, edit, and manage **Plans** (Name, Price, Duration, API limit, Description, Active/Inactive status)
- Attach/manage **Features** included in each plan
- Activate or deactivate plans without deleting them

### 🧾 Subscriptions (User + Admin)
- Users can browse available plans and **subscribe**
- **Checkout** flow before confirming a subscription
- Subscription status tracking (Active / Expired / Cancelled)
- Admin can view and manage **all users' subscriptions**

### 💰 Payments
- Users can make payments for their subscriptions
- Full **payment history** per user
- Payment method and transaction type tracking
- Admin dashboard shows **total revenue** across the platform

### 📊 Dashboards & Analytics
- **Admin Dashboard** — total revenue, total users, total plans, total subscriptions, and users without an assigned role
- **User Dashboard** — personalized view of active subscriptions and account info
- **Analytics** page for deeper platform insights

### 🛠️ User & Feature Management (Admin)
- Full **CRUD** on system users (create, view details, edit, deactivate)
- Manage **Features** that can be attached to subscription plans

---

## 🏗️ Architecture

The project follows a clean **N-Layer (3-Layer) Architecture**, separating concerns across three projects within the solution:

```
SmartSubscription/
│
├── API/   →  Presentation Layer (ASP.NET Core MVC)
│   ├── Controllers/     → Handles HTTP requests & responses
│   ├── Views/            → Razor (.cshtml) UI pages
│   ├── Models/           → View-specific models
│   └── Program.cs        → App startup, DI container, middleware pipeline
│
├── BLL/   →  Business Logic Layer
│   ├── Services/         → Core business logic (UserService, PlanService, etc.)
│   ├── DTOs/              → Data Transfer Objects used between API ↔ BLL ↔ DAL
│   └── Mapper/           → Object-to-object mapping configuration
│
└── DAL/   →  Data Access Layer
    ├── EF/Tables/         → Entity Framework entity classes (DB Tables)
    ├── Interfaces/        → Repository contracts (IUserRepository, IPlanRepository, etc.)
    ├── Repositories/      → Repository pattern implementation (EF Core queries)
    └── DataAccessFactory  → Centralized repository/data-access creation
```

### How it works
1. **API Layer** receives HTTP requests through Controllers and renders Razor Views.
2. Controllers call into the **BLL (Service)** layer — no business logic lives in the controllers.
3. Services use **DTOs** to communicate cleanly between layers, with mapping handled centrally.
4. Services depend on **Repository interfaces**, which are implemented in the **DAL** using a generic `Repository<T>` pattern + entity-specific repositories.
5. **Entity Framework Core** maps the repositories to a **SQL Server** database.
6. Authentication is **session-based**, with a custom middleware in `Program.cs` that restores a session from a secure "Remember Me" cookie when present.

### Design Patterns Used
- **Repository Pattern** — abstracts data access logic from business logic
- **Service Layer Pattern** — centralizes business rules in BLL
- **DTO Pattern** — decouples internal entities from what's exposed to the API layer
- **Dependency Injection** — all services and repositories are registered via .NET's built-in DI container

---

## 🗄️ Database Schema

The system uses **SQL Server** with the following core tables:

| Table | Description | Key Columns |
|---|---|---|
| **User** | Stores registered users | `Id`, `Name`, `Email`, `Password`, `IsEmailVerified`, `IsActive`, `CreatedAt` |
| **Role** | Defines system roles (e.g. Admin, User) | `Id`, `Name` |
| **UserRole** | Many-to-many mapping between Users and Roles | `Id`, `UserId` (FK → User), `RoleId` (FK → Role) |
| **Plan** | Subscription plans available to users | `Id`, `Name`, `Price`, `DurationDays`, `ApiLimit`, `Description`, `IsActive` |
| **Feature** | Features that can be attached to plans | `Id`, `Name`, `Description` |
| **Subcription** | A user's subscription to a plan | `Id`, `UserId` (FK → User), `PlanId` (FK → Plan), `StartDate`, `EndDate`, `Status` |
| **Payment** | Payment records for subscriptions | `Id`, `UserId`, `SubcriptionId` (FK → Subcription), `Amount`, `PaymentDate`, `PaymentMethod`, `TransactionType` |

### Entity Relationships
- A **User** can have multiple **Roles** (via `UserRole`)
- A **User** can have multiple **Subscriptions**
- A **Plan** can be used by multiple **Subscriptions**
- A **Subscription** can have multiple **Payments**
- A **Plan** can be linked to multiple **Features**

```
User ──< UserRole >── Role
User ──< Subcription >── Plan
Subcription ──< Payment
```

---

## 🧰 Tech Stack

| Layer | Technology |
|---|---|
| Language | C# |
| Framework | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Frontend | Razor Views (.cshtml), HTML, CSS, Bootstrap |
| Auth | Session-based authentication + Remember Me cookie |
| Architecture | 3-Layer Architecture (API / BLL / DAL) + Repository Pattern |

---

## 🖼️ Screenshots

> All screenshots are available in the [`/screenshots`](./screenshots) folder of this repository.

### 🔐 Authentication

**Login**
Users sign in with their email and password. Includes a **"Remember Me"** checkbox (persists login via secure cookie) and inline error messages for invalid credentials or inactive accounts.
![Login](./screenshots/login.png)

**Create Account**
New user registration form — captures Name, Email, and Password to create a new account in the system.
![Create Account](./screenshots/create_account.png)

**Forgot Password**
First step of password recovery — user enters their registered email to request a reset code.
![Forgot Password](./screenshots/forget_pass.png)

**Enter Reset Code**
User is prompted to input the reset code that was generated for their account.
![Enter Reset Code](./screenshots/forget_enter_reset_code.png)

**Verify Reset Code**
The entered code is validated before allowing the user to proceed to setting a new password.
![Verify Reset Code](./screenshots/forget_reset_code.png)

**Reset Password**
Final step of recovery — user sets and confirms a new password for their account.
![Reset Password](./screenshots/reset_pass.png)

---

### 🧑‍💼 Admin Panel

**Admin Dashboard**
Central overview for admins — shows **total revenue, total users, total plans, total subscriptions**, and flags any users who don't yet have a role assigned.
![Admin Dashboard](./screenshots/admin_dashboard.png)

**Analytics**
Deeper insights into platform performance and subscription/revenue trends.
![Analytics](./screenshots/admin_analysis.png)

**Plan Management**
Lists all subscription plans with their price, duration, and active/inactive status. Admin can create new plans or edit/deactivate existing ones from here.
![Plan Management](./screenshots/admin_plan.png)

**New Plan**
Form to create a new subscription plan — set Name, Price, Duration (days), API limit, and Description.
![New Plan](./screenshots/admin_newPlan.png)

**Edit Plan**
Update an existing plan's details or toggle its active status.
![Edit Plan](./screenshots/admin_editPlan.png)

**Feature Management**
Lists all features that can be attached to subscription plans (e.g., what a plan unlocks for the user).
![Feature Management](./screenshots/admin_feature.png)

**Edit Feature**
Update the name or description of an existing feature.
![Edit Feature](./screenshots/admin_editFeature.png)

**Role Management**
Lists all system roles (e.g., Admin, User) used for access control.
![Role Management](./screenshots/admin_roleManagement.png)

**Edit Role**
Rename or update an existing role.
![Edit Role](./screenshots/admin_editRole.png)

**User Management**
Full list of registered users with options to view details, edit, or activate/deactivate accounts.
![User Management](./screenshots/admin_UserManagement.png)

**User Details**
Detailed view of a single user — profile info, account status, and assigned role.
![User Details](./screenshots/admin_userDetails.png)

**Edit User**
Update a user's information, role assignment, or active status.
![Edit User](./screenshots/admin_editUser.png)

**All Subscriptions**
Admin-level view of every subscription across all users, with status (Active/Expired/Cancelled).
![All Subscriptions](./screenshots/admin_subcription.png)

**Edit Subscription**
Modify a specific subscription's plan, dates, or status directly from the admin side.
![Edit Subscription](./screenshots/admin_editSubcribtion.png)

---

### 👤 User Panel

**User Dashboard**
Personalized landing page for a logged-in user — shows their active subscription and account summary.
![User Dashboard](./screenshots/user_dashbord.png)

**Available Plans**
Browse all active subscription plans available to subscribe to, with pricing and feature details.
![User Plans](./screenshots/user_plan.png)

**Checkout**
Confirmation step before finalizing a subscription — review the selected plan before payment.
![Checkout](./screenshots/user_checkout.png)

**My Subscriptions**
A user's own subscription history — current and past plans with their status and validity dates.
![My Subscriptions](./screenshots/user_subcriptions.png)

**Payment History**
Full record of a user's past payments, including amount, date, and payment method.
![Payment History](./screenshots/user_payments.png)

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 / VS Code

### Setup
```bash
# Clone the repository
git clone <YOUR_GITHUB_REPO_LINK_HERE>
cd SmartSubscription

# Update the connection string in API/appsettings.json
# "DbConn": "Server=YOUR_SERVER;Database=SmartSubcriptionsDb;Trusted_Connection=True;TrustServerCertificate=True;"

# Apply EF Core migrations (run from the DAL or solution root, adjust startup project as needed)
dotnet ef database update --project DAL --startup-project API

# Run the project
dotnet run --project API
```

The app will start with the **Login** page as the default route.

---

## 📁 Project Structure (Solution)

```
SmartSubscription.sln
├── API/      → Controllers, Views, Program.cs
├── BLL/      → Services, DTOs, Mapper
└── DAL/      → EF Entities, Repositories, Interfaces
```

---

## 📌 GitHub Repository

🔗 **Repo Link:** https://github.com/hedayet-ullah-patwary/SmartSubscription

---

## 📺 Video Tutorial

🔗 **YouTube:** [https://www.youtube.com/watch?v=djDyk-_5Cew](https://www.youtube.com/watch?v=djDyk-_5Cew)

---

## 📄 License

This project is open-source and available for learning purposes. Feel free to fork, explore, and build on top of it.

---

## ⭐ Support

If this project helped you, consider giving it a ⭐ on GitHub and subscribing to the YouTube channel for more full-stack .NET projects!
