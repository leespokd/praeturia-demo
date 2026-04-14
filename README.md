# Loan Origination Service (.NET 8)

This project implements a simple loan origination service that accepts loan applications via a REST API and processes them asynchronously in the background based on eligibility rules.

---

## Tech Stack

- .NET 8 / ASP.NET Core
- Entity Framework Core (SQLite)
- BackgroundService for async processing
- xUnit for unit testing

---

## Features

- Submit loan applications via REST API
- Retrieve application status and decision logs
- Background processing of applications
- Eligibility rules evaluation
- Decision logging for each rule
- Basic logging and observability

---

## Running the Application

### Option 1: Run via Visual Studio (IIS Express)

1. Open `Praetura-demo.sln`
2. Set `Praetura-demo` as the startup project
3. Press `F5` or click **Run**

API will be available at:
https://localhost:{port}

---

### Option 2: Run via CLI

```bash
## Running the Application
```

### 1. Restore dependencies

```bash
dotnet restore
```

### 2. Build the solution

```bash
dotnet build
```

### 3. Apply database migrations (first run only)

```bash
dotnet ef database update --project src/Praetura-demo.csproj
```

### 4. Run the application

```bash
dotnet run --project src/Praetura-demo.csproj
```

### API will be available at:

```
http://localhost:5265/swagger/index.html
```

---

### Running tests

```bash
dotnet test
```

```

## API Endpoints

### POST /loan-applications

#### Request
```json
{
  "name": "Alice Example",
  "email": "alice@example.com",
  "monthlyIncome": 3500,
  "requestedAmount": 8000,
  "termMonths": 36
}
```

```json
{
  "id": "guid",
  "status": "Pending",
  "createdAt": "timestamp"
}
```

### GET /loan-applications/{id}

```json
{
  "id": "guid",
  "name": "...",
  "status": "Approved",
  "decisionLogs": [...]
}
```

### Eligibility Rules

Monthly income >= £2,000
Requested amount <= 4 × monthly income
Term between 12 and 60 months

### Background Processing
Runs every 60 seconds
Processes pending applications
Updates status to:
Approved
Rejected
Adds ReviewedAt timestamp
Logs decisions and errors

### Architecture Overview
Controllers - API layer
Services - Business logic
EF Core - Data access
BackgroundService - Async processing

### Trade-offs & Assumptions
SQL lite used
Single worker assumed
No idempotency
Basic validation only
If the system had to handle 5000000 applications per day, I would implement

1. horizontal scaling, so the system can grow in line with demand
2. use message brokers to process applications, to handle backfill and retry logic
3. I would automatically push applications to a queue on arrival, rather than polling, which would not be on the main thread
4. devise strategy to keep the database lean and performative, such as archival storage of past loan applications/database partitioning
5. any applications added to the queue for processing should immediately be marked as 'Processing' at the point of retrieval, to prevent duplicate processing
6. add any necessary database indexes where queries are made
7. Add a failed status to escalate any applications which repeatedly fail, and are not recoverable
8. rate limiting on API

Trade offs/more time to work on

1. In the background worker, I simply retrieved the oldest 20 loan applications, and processed. In reality these entries would likely be passed to a message broker in order to account for backfill and usage spikes, and also auto-retries on failure
2. In the background worker, I also assumed one worker running at any one time - in a real world scenario, we'd likely have to account for multiple workers running in parallel (if using this solution), so would need a solution (such as updating status to processing on retrieval of loan applications) to ensure workers don't process the same loans
3. In a real world system we would most likely implement more rules around loan applications, such as checking if the client already has a loan in progress
4. I would add customer entities to the database, and link each loan application/loan to a registered customer
5. I would implement authentication at the API level, using short lived access tokens and refresh tokens
6. I'd add a mechanism to alert users of loan application updates
7. I'd make create loan application idempotent
8. I generally use the results pattern to make failures explicit, and remove the need for exception throwing
9. Add additional decision rules, such as prevent request for a loan of zero, or max loan allowed
10. Add logic to prevent duplicate decision log entries being added to a loan application
11. In production, I'd add a solution level Dockerfile which includes all projects
12. I'd also add db indexes on commonly used properties in where clauses where applicable
    
