

https://github.com/user-attachments/assets/b467f0b3-ea3d-405b-b394-eef94dd7c7af

# TaxSim-Agent

A hybrid full-stack simulation platform featuring a deterministic .NET backend for core calculations and a stochastic React frontend interface. Designed with a clean monorepo architecture, the application isolates backend business logic, database configurations, and client-side simulation modules to showcase robust system design and modern web development practices.

---

## Features

- **Deterministic .NET Backend**: High-performance core calculation engine ensuring reliable, reproducible tax computation logic.
- **Stochastic React Frontend**: Dynamic, probabilistic interface modeling variable user scenarios and simulation parameters.
- **Monorepo Architecture**: Clean isolation of backend business services, local database state, and frontend components within a unified workspace.
- **Local Data Persistence**: Integrated SQLite database management safely filtered out of version control.
- **Modular Design**: Loosely coupled API layer facilitating seamless client-server communication over local development endpoints.

---

## Getting Started

This project uses a hybrid architecture, combining a C# .NET API backend with a React Single Page Application (SPA) frontend.

### Prerequisites

- .NET SDK (**6.0** or later)
- Node.js and npm installed

---

## Installation

Clone the repository:

```bash
git clone https://github.com/DeveloperHeat/TaxSim-Agent.git
cd TaxSim-Agent
```

**Note:** You will need two terminal windows open simultaneously: one to run the .NET backend API, and one to run the React development server.

### 1. Run the .NET Backend

Navigate to the backend directory, restore dependencies, and start the server:

```bash
cd backend
dotnet restore
dotnet run
```

### 2. Run the React Frontend

Open a separate terminal window, navigate to the frontend directory, install dependencies, and launch the development server:

```bash
cd frontend
npm install
npm run dev
```

You can then:

- Interact with the stochastic simulation parameters directly through the browser UI.
- Inspect backend calculation requests logged live in your .NET terminal.

## Tech Stack

| **Component**    | **Technology**                                                 |
| ---------------- | -------------------------------------------------------------- |
| Backend Language | C# (.NET)                                                      |
| Frontend Library | React, JavaScript/TypeScript                                   |
| Database         | SQLite                                                         |
| Architecture     | Hybrid Full-Stack Monorepo                                     |
| Design Pattern   | Deterministic Calculation Engine + Stochastic Client Interface |

## Project Architecture

The system splits execution logic between a deterministic calculation pipeline and a stochastic client model:

```text
React Frontend (Stochastic UI / Parameters)
                  │
                  ▼ (HTTP / Localhost API)
      .NET Backend (Deterministic Engine)
                  │
                  ▼
       SQLite Database (Local State)
                  │
                  ▼
           Simulation Output
```

State and calculation rules are strictly managed on the backend layer, while user-driven probabilistic variants are handled dynamically on the client side.

## Extending The Project

Adding new simulation parameters or calculation endpoints follows a structured pattern:

1. **Backend Extensions**: Register new business logic handlers or database models inside the C# backend solution.
2. **API Contracts**: Expose endpoints via .NET controllers to serve the calculated metrics.
3. **Frontend Integration**: Consume the updated endpoints within your React frontend components to display new simulation metrics.

## Troubleshooting

### Port Conflicts

If you receive an error indicating that port `5000` or `3000` is already in use:

- Ensure no background instances of previous .NET applications or node servers are lingering.
- Kill active processes on those ports or update your configuration properties to bind to an available port.

## Future Improvements

- Expanded stochastic modeling variables for complex tax brackets
- User authentication and multi-session state management
- Advanced chart visualization dashboards using D3.js or Chart.js
- Comprehensive unit and integration test suites for the .NET calculation engine
- Docker containerization for seamless cross-environment deployment
