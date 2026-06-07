# Flight Search Demo – UI & API Setup

## 🚀 Overview
This project contains two components:

- **FlightSearchAPI** – ASP.NET Core Web API (run via IIS Express for Swagger)
- **flight-search-ui** – Vue.js frontend

This guide explains how to run both locally.

---

## 🛠 Start the API (IIS Express with Swagger)

### Requirements
- Visual Studio 2026 (Community or higher)
- .NET 10 SDK

### Steps
1. Open the solution in Visual Studio:
   ```
   FlightSearchAPI.sln
   ```
2. In the toolbar, select:
   **IIS Express** (not ProjectName)
3. Press **F5** or click **Start Debugging**.

Swagger UI will be available at:

```
https://localhost:44381/swagger
```

The exact port is shown in Visual Studio when the API starts.

---

## 💻 Start the UI

### Requirements
- Node.js (LTS recommended - I used v18.18.0)
- npm or yarn

### Steps
1. Navigate to the UI folder:
   ```bash
   cd flight-search-ui
   ```
2. Install dependencies:
   ```bash
   yarn install
   ```
3. Start the development server:
   ```bash
   yarn serve
   ```

The UI will run at something like:

```
http://localhost:8080
```

---

## 🔗 Connecting UI to API
The UI expects the API to be running locally under IIS Express.  
If needed, update the API base URL in your Vue service files.

---