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
🔧 Configuración
1. Configuración de Base de Datos
La aplicación usa LocalDB por defecto. La cadena de conexión se configura en appsettings.json:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskAPIDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
2. Migraciones de Base de Datos
bash
# Crear migración
Add-Migration InitialCreate

# Aplicar migración
Update-Database
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

🧪 Testing y Calidad
Validaciones: A nivel de modelo y negocio

Manejo de errores: Middleware global de excepciones

Logging: Registro estructurado de operaciones

Transacciones: Rollback automático en errores

🚀 Ejecución
Clonar el repositorio

Restaurar paquetes NuGet

Ejecutar migraciones de base de datos

Ejecutar la aplicación

bash
dotnet restore
dotnet ef database update
dotnet run
La API estará disponible en: https://localhost:7000
Documentación Swagger: https://localhost:7000/swagger

📊 Estado de Tareas
Los estados disponibles son:

Pending: Tarea pendiente

InProgress: Tarea en progreso

Completed: Tarea completada

🔒 Características de Seguridad
Validación de datos de entrada

Manejo seguro de excepciones

Transacciones atómicas

Logging de operaciones críticas

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