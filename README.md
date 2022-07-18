# RabbitMQ-Demo
Microservices with RabbitMQ Message Bus demo using .Net 6 

1. In this project 3 services were used. (Product API, Order API, Report API ).
2. The Product API project send the message to Order API and Report when new product created. 
3. The Order API will send the messgae to Report API when new order Placed. 
4. The Report API was listing messages from Product and Order services and store it into the Database. 
