# Sistema de Gestión de Biblioteca - API RESTful
**Candidato:** Juan Sebastián Jurado Torres  
**Nivel:** Junior Full-Stack Developer  
**Fecha:** Mayo 2026  

## 🚀 Decisiones de Diseño y Arquitectura

### 1. Arquitectura Onion (Desacoplamiento Total)
El sistema se ha estructurado utilizando **Arquitectura Onion (Arquitectura de Cebolla)** con el objetivo de garantizar un desacoplamiento total entre las reglas de negocio y las tecnologías de infraestructura (base de datos, controladores HTTP, librerías de terceros).

La dependencia de las capas fluye estrictamente hacia el centro:
* **Domain (Núcleo):** Contiene las entidades puras del dominio corporativo (`Book`, `User`, `Loan`, `Reservation`, `Copy`, `Author`, `Category`) completamente aisladas de cualquier framework.
* **Application (Lógica de Negocio):** Define los contratos de servicios, interfaces globales de repositorio (`IRepository<T>`) y gestiona las validaciones fundamentales de la aplicación.
* **Infrastructure (Persistencia):** Implementa el acceso físico a los datos utilizando **Entity Framework Core** y gestiona el contexto de base de datos relacional (`LibraryDbContext`).
* **API (Presentación):** Expone los endpoints RESTful documentados mediante controladores desacoplados, controlando los ciclos de vida de las peticiones HTTP.

### 2. Patrón Repository Genérico (`IRepository<T>`)
Para evitar la duplicación de código redundante (anti-patrón *Anemic Domain Model*) y centralizar las operaciones CRUD de persistencia, se diseñó e implementó un repositorio genérico. Esto nos permite cumplir de forma rigurosa con los principios **SOLID**:
* **Principio de Responsabilidad Única (SRP):** Los controladores delegan la persistencia de datos al repositorio, abstrayéndose de si la información viene de memoria, SQLite o SQL Server.
* **Principio de Abierto/Cerrado (OCP):** Nuevas entidades del sistema pueden adoptar el repositorio genérico de inmediato y extender sus capacidades sin necesidad de modificar el código base existente.

### 3. Estrategia de Persistencia Dinámica con SQLite
Se seleccionó **SQLite** como motor relacional portable para el desarrollo del Producto Mínimo Viable (MVP). Mediante la inicialización controlada por el ciclo de vida de la aplicación (`context.Database.EnsureCreated()`), la API mapea y autogenera de manera automática las tablas, claves primarias/foráneas y restricciones relacionales al arrancar. 

Esto garantiza una experiencia *"Plug-and-Play"* ideal para la evaluación técnica, eliminando dependencias pesadas de infraestructura local o contenedores externos en la fase de demo.

---

## 🛠️ Guía de Ejecución y Pruebas del MVP

### Prerrequisitos
* .NET SDK (Versión 8.0 o superior / .NET 10 compatible).
* IDE de desarrollo (Visual Studio 2022 o Visual Studio Code).

### Pasos para levantar la Demo local:
1. Abra la solución global `LibraryManager.sln`.
2. Establezca el proyecto **`Library.API`** como el *Proyecto de Inicio*.
3. Presione **F5** (o ejecute el comando `dotnet run --project Library.API` en la terminal).
4. El sistema verificará la existencia física de la base de datos. En la consola verá la confirmación en verde: `✅ Base de datos creada con éxito.`.

### Endpoints Principales habilitados:
* `GET /api/Books?search=cien&page=1&pageSize=10` -> Soporte integrado para búsquedas complejas multivariable (Título/ISBN) y paginación activa.
* `POST /api/users` -> Registro de cuentas con validación estricta de estructura de correo y asignación de roles (Lector/Administrador).
* `POST /api/loans?copyId=1&userId=1` -> Registra de manera segura un préstamo en la base de datos relacional.
* `POST /api/loans/{id}/return` -> Procesa el retorno físico de las copias, calculando de manera dinámica las multas económicas en caso de retrasos basados en la fecha límite.
* `GET /api/openapi/v1.json` -> Especificación y documentación viva de OpenAPI activa para consumo de Frontend (Next.js/Angular).
