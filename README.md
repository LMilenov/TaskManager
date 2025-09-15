# TaskManager API

A simple **Task Management REST API** built with **.NET 7** and **PostgreSQL**.  
The project supports **user authentication (JWT)**, categories, and tasks with due dates and validation.  

---

## 🚀 Features
- User registration & login with JWT authentication
- CRUD operations for:
  - Categories
  - Tasks
- Validation:
  - Due dates must be in the future
  - Required fields with max length validation
- PostgreSQL database with Entity Framework Core (Code First)
- Swagger UI for API testing

---

## 🛠️ Tech Stack
- **.NET 7 / C#**
- **Entity Framework Core** (Code First Migrations)
- **PostgreSQL**
- **JWT Authentication**
- **Swagger / OpenAPI**

---

## ⚙️ Setup Instructions

1. Clone the repository
```bash
git clone https://github.com/your-username/TaskManager.git
cd TaskManager

2. Configure PostgreSQL

Create a database (example: taskmanagerdb) and update your connection string in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=taskmanagerdb;Username=postgres;Password=yourpassword"
}

3. Apply migrations
dotnet ef database update

4. Run the API
dotnet run --project TaskManager.API


API will be available at:
👉 http://localhost:5033/swagger

🔑 Authentication

Register a user: POST /api/auth/register

Login: POST /api/auth/login

Use the JWT token for authenticated endpoints (Authorization: Bearer <token>).

📌 Example Endpoints

GET /api/categories

POST /api/categories

GET /api/tasks

POST /api/tasks

🧪 Testing

The project includes xUnit tests for controllers and database logic using InMemoryDb.

Run tests with:

dotnet test

📄 License

This project is for educational purposes.
You can use it freely as a portfolio project.