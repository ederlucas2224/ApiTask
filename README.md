Task Management API
Una API RESTful completa para gestión de tareas, desarrollada con ASP.NET Core, Entity Framework Core y arquitectura limpia.

🚀 Características
Gestión completa de tareas: Crear, leer, actualizar y eliminar tareas

Estados de tarea: Pendiente, En Progreso, Completado

Búsqueda y filtrado: Por estado y fecha de creación

Transacciones seguras: Manejo robusto de operaciones con rollback automático

Validación de datos: Validaciones a nivel de aplicación y base de datos

Documentación automática: Swagger/OpenAPI integrado

Manejo global de excepciones: Middleware personalizado para errores

Logging completo: Registro detallado de operaciones

🛠️ Stack Tecnológico
Backend: ASP.NET Core 8.0

ORM: Entity Framework Core

Base de datos: SQL Server (LocalDB para desarrollo)

Arquitectura: Clean Architecture con patrones Repository y Unit of Work

Documentación: Swagger/OpenAPI

Logging: ILogger integrado

📋 Requisitos Previos
.NET 8.0 SDK

SQL Server (LocalDB incluido con Visual Studio)

Visual Studio 2022 o VS Code

🏗️ Estructura del Proyecto
text
Api/
├── Controllers/          # Controladores REST API
├── Service/             # Lógica de negocio y DTOs
├── Repository/          # Patrón Repository y Unit of Work
├── Data/               # DbContext y configuraciones de EF Core
├── Model/              # Entidades del dominio
├── Middleware/         # Middleware personalizado
└── Program.cs          # Configuración e inyección de dependencias
🚀 Cómo Ejecutar el Proyecto
1. Clonar el Repositorio
bash
git clone <url-del-repositorio>
cd Api
2. Restaurar Dependencias
bash
dotnet restore
3. Configurar Base de Datos
La aplicación usa LocalDB por defecto. La cadena de conexión se configura en appsettings.json:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskAPIDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
4. Ejecutar Migraciones
bash
# Desde la consola de Package Manager en Visual Studio
Add-Migration InitialCreate
Update-Database

# O desde terminal
dotnet ef migrations add InitialCreate
dotnet ef database update
5. Ejecutar la Aplicación
bash
dotnet run
6. Acceder a la API
API: https://localhost:7000/api/tasks

Swagger UI: https://localhost:7000/swagger

Health Check: https://localhost:7000/health

📚 Endpoints de la API
Tareas
Método	Endpoint	Descripción
GET	/api/tasks	Obtener todas las tareas
GET	/api/tasks/{id}	Obtener tarea por ID
POST	/api/tasks	Crear nueva tarea
PUT	/api/tasks/{id}	Actualizar tarea existente
DELETE	/api/tasks/{id}	Eliminar tarea
Ejemplos de Uso
Crear tarea:

http
POST /api/tasks
Content-Type: application/json

{
  "title": "Mi nueva tarea",
  "description": "Descripción de la tarea"
}
Actualizar tarea:

http
PUT /api/tasks/1
Content-Type: application/json

{
  "title": "Título actualizado",
  "status": "InProgress"
}
🎯 Modelos de Datos
TaskItem (Entidad)
csharp
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreationDate { get; set; }
}
DTOs de API
CreateTaskRequest: Para creación de tareas

UpdateTaskRequest: Para actualización parcial

TaskResponse: Respuesta estandarizada

🏛️ Decisiones de Diseño Arquitectónico
1. Clean Architecture
Justificación: Separación clara de responsabilidades y independencia del framework.

Domain Layer (Model): Entidades puras sin dependencias externas

Application Layer (Service): Lógica de negocio y casos de uso

Infrastructure Layer (Data/Repository): Acceso a datos y implementaciones externas

Presentation Layer (Controllers): Manejo de HTTP y DTOs

2. Repository Pattern + Unit of Work
Justificación: Abstracción del acceso a datos y gestión transaccional.

csharp
// Ventajas implementadas:
- Desacoplamiento de Entity Framework
- Fácil testing con mocks
- Reutilización de operaciones CRUD
- Gestión consistente de transacciones
3. Separación en Capas
Justificación: Mantenibilidad y escalabilidad.

text
Controller → Service → Repository → DbContext
Controllers: Solo manejan HTTP, sin lógica de negocio

Services: Contienen reglas de negocio y coordinación

Repositories: Solo acceso a datos, sin lógica

DbContext: Configuración de EF Core

4. DTOs (Data Transfer Objects)
Justificación: Separación entre modelos de dominio y modelos de API.

Seguridad: No exponer entidades internas directamente

Flexibilidad: Evolución independiente de API y dominio

Performance: Control estricto de datos transferidos

5. Inyección de Dependencias
Justificación: Principio de inversión de dependencias (DIP).

csharp
// Configuración centralizada en Program.cs
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITaskService, TaskService>();
6. Manejo Global de Excepciones
Justificación: Consistencia en respuestas de error.

csharp
// Middleware personalizado
app.UseGlobalExceptionHandler();
7. Patrón CQRS Ligero
Justificación: Separación entre operaciones de lectura y escritura.

Queries: GetAllAsync, GetByIdAsync, GetTasksByStatusAsync

Commands: CreateTaskAsync, UpdateTaskAsync, DeleteTaskAsync

🔄 Patrones Implementados
Repository Pattern
Abstracción del acceso a datos

Fácil testing y mantenimiento

Unit of Work
Gestión transaccional coordinada

Consistencia en operaciones múltiples

Dependency Injection
Inyección nativa de ASP.NET Core

Configuración centralizada en Program.cs

Factory Pattern
IDesignTimeDbContextFactory para configuraciones de EF Core

🧪 Testing y Calidad
Validaciones: A nivel de modelo y negocio

Manejo de errores: Middleware global de excepciones

Logging: Registro estructurado de operaciones

Transacciones: Rollback automático en errores

🔒 Características de Seguridad
Validación de datos de entrada

Manejo seguro de excepciones

Transacciones atómicas

Logging de operaciones críticas

📊 Estado de Tareas
Los estados disponibles son:

Pending: Tarea pendiente

InProgress: Tarea en progreso

Completed: Tarea completada

🤝 Contribución
Fork del proyecto

Crear rama de feature (git checkout -b feature/AmazingFeature)

Commit de cambios (git commit -m 'Add AmazingFeature')

Push a la rama (git push origin feature/AmazingFeature)

Crear Pull Request

📄 Licencia
Este proyecto está bajo la Licencia MIT - ver el archivo LICENSE para detalles.

🆘 Soporte
Si encuentras algún problema o tienes preguntas:

Revisa la documentación de Swagger

Verifica los logs de la aplicación

Abre un issue en el repositorio