# Stock & Order Management

---

## 📖 Project Description

This project is an **Inventory Management System** designed to monitor product stock levels across stores within a supply chain. When stock falls below a critical threshold, the system automatically places an order from the most suitable supplier.

The system integrates with the external Fake Store API for product data, while maintaining its own internal product catalog. Products are matched with Fake Store items using a common `productCode` field.

---

## 🚀 How It Works

The project is structured into two microservices: Stock and Order, and includes three class libraries for:

• Shared message contracts  
• External service integration layer  
• Middleware layer 

The application starts from the Stock Microservice.

When a product falls below its critical stock threshold, the Stock Service identifies the low-stock items and sends them to the Order Microservice.

Communication between services is handled using MassTransit with RabbitMQ. The Stock service publishes low-stock product messages to a queue, and the Order service listens to that queue.

Upon receiving the request, the Order service attempts to create a purchase order by searching for the matching product on the external Fake Store API, selecting the item with the lowest available price.


                             +-----------------------------+
                             |      Fake Store API         |
                             | (Fetch & select cheapest)   |
                             +-------------▲---------------+
                                           |
                                HTTP GET   |
                               via HttpClient
                               +-----------+------------+
                               |      Order Service      |
                               |-------------------------|
                               | Listens to queue        |
                               | Gets productCode        |
                               | Queries Fake Store API  |
                               | Places cheapest order   |
                               +-----------▲-------------+
                                           |
                                   RabbitMQ Queue
                                   (via MassTransit)
                                           ▲
                               +-----------+-------------+
                               |    Stock Microservice    |
                               |--------------------------|
                               | POST /orders/check-and-place
                               | → Checks all products
                               | → Detects low-stock
                               | → Publishes events
                               +-----------▲-------------+
                                           |
                                User Input (POST /products)

---

## How to Run

1. Clone the repo and go to the project folder:
   git clone https://github.com/umutekici/InventoryFlow.git
   cd InventoryFlow

2. Run OrderService.API:
   cd OrderService.API
   dotnet run

3. Run StockService.API:
   cd StockService.API
   dotnet run

---

## Project Architecture

This project is structured into the following layers:

- **Shared message contracts**: Common data transfer objects (DTOs) and contracts shared between services for consistent communication.
- **External service integration layer**: Handles communication and abstraction with third-party APIs or systems.
- **Middleware layer**: Cross-cutting concerns like error handling, logging, and authentication handled in the request pipeline.

---

## Technologies Used

- .NET Core 8
- Swagger
- MassTransit
- RabbitMQ
- MemoryCache

---

## 🔧 API Endpoints

| Method | Endpoint                 | Description                                          |
|--------|--------------------------|------------------------------------------------------|
| POST   | `/products`              | Add a new product with name, threshold, and initial stock. |
| GET    | `/products/low-stock`    | List products that are below their critical stock level.    |
| POST   | `/orders/check-and-place`| Place orders for products that are below critical stock.    |

---

## Example Request / Response

#### POST `/products`  

**Request:**

json:
{
  "name": "Cotton T-Shirt",
  "category": "Clothing",
  "criticalStock": 20,
  "initialStock": 100
}

**Response:**

![image](https://github.com/user-attachments/assets/751a4356-e726-4512-b724-07bd4884b706)


#### POST `/products/low-stock`  

**Response:**

![image](https://github.com/user-attachments/assets/60c97b4f-b4ee-4341-bf02-53ecf2414ac7)


#### POST `/orders/check-and-place`  

![image](https://github.com/user-attachments/assets/2ea2fa49-8079-40cf-9346-e219c7ea298b)

After this operation, the order queue is successfully visible on RabbitMQ as shown in the image below:

![image](https://github.com/user-attachments/assets/ece28240-4941-4db2-8728-195ddcdb9f85)


The process of finding the product with the lowest price among the products on FakeAPI is performed in the code block below:

![image](https://github.com/user-attachments/assets/3b23a6ac-fe90-4690-aa5d-1b4a22097e9e)

The Id value in the JSON object from FakeStoreAPI is matched with the Code field in the product entities on StockService.



**Bonus Task**

![image](https://github.com/user-attachments/assets/63cfe2ce-0c87-49e6-8416-56df60f9fcb1)

This method converts a number between 1 and 11 to a Roman numeral by subtracting the largest Roman numeral values and appending the corresponding symbols to the result string.

Thank you for reading!

