# Auction Management System  
## Design Decisions & Assumptions

### Architecture & Patterns
- The project follows **Clean Architecture (Onion Architecture)**, ensuring a clear separation of concerns across layers:  
  - **Domain Layer**: business entities and rules (`Auction`, `Vehicle`, `Bid`, etc.). Entities encapsulate their own validations and internal logic, such as auction state management and bidding rules.  
  - **Application Layer**: application services (e.g., `IAuctionService`) that orchestrate use cases and handle DTOs.  
  - **Infrastructure Layer**: in-memory repositories (`InMemoryVehicleRepository`, `InMemoryAuctionRepository`, `InMemoryBidderRepository`) using thread-safe collections and synchronization primitives.  
  - **Presentation Layer**: controllers or API endpoints exposing the system.  

- The **Repository Pattern** abstracts data access, facilitating easy replacement of infrastructure and improved testability.  

- Operations return a generic `Result<T>` wrapper encapsulating success or failure, avoiding exceptions for normal control flow and enabling consistent error handling.

### Concurrency and Thread Safety
- In-memory repositories use `ConcurrentDictionary` to store entities in a thread-safe manner, ensuring safe concurrent access and updates.  
- Where more complex state changes or multi-step operations occur, **semaphores** are employed to guarantee atomicity and avoid race conditions.  
- This approach ensures that concurrent requests do not corrupt shared data, preserving data integrity without introducing heavy locking overhead.

### Domain Modeling
- Vehicles are represented by specific subclasses inheriting from an abstract `Vehicle` base class (`Truck`, `SUV`, `Sedan`, `Hatchback`), each with specific properties and validation logic.  
- Domain entities encapsulate business logic internally — for example, `Auction` manages auction lifecycle and bidding constraints.  

### Error Handling
- Service methods return `Result<T>` objects encapsulating either the data or an error, promoting uniform error handling throughout the system.  
- This pattern simplifies API responses by clearly signaling success or failure without relying on exceptions.

### 📌 Assumptions
- Only one active auction exists per vehicle at any given time.  
- Auctions must be explicitly started and ended before bids can be placed.  
- Bidders are uniquely identified by their email addresses.  
- Data is currently stored in-memory with thread-safe collections and synchronization, but the architecture supports easy migration to persistent storage.  
- Cloning of entities inside repositories ensures that consumers cannot inadvertently modify internal state, maintaining data integrity.
