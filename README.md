# ⚡ EV Charging Optimizer

> The EV Charging Optimizer is a full-stack web application that enables EV owners to reduce charging costs by automatically identifying the most cost-effective charging windows using real-time electricity spot market prices. The platform intelligently schedules charging sessions based on dynamic energy pricing, vehicle availability, and grid demand to maximize efficiency and minimize energy expenses.

🌐 **Live Demo (work in progress):** [https://ev-charging-optimizer.onrender.com/swagger](https://ev-charging-optimizer.onrender.com/swagger)

---

## 💡 The Problem

Electricity prices in Germany change every hour — sometimes varying 3–4x between the cheapest and most expensive hour. Most EV owners plug in immediately without realising they could save money by charging at a smarter time.

## ✅ The Solution

EV Charging Optimizer fetches live spot market prices, calculates how long your car needs to charge, and recommends the **cheapest consecutive window** before your deadline — automatically.

---

## 🚀 Tech Stack

| Layer | Technology |
|---|---|
| Frontend | React + Vite (JavaScript) |
| Backend | .NET 8 Web API (C#) |
| Database | PostgreSQL 16 |
| ORM | Entity Framework Core |
| Auth | JWT + BCrypt |
| Charts | Recharts |
| HTTP Client | Axios |
| Architecture | Clean Architecture |
| Deployment | Docker + Render |

---

## 📁 Project Structure

```
ev-charging-optimizer/
├── Dockerfile                         ← Docker config for Render deployment
├── docker-compose.yml                 ← Local PostgreSQL setup
├── EvChargingOptimizer.sln
├── .gitignore
├── README.md
├── frontend/                          ← React Frontend
│   └── src/
│       ├── api/api.js                 ← All API calls (single source of truth)
│       ├── components/Navbar.jsx
│       ├── pages/
│       │   ├── Login.jsx
│       │   ├── Register.jsx
│       │   ├── Dashboard.jsx          ← Price chart + optimizer + session history
│       │   ├── Vehicles.jsx
│       │   └── Stations.jsx
│       └── App.jsx                    ← React Router routes
└── src/
    ├── EvChargingOptimizer.Api/       ← Controllers, Program.cs, appsettings
    ├── EvChargingOptimizer.Application/  ← DTOs, Interfaces
    ├── EvChargingOptimizer.Domain/    ← Pure C# entities
    └── EvChargingOptimizer.Infrastructure/  ← Services, DbContext, Migrations
```

---

## 🧠 How the Optimizer Works

```
User inputs: vehicle + station + current battery % + target % + deadline
        ↓
Calculate energy needed (kWh) and charging duration (hours)
        ↓
If station.PricePerKwh = 0  →  Fetch spot prices → Sliding window algorithm
If station.PricePerKwh > 0  →  Fixed price × energy = total cost
        ↓
Return: cheapest start time, end time, estimated cost (EUR)
        ↓
Auto-save charging session to database
```

### Sliding Window Algorithm
Scans all 15-minute price slots between now and the deadline. Finds the consecutive block of the required duration with the lowest total electricity cost. O(n) time complexity.

### Home vs Public Charger

| Station `PricePerKwh` | Behaviour |
|---|---|
| `0` — Home charger | Uses live spot prices + sliding window to find cheapest time |
| `> 0` — Public charger | Uses fixed price, calculates cost immediately |

---

## ⚙️ Prerequisites (Local Development)

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)

---

## 🛠️ Getting Started (Local)

### 1. Clone the repository

```bash
git clone https://github.com/GitashreeMahato/ev-charging-optimizer.git
cd ev-charging-optimizer
```

### 2. Start the database

```bash
docker-compose up -d
```

### 3. Run database migrations

```bash
dotnet ef database update \
  --project src/EvChargingOptimizer.Infrastructure/EvChargingOptimizer.Infrastructure.csproj \
  --startup-project src/EvChargingOptimizer.Api/EvChargingOptimizer.Api.csproj
```

### 4. Start the backend API

```bash
cd src/EvChargingOptimizer.Api
dotnet run
```

- API: `http://localhost:5181`
- Swagger UI: `http://localhost:5181/swagger`

### 5. Start the frontend

```bash
cd frontend
npm install
npm run dev
```

- Frontend: `http://localhost:5173`

---

## 🔑 Configuration

**`src/EvChargingOptimizer.Api/appsettings.json`**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=evchargingdb;Username=postgres;Password=Admin1234!"
  },
  "Jwt": {
    "Key": "MySuperSecretKeyForEvChargingOptimizer2026!",
    "Issuer": "EvChargingOptimizer",
    "Audience": "EvChargingOptimizerUsers",
    "ExpiryHours": 24
  },
  "SpotPrice": {
    "ApiUrl": "https://spot.56k.guru/api/v2/hass?area=DE-LU&currency=EUR"
  }
}
```

---

## 📡 API Endpoints

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/Auth/register` | ❌ | Register new user |
| POST | `/api/Auth/login` | ❌ | Login, returns JWT token |
| GET | `/api/UserVehicles` | ✅ | List user's vehicles |
| POST | `/api/UserVehicles` | ✅ | Add a vehicle |
| PUT | `/api/UserVehicles/{id}` | ✅ | Update a vehicle |
| DELETE | `/api/UserVehicles/{id}` | ✅ | Delete a vehicle |
| GET | `/api/ChargingStations` | ✅ | List user's stations |
| POST | `/api/ChargingStations` | ✅ | Add a station |
| PUT | `/api/ChargingStations/{id}` | ✅ | Update a station |
| DELETE | `/api/ChargingStations/{id}` | ✅ | Delete a station |
| GET | `/api/ChargingSessions` | ✅ | List charging session history |
| GET | `/api/ElectricityPrices` | ✅ | List stored spot prices |
| GET | `/api/ExternalPrices/fetch-today` | ✅ | Manually fetch today's prices |
| POST | `/api/Optimizer/optimize` | ✅ | Get cheapest charging window |

### Optimizer Request Example

```json
{
  "vehicleId": 1,
  "stationId": 1,
  "currentBatteryPercent": 20,
  "targetBatteryPercent": 80,
  "batteryCapacityKwh": 75,
  "chargerPowerKw": 11,
  "deadline": "2026-03-17T08:00:00"
}
```

---

## 🗄️ Database Schema

| Table | Description |
|---|---|
| `Users` | Registered users (email + BCrypt password hash) |
| `UserVehicles` | User's EV vehicles (model, battery, connector) |
| `ChargingStations` | Home or public charging stations |
| `ElectricityPrices` | 15-minute spot price slots (DE-LU area, EUR) |
| `ChargingSessions` | Optimized charging session history |

---

## 🔄 Background Jobs

| Job | When | What it does |
|---|---|---|
| Startup fetch | Every app start | Fetches today's spot prices immediately |
| Daily fetch | 14:00 CET every day | Fetches next-day prices (available after 14:00) |
| Auto migrations | Every app start | Runs any pending EF Core migrations automatically |

---

## 🌍 Deployment (Render: fixing bug) 

| Component | Service | URL |
|---|---|---|
| Backend API | Render Web Service (Docker) | `https://ev-charging-optimizer.onrender.com` |
| Swagger UI | Render Web Service | `https://ev-charging-optimizer.onrender.com/swagger` |
| Database | Render PostgreSQL | Internal connection |
| Frontend | Render Static Site | Coming soon |

### Render Environment Variables

| Key | Description |
|---|---|
| `ConnectionStrings__DefaultConnection` | Render PostgreSQL connection string |
| `Jwt__Key` | JWT signing secret |
| `Jwt__Issuer` | JWT issuer |
| `Jwt__Audience` | JWT audience |
| `Jwt__ExpiryHours` | Token expiry in hours |
| `SpotPrice__ApiUrl` | External spot price API URL |
| `ASPNETCORE_ENVIRONMENT` | Set to `Production` |

---

## 🧰 Useful Commands

```bash
# Start local database
docker-compose up -d

# Stop local database
docker-compose down

# Run migrations
dotnet ef database update \
  --project src/EvChargingOptimizer.Infrastructure/EvChargingOptimizer.Infrastructure.csproj \
  --startup-project src/EvChargingOptimizer.Api/EvChargingOptimizer.Api.csproj

# Add new migration
dotnet ef migrations add MigrationName \
  --project src/EvChargingOptimizer.Infrastructure/EvChargingOptimizer.Infrastructure.csproj \
  --startup-project src/EvChargingOptimizer.Api/EvChargingOptimizer.Api.csproj

# Run API locally
cd src/EvChargingOptimizer.Api && dotnet run

# Run frontend locally
cd frontend && npm run dev

# Open local database shell
docker exec -it ev_charging_postgres psql -U postgres -d evchargingdb
```

---

## 🐛 Troubleshooting

| Issue | Fix |
|---|---|
| Cannot connect to database | Make sure Docker Desktop is running, port 5433 is free |
| `dotnet run` fails | Run `dotnet ef database update` first |
| CORS error in browser | Ensure backend is running on port 5181 |
| No prices on dashboard | Call `GET /api/ExternalPrices/fetch-today` manually |
| 401 Unauthorized | Log out and log back in to get a fresh JWT token |
| Duplicate stations/vehicles | Delete via psql: `DELETE FROM "ChargingStations" WHERE "Id" > 1` |
| Render API slow first load | Free tier sleeps after 15 min of inactivity — first request takes ~30s to wake |

---

## ✨ Features

- ✅ JWT Authentication — register, login, BCrypt password hashing
- ✅ Manage vehicles and charging stations
- ✅ Live spot electricity price fetching (DE-LU area, EUR)
- ✅ Sliding window algorithm — finds cheapest charging window
- ✅ Fixed-price support for public chargers
- ✅ Auto-save charging sessions after every optimization
- ✅ Price chart on dashboard (Recharts)
- ✅ Background daily price fetch at 14:00 CET
- ✅ Auto-run EF migrations on startup (production-ready)
- ✅ Docker deployment on Render
- ✅ Swagger UI with JWT authorization
- ⏳ React frontend deployment (coming soon)
- ⏳ Email/push notifications (planned)
- ⏳ Unit tests (planned)

---

## 📄 License

This project is for portfolio and educational purposes.