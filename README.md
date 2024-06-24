# Fintech Application with Microservices Architecture

This project is a fintech application built with .NET 8, utilizing microservices architecture with Docker, Docker Compose, RabbitMQ, PostgreSQL, EF Core, and C#. The application consists of 5 microservices: 3 are web APIs and 2 are console applications.

# Diagram

![Fintech](https://github.com/ImesashviliIrakli/Fintech/assets/77686006/c2007582-dda5-486d-a03c-a42b0100499b)

## Services Overview

1. **IdentityService**
   - Responsible for managing company registrations and issuing API keys and secrets.
   - Accessible at: `localhost:5001/swagger`

2. **OrderService**
   - Manages the creation and processing of orders.
   - Communicates with IdentityService for authentication.
   - Accessible at: `localhost:5003/swagger`

3. **PaymentService**
   - Handles payment processing for orders.
   - Utilizes RabbitMQ for communication with OrderService.
   - Accessible at: `localhost:5005/swagger`

4. **ServiceA and ServiceB**
   - Mock services that simulate transaction processing based on the last digit of the card number.

## Prerequisites

Before running the application, ensure you have the following installed and configured:

- **Docker Desktop** for Windows
- **PostgreSQL** with setup for username and password.

## How to Run

1. **Update Databases:**
- Run `update-database` command for each of the services (IdentityService, OrderService, PaymentService) to create necessary tables.

2. **Postgre UserName/Password Change:**
   - In the `.env` file located in the root directory of the solution, update the Postgre UserName and Password, and if your port is different change that too. 

3. **Build and Start Docker Containers:**
- Open a terminal in the solution folder and run:
  ```
  docker-compose build
  docker-compose up
  ```
- Wait for the services to start (may take up to 2 minutes).

3. **Access Services:**
- Once Docker containers are up, access the following URLs:
  - IdentityService Swagger UI: `localhost:5001/swagger`
  - OrderService Swagger UI: `localhost:5003/swagger`
  - PaymentService Swagger UI: `localhost:5005/swagger`
  - RabbitMQ Management UI: `localhost:15672` (username: guest, password: guest)

## Usage

1. **IdentityService:**
- Create a new company to obtain API Key and API Secret.

2. **OrderService:**
- Use obtained credentials from IdentityService to create new orders.
- Access `/orders` endpoint to create orders.

3. **PaymentService:**
- Use credentials from IdentityService to process payments.
- Access `/process` endpoint to initiate payment processing with orderId, cardnumber, and expiry date.

4. **ServiceA and ServiceB:**
- Mock services triggered by PaymentService based on the card number's last digit, simulating transaction outcomes.

## Notes

- The services are designed to demonstrate a simplified fintech application flow.
- Ensure all dependencies are correctly configured and running before using the application.
