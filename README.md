Welcome to the Virtual University Platform, a modern online learning environment designed to connect instructors and students through high-quality digital educational content.

Our system allows teachers to upload and share course sessions in the form of videos, images, documents, and class notes, while students can easily purchase and access these materials anytime, anywhere.

Below is an overview of how the platform works and the roles available within it.


1. Super Admin

The Super Admin oversees the entire platform and has full access to all features. This includes:

Managing every university registered on the system
Access to all courses, classes, and their sessions
Viewing and managing all users (students, instructors, and administrators)
Monitoring financial transactions and activity logs across the platform
This role maintains overall system governance and ensures smooth operation of the virtual university network.

2. University Admin

Each university has at least one University Admin, responsible for managing everything related to their specific institution. Their main responsibilities include:

Reviewing and approving newly registered users (both students and instructors) after verifying their information
Posting announcements for students, instructors, or other administrators
Processing withdrawal requests submitted by instructors and students
Managing all courses, classes, and sessions in their university
Viewing activity and transaction history related to their institution
The University Admin ensures proper academic and financial flow within the university.

3. Professor

Once approved by the University Admin, an Professor can teach at any university inside the system. They can:

Create and manage courses, classes, and individual sessions
Upload session materials: videos, documents, images, and notes
Manage their enrolled students
View which sessions each student has purchased
Access their personal wallet, track their earnings, export financial reports to Excel, and request withdrawals
Instructors have the tools they need to deliver high‑quality digital education.

4. Student

A student is the main end‑user of the platform. After being approved by the University Admin, each student can:

Access courses offered in their registered university, academic program, and current semester
Choose a class and purchase individual class sessions
View all provided materials such as notes, documents, images, descriptions, and videosEach video can be watched up to three times and cannot be downloaded for security reasons.
Use their personal wallet to recharge and pay for sessions
Alternatively pay directly through the ZarinPal payment gateway
Request wallet withdrawals and view/manage their full transaction history
This creates a flexible and secure learning environment built for student success.

Additional Features
Two‑Factor Authentication
For added security, every login requires entering a verification code sent via SMS after providing national ID and password (SMS.ir).

Shopping Cart
Students can add multiple session purchases to their cart and pay in one transaction.

Tax Calculation System
All purchases and financial operations automatically include accurate tax calculations.

Nationwide Availability
The platform is fully scalable and ready to be deployed for any university across the country.

You can download & see screenshots of the any pages of the system here: https://cdn.imgurl.ir/uploads/w12507_BehAmooz_-_Screenshots.rar


Technical Overview
This project is a virtual university platform built with modern, enterprise‑grade .NET practices and a layered, domain‑driven architecture.

Technology Stack
Language: C#
Framework: .NET 10
UI Layer: ASP.NET Core Razor Pages
Database: Microsoft SQL Server
ORM & Data Access:
Entity Framework Core (EF Core)
LINQ for querying
Caching: Redis (for performance optimization and cache management)
Cloud Storage & Media:
AWS for general file and asset storage
ArvanCloud & ArvanPlayer for hosting and streaming images and videos
Architecture & Design
The system is designed using a clean, layered, onion‑style architecture with strong separation of concerns:

Core Layers:

Domain – Core business logic, entities, value objects, domain services, and domain events
Application – Use cases, application services, CQRS handlers, DTOs, and orchestration logic
Infrastructure – EF Core implementations, repository implementations, Redis caching, external services (AWS, ArvanCloud, ArvanPlayer, SMS, etc.)
Presentation – Razor Pages UI, controllers/handlers, view models, and HTTP endpoints
Additional Structure:

The solution contains 20+ projects, organized into multiple sub‑layers to keep domain, application, infrastructure, and presentation concerns separated and testable.
Design Patterns & Principles
Architectural Patterns:

Onion Architecture as the main architectural style
CQRS (Command Query Responsibility Segregation) for separating read and write concerns
Repository Pattern for abstracting data access and supporting testability
Methodologies & Principles:

DDD (Domain‑Driven Design):
The codebase is structured around domain concepts (bounded contexts, aggregates, entities, value objects).

Business rules and invariants are modeled in the domain layer, not in the UI or infrastructure.

SOLID Principles:
Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion are applied across services and abstractions.

OOP (Object‑Oriented Programming):
Encapsulation of business logic

Proper use of interfaces, abstractions, and composition

Security & Identity
Security is implemented and integrated throughout the platform:

Identity Management:Full user management based on .NET identity practices (or custom identity implementation where needed)
Authentication & Authorization:
Role‑based access control for the four main roles (Super Admin, University Admin, Instructor, Student)
Proper authorization checks across application and presentation layers
Two‑Factor Authentication:
SMS‑based 2FA during login (national ID + password + SMS code)
File & Media Management
File Storage:
Integration with AWS for file and document storage
Media Streaming:
Deep integration with ArvanCloud and ArvanPlayer for video hosting and streaming
Videos are protected against direct download, with controlled playback logic (e.g., max 3 plays per session)
