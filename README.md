# Gestión de Productos - CRUD Full Stack

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-512BD4)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-CC2927)](https://www.microsoft.com/sql-server)

## Descripción

Aplicacion Full Stack para la gestion de productos, desarrollada como prueba tecnica. Implementa un CRUD completo con API REST, Entity Framework Core, SQL Server, Swagger y frontend en Blazor WebAssembly.

## Tecnologías Utilizadas

- **Backend**: ASP.NET Core Web API (.NET 8)
- **ORM**: Entity Framework Core 8
- **Base de Datos**: SQL Server (LocalDB)
- **Documentacion API**: Swagger 
- **Frontend**: Blazor WebAssembly
- **Control de Versiones**: Git + GitHub (Flujo con ramas desarrollo/produccion)

## Estructura del Proyecto

GestionProductos-CRUD/
├── GestionProductos.API/ # Backend - API REST
│ ├── Controllers/ # Controladores
│ ├── Data/ # DbContext
│ ├── Models/ # Entidades
│ └── Migrations/ # Migraciones EF Core
├── GestionProductos.Frontend/ # Frontend - Blazor WebAssembly
│ ├── Pages/ # Paginas (Listado, Formulario, Detalle)
│ ├── Services/ # Servicios para consumir API
│ ├── Models/ # Modelos compartidos
│ └── Components/ # Componentes (NavMenu, etc.)
├── evidencias/ # Capturas de pantalla
└── README.md # Este archivo



## Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recomendado) o VS Code
- [Git](https://git-scm.com/)


## Instalacion y Ejecucion

### 1. Clonar el repositorio

git clone https://github.com/Guille-X/GestionProductos-CRUD.git
cd GestionProductos-CRUD

## La cadena de conexion esta en GestionProductos.API/appsettings.Development.json

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GestionProductosDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}

## Ejecutar Migraciones

cd GestionProductos.API
dotnet ef database update

## Ejecutar Backend (API)

cd GestionProductos.API
dotnet run

La API estara disponible en:

HTTP: http://localhost:7220

Swagger: http://localhost:7220/swagger

## Ejecutar Frontend

cd GestionProductos.Frontend
dotnet run

El frontend estara disponible en:

HTTP: http://localhost:5182


## Endpoints de la API


Metodo	URL	Descripcion
GET	/api/productos	Lista productos (filtros: codigo, nombre, activo)
GET	/api/productos/{id}	Obtiene producto por ID
POST	/api/productos	Crea nuevo producto
PUT	/api/productos/{id}	Actualiza producto existente
DELETE	/api/productos/{id}	Elimina producto (soft delete)

## Pruebas Realizadas

Crear producto con datos validos

Validar codigo unico

Validar precio > 0

Validar stock >= 0

Actualizar producto existente

Soft delete (marcar como inactivo)

Listar con filtros

Ver detalle de producto

# Autor
## Guillermo Martin
## Email: guillermoajsivinac@gmail.com



