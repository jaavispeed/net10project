# PulseTrain API

API backend desarrollada en **.NET 10** siguiendo un enfoque **CQRS** con **MediatR**, **Mapster** y **Command/Handler**.  
El proyecto está preparado para ejecutarse tanto **localmente** como mediante **Docker**, y utiliza **Scalar** para la documentación de la API.

---

## Stack tecnológico

- **.NET 10**
- **ASP.NET Core Web API**
- **MediatR** (Command / Handler)
- **Mapster** (mapping de DTOs)
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Docker / Docker Compose**
- **Scalar** (documentación OpenAPI)

---

## Arquitectura

- Controllers delgados
- Lógica de negocio en **Services**
- Casos de uso mediante **Commands + Handlers**
- Mapping explícito con **Mapster**
- Manejo de errores mediante excepciones y middleware
- EF Core directo (sin repository por decisión de diseño)

---

## Requisitos

- .NET SDK 10
- Docker y Docker Compose
- SQL Server (local o en contenedor)

---

## Ejecución local

Desde la raíz del proyecto:

```bash
dotnet run
```
