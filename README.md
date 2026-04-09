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
    
