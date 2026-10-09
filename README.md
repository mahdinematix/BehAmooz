[Uploading BehAmooz_README.md…]()
# BehAmooz — Virtual University Platform

BehAmooz is a multi-university learning platform designed to help students access course material when they cannot attend classes in person. Instructors publish individual sessions and their learning materials, while students can purchase the sessions they need and access the content remotely.

The project was built as a substantial .NET application with a DDD-oriented Onion Architecture and a modular solution structure. It brings together academic workflows, user and access management, messaging, media delivery, and financial operations in one application.

> **Framework note:** The projects currently target **.NET 9** (`net9.0`). Update this documentation if the target framework is changed in a future revision.

## Key Features

### Academic and Content Management
- Multi-university structure with university-specific administration.
- Management of courses, classes, individual sessions, and session-related materials.
- Support for educational videos, documents, images, and class notes.
- Students can browse the sessions available to them and purchase individual sessions instead of paying for an entire course.

### Roles and Access
The platform defines four main roles:

- **Super Admin:** Oversees platform-wide users, universities, academic content, and operational activity.
- **University Admin:** Manages a specific university, reviews student/instructor registrations, publishes announcements, oversees academic content, and handles withdrawal requests.
- **Instructor:** Manages courses, classes, sessions, learning materials, enrolled students, and their financial activity.
- **Student:** Browses eligible classes, purchases sessions, accesses learning materials, and manages wallet and transaction activity.

The application uses cookie-based authentication and role-based authorization for protected areas. The intended sign-in flow also includes SMS verification through SMS.ir.

### Purchases and Financial Workflows
- Shopping cart for purchasing multiple sessions in one checkout.
- Wallet operations, deposits, purchases, withdrawal requests, settlement workflows, and transaction history.
- Integration with the ZarinPal payment gateway.
- Tax calculation as part of purchase and financial workflows.
- Instructor earnings and financial reporting, including Excel export.

### Messaging, Activity, and Integrations
- Role-based messaging and announcements.
- Activity/log management for administrative visibility.
- Redis integration for caching and OTP-related storage.
- AWS S3-compatible storage for general files and assets.
- ArvanCloud/ArvanPlayer integration for media hosting and video streaming.
- TUS-based upload service integration for media uploads.
- Email and SMS service abstractions.
- SignalR hub used by the application for upload-related real-time communication.

The intended video workflow restricts direct downloads and limits playback of purchased session videos. These controls should be reviewed and tested as part of any production deployment rather than treated as a substitute for a full security assessment.

## Architecture

BehAmooz follows a **DDD-oriented Onion Architecture** with modules separated by business responsibility. The Visual Studio solution contains **23 C# projects**.

Each main module is organized into projects for Domain, Application, Application Contracts, Infrastructure/EF Core, and Infrastructure Configuration. This structure separates business concepts and use cases from persistence details and application startup wiring.

### Main Modules

| Module | Responsibility |
| --- | --- |
| `AccountManagement` | User and account-related workflows, roles, and account/financial capabilities. |
| `StudyManagement` | Universities, courses, classes, sessions, and educational content workflows. |
| `MessageManagement` | Messages and announcements between platform roles. |
| `LogManagement` | Activity and operational log records. |
| `01_Framework` | Shared abstractions and cross-cutting integrations, including SMS, email, payment, storage, and shared infrastructure services. |
| `02_Query` | Read/query functionality that brings together data from multiple modules. |
| `ServiceHost` | ASP.NET Core entry point, Razor Pages/MVC presentation, dependency-injection registration, authentication, and authorization. |

### Design Principles and Patterns
- **Onion Architecture** to organize dependencies and isolate business logic.
- **Domain-Driven Design (DDD)** to structure business concepts and rules around domain models.
- **CQRS** to separate read and write responsibilities where appropriate.
- **Repository Pattern** to abstract persistence access.
- **SOLID and OOP** to support encapsulation, clear responsibilities, and maintainability.
- **Dependency Injection** to wire module services and infrastructure implementations.

## Technology Stack

- **Language and framework:** C#, .NET 9, ASP.NET Core
- **Presentation:** Razor Pages and MVC Areas
- **Database and ORM:** Microsoft SQL Server, Entity Framework Core 9, LINQ
- **Caching and temporary state:** Redis / StackExchange.Redis
- **File storage:** AWS SDK for S3-compatible object storage
- **Media:** ArvanCloud, ArvanPlayer, TUS-based uploads
- **Payments:** ZarinPal
- **Messaging services:** SMS.ir and email integration
- **Real-time communication:** SignalR
- **Reporting:** ClosedXML for Excel-based reports

## Screenshots

A screenshot archive of the application is available here:

[BehAmooz UI screenshots (RAR)](https://cdn.imgurl.ir/uploads/w12507_BehAmooz_-_Screenshots.rar)

## Getting Started

### Prerequisites

- .NET 9 SDK
- Microsoft SQL Server
- Redis available at `localhost:6379` (the current startup code connects to this address directly)
- Valid development configuration for the services you intend to exercise, such as S3-compatible storage, SMS, payment, and media providers

### Configure Local Settings First

The application reads its database connection from `ConnectionStrings:BehAmoozDb` and uses configuration sections for integrations such as `AWS`, `SmsIr`, and `Payment`. Do **not** use credentials committed to the repository. Before running the application:

1. Rotate any cloud-storage, SMS, payment, or database credentials that have been committed to Git; consider them compromised.
2. Remove secrets from tracked configuration files and Git history, then store local values in .NET User Secrets or environment variables.
3. Use the provider's sandbox/test mode for local development wherever possible.
4. Ensure Redis is running on `localhost:6379`, or update the startup code to load its endpoint from configuration.

For .NET configuration via environment variables, nested settings use double underscores. Examples of setting names (use your own values; never commit them):

```text
ConnectionStrings__BehAmoozDb
AWS__AccessKey
AWS__SecretKey
SmsIr__ApiKey
Payment__merchant
```

### Restore, Build, and Run

From the repository root:

```bash
dotnet restore BehAmooz.sln
dotnet build BehAmooz.sln
dotnet run --project ServiceHost/ServiceHost.csproj
```

Before the first run, initialize the SQL Server database using the EF Core migrations available in the relevant module projects. Because the solution contains multiple EF Core infrastructure projects, check the corresponding `DbContext`, migrations, and startup/target project when applying migrations.

## Configuration and Security Notes

- This is a substantial learning and development project, not a claim of a fully audited production service.
- Do not commit real credentials, connection strings, SMS tokens, or payment secrets.
- Rotate secrets exposed in previous commits and remove them from repository history; deleting them only in a new commit is not sufficient.
- Review authentication, authorization, file-upload limits, access to media, payment workflows, and transaction handling before deploying to a public environment.
- The current Redis endpoint is hard-coded to `localhost:6379` in startup code and should be moved to configuration for deployments.
- The project files currently target .NET 9 even if older project summaries mention .NET 10.

## Repository

[GitHub — mahdinematix/BehAmooz](https://github.com/mahdinematix/BehAmooz)
