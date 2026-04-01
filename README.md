# CF CLI
A fast and developer-friendly command-line tool for scaffolding Clean Architecture projects in .NET.  
CF CLI helps you generate a production-ready folder structure, boilerplate code, configuration layers, and best practices — all with a single command.

---

## 🚀 Features
- ⚡ **Instant scaffolding** of .NET Clean Architecture projects
- 🧱 **Pre-defined domain, application, infrastructure, and API layers**
- 🎯 **Best-practice templates based on real-world architecture**
- 📦 **Automatic file and folder generation**
- 🔧 **Extensible** — add your own templates or customize existing ones
- 🛠️ **Simple CLI commands**, easy to remember and use
- 🧹 **Opinionated but flexible** project structure

---

## 📥 Installation

Install CleanForge as a global .NET tool:

```bash
dotnet tool install -g CleanForge.Scaffold
```

To update to the latest version:

```bash
dotnet tool update -g CleanForge.Scaffold
```

---

## 💻 Usage

Scaffold a new Clean Architecture project:

```bash
cleanforge --name MyProject --output ./MyProject
```

| Option     | Description                          | Default              |
|------------|--------------------------------------|----------------------|
| `--name`   | Name of the project to generate      | `CleanForgeProject`  |
| `--output` | Output directory for the project     | Current directory    |

Then get started with your new project:

```bash
cd MyProject
dotnet restore
dotnet run --project MyProject.API
```

---

## 🏗️ Project Structure Generated

When you run the CF CLI to scaffold a new project, it generates the following folder structure:

```
MyCleanArchitectureProject/
├── MyCleanArchitectureProject.Domain/
├── MyCleanArchitectureProject.Application/
├── MyCleanArchitectureProject.Infrastructure/
├── MyCleanArchitectureProject.API/
└── MyCleanArchitectureProject.sln


```
- `Domain`: Contains the core business logic and entities.
- `Application`: Contains use cases, services, and application logic.
- `Infrastructure`: Contains data access, external services, and implementations.
- `API`: Contains the web API layer and controllers.

--- 
## 📋 Core Configurations Included
The generated project includes essential configurations for a Clean Architecture setup:
- **Dependency Injection** setup for managing service lifetimes and dependencies.
- **Logging** configuration for tracking application behavior.
- **Error Handling** middleware for consistent error responses.
- **MediatR** integration for handling commands and queries.
- **AutoMapper** setup for object-to-object mapping.
- **Fluent Validation structure** for validating requests.
- **Clean folder boundaries** to enforce separation of concerns.
- **Plug-and-play architecture** for easy extension and modification.
- **Docker Support** with Dockerfiles for containerizing the application.
   
---

## 🧱 Why CF CLI?

CF CLI was created to solve a common problem:

> **Developers often waste hours or days setting up Clean Architecture manually.**

**CF CLI solves that problem by offering:**
- ⚡ Extremely fast scaffolding
- 💯 Consistent structure across teams
- 🧩 Plug-and-play architecture
- 🧪 Test-ready layout
- 📝 Pattern discipline (CQRS, DI, SOLID, DDD-friendly)
- 🔄 No repetitive setup work

**Perfect for:**

- 👤 Solo developers
- 👥 Teams adopting Clean Architecture
- 🏢 Bootstrapping enterprise applications
- 🎓 Training new developers or onboarding juniors 

---
## 🤝 Contributing

Contributions are welcome!

If you'd like to contribute:

1. Fork the repo
2. Create a feature branch
3. Make your changes
4. Submit a pull request
5. Make sure your PR includes:
6. Clear description
7. Relevant screenshots if needed

---
## ⭐ Support This Project

If CF CLI saves you setup time or helps your team stay consistent, please consider:

- Starring ⭐ the repo
- Sharing it with other .NET developers
- Opening issues & PRs

Your support motivates future improvements!

