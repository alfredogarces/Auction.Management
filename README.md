# Auction Management System
## Design Decisions & Assumptions

### Architecture & Patterns
- The project follows **Clean Architecture (Onion Architecture)** to enforce clear separation of concerns between layers:
  - **Domain Layer**: Business entities and rules (`Auction`, `Vehicle`, `Bid`, etc.)
  - **Application Layer**: Services (`IAuctionService`) and DTOs 
  - **Infrastructure Layer**: In-memory repositories (mock data access)
  - **Presentation Layer**: Controllers (e.g., `AuctionController`)
- The **Repository Pattern** is used to abstract data access, supporting easy testability and infrastructure replacement.
- A `Result<T>` wrapper is used to encapsulate operation outcomes (success/failure) and avoid throwing exceptions for control flow.

### Domain Modeling
- Vehicles are represented by specific subclasses: `Truck`, `SUV`, `Sedan`, `Hatchback`, all inheriting from an abstract `Vehicle` base class.
- Each domain entity encapsulates its own validation (via `Validator` classes).
- Auctions are tied to a single vehicle and manage bids internally


### Error Handling
- Service methods return a `Result<T>` object containing either data or an `Error`.
- This enables consistent error handling throughout the system, and allows the API to easily translate failures into HTTP error responses.

### 📌 Assumptions
- One active auction per vehicle.
- Auctions must be explicitly started and closed.
- Bidders are uniquely identified by their email address.
- All data is stored in memory using simple lists (`InMemoryVehicleRepository`, `InMemoryAuctionRepository`), but this can be swapped with a database.



