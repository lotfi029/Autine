# 🧠 Autine – Graduation Project  

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-active-success)

**Autine** is an intelligent system designed to support children with Autism Spectrum Disorder (ASD) through an interactive, AI-driven environment.  
It allows users to communicate with a **trained chatbot** that helps them develop communication skills safely and effectively — all under the **supervision of specialized doctors** who can customize the chatbot’s settings according to each patient’s needs.

--- 

## 🎯 Project Goal  

The goal of **Autine** is to build a **virtualization system** that helps autistic patients prepare for real-world communication by interacting with an intelligent chatbot that simulates realistic conversations.  
Doctors and therapists can:  
- Adjust chatbot settings and dialogue scenarios based on the patient’s condition.  
- Monitor real-time chat sessions.  
- Analyze communication data to track improvement over time.

---

## ⚡ Performance Optimization  

To ensure scalability and fast response times, the system includes several optimization strategies:  
- **Optimized Queries:** Rewritten SQL queries for maximum efficiency.  
- **CQRS Implementation:** Separation of read/write operations to improve database performance.  
- **Caching:** Reused frequently accessed data to reduce database hits.  
- **SignalR Optimization:** Tuned WebSocket connections for low-latency real-time communication.  
- **Asynchronous Processing:** Utilized async/await to handle high concurrency smoothly.  
- **SOLID and Clean Code:** Ensured maintainable, efficient code that minimizes runtime overhead.

---

## 🚀 Features  

- 🤖 **Chatbot Integration** — Integrated with an external API for intelligent and personalized conversations.  
- 💬 **Realtime Communication (SignalR)** — Real-time interactions between users and supervisors.  
- 🧩 **CQRS Pattern** — Clear separation of commands and queries for better scalability.  
- ⚙️ **Service Pattern** — Organized business logic for better testability and structure.  
- 🏗️ **Repository Pattern + Unit of Work** — Clean data access management and transaction handling.  
- 🧱 **SOLID Principles** — Maintainable, flexible, and extensible architecture.  
- 🧩 **IoC & Dependency Injection** — Improved modularity and reduced coupling.  
- ⚡ **Optimized Queries** — Enhanced database performance and query execution time.

---

## 🧰 Tech Stack  

- **.NET 9 / C#**  
- **Entity Framework Core**  
- **SignalR**  
- **CQRS + MediatR**  
- **Repository & Service Layer**  
- **SQL Server**
- ** Hybird Cach **
- **Dependency Injection (built-in .NET IoC)**  

---

## 📂 Project Architecture  
Autine/
│
├── Autine.Domain/ # Entities and core domain logic
├── Autine.Application/ # CQRS handlers, DTOs, and interfaces
├── Autine.Infrastructure/ # Database context, repositories, and Unit of Work
├── Autine.API/ # API endpoints, controllers, and SignalR hubs
└── README.md # Project documentation

---

## 🧠 Design Principles  

- Separation of Concerns  
- Single Responsibility  
- Dependency Inversion  
- High Cohesion, Low Coupling  

---

## 🌍 Future Improvements

- 🧠 Integrate AI model training for adaptive chatbot behavior.

- ""🎙️ Add voice-based interaction for more natural communication.

- 📊 Build an analytics dashboard for doctors to track user progress.

- 💾 Enhance data security and privacy for medical data compliance.

---

## 🙌 Acknowledgments

- Special thanks to the supervising doctors and mentors for their valuable guidance and support in bridging technology with autism therapy. 

## 👨‍💻 Author

**Mohamed Lotfi**  
📧 [mohamed.lotfi.dev@gmail.com](mailto:mohamed.lotfi.dev@gmail.com)  
📞 +20 103 028 6574  
🔗 [LinkedIn Profile](http://linkedin.com/in/mohamedlotf)

