# **Exercise Tracker API**  

A simple RESTful API to track exercise sessions, built with **ASP.NET Core**, **Entity Framework Core**, and **SQL Server**.  

## **Features**  
✅ Record and manage exercise sessions  
✅ CRUD operations for exercises  
✅ Uses **Entity Framework Core** for database management  
✅ Follows **clean architecture** with Repository and Service layers  
✅ Uses **DTOs** for better API responses  

## **Technologies Used**  
- **ASP.NET Core 8**  
- **Entity Framework Core**  
- **SQL Server**  
- **Dependency Injection**  

## **Installation**  

### **1️⃣ Clone the Repository**  
```bash
git clone https://github.com/Eyad-Mostafa/ExerciseTracker.git
cd ExerciseTracker
```

### **2️⃣ Set Up the Database**  
- Ensure SQL Server is running  
- Update the connection string in `appsettings.json`:  
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ExerciseTrackerDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### **3️⃣ Apply Migrations**  
Run the following command in the **Package Manager Console (PMC)**:  
```powershell
Update-Database
```

Or using the **CLI**:  
```bash
dotnet ef database update
```

### **4️⃣ Run the Application**  
```bash
dotnet run
```
The API will be available at **`https://localhost:7223`** (or the configured port).  

## **Endpoints**  

| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/exercises` | Get all exercises |
| `GET` | `/api/exercises/{id}` | Get an exercise by ID |
| `POST` | `/api/exercises` | Add a new exercise |
| `PUT` | `/api/exercises/{id}` | Update an existing exercise |
| `DELETE` | `/api/exercises/{id}` | Delete an exercise |

## **Example JSON Requests**  

### **➕ Create Exercise (`POST /api/exercises`)**  
```json
{
  "startDate": "2025-02-27T10:00:00",
  "endDate": "2025-02-27T11:30:00",
  "comments": "Morning workout"
}
```

### **🔄 Update Exercise (`PUT /api/exercises/{id}`)**  
```json
{
  "startDate": "2025-02-27T10:15:00",
  "endDate": "2025-02-27T11:45:00",
  "comments": "Extended workout session"
}
```

## **Project Structure**  
```
ExerciseTracker
│── ExerciseTracker.API
│   ├── Controllers
│   │   ├── ExercisesController.cs
│   ├── Services
│   │   ├── ExerciseService.cs
│   ├── DTOs
│   │   ├── ExerciseDto.cs
│
│── ExerciseTracker.Core
│   ├── Models
│   │   ├── Exercise.cs
│   ├── Repositories
│   │   ├── IExerciseRepository.cs
│
│── ExerciseTracker.EF
│   ├── Repositories
│   │   ├── ExerciseRepository.cs
│   ├── AppDbContext.cs
```
- **API Layer**: Handles HTTP requests and responses  
- **Service Layer**: Contains business logic  
- **Repository Layer**: Handles database operations using EF Core  
- **DTOs**: Define request structure

