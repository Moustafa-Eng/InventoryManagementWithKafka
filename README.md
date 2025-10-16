# 🏪 Inventory Management using Apache Kafka (.NET 9)

A practical example demonstrating how to integrate **Apache Kafka** with a **.NET 9 background service** to handle real-time inventory updates.

---

## 🚀 Project Overview

This project simulates a small-scale **Inventory Management System** where:

- The **Producer** publishes product stock updates to a Kafka topic named `InventoryUpdates`.
- The **Consumer** listens to that topic and processes updates in real time (e.g., logging, database update, etc.).

---

## ⚙️ Architecture

+----------------+ +-----------------+
| Inventory API | ---> | Kafka Broker 🧩 |
| (Producer) | | (Dockerized) |
+----------------+ +-----------------+
↓
+-----------------+
| BackgroundService|
| (Consumer) |
+-----------------+
د

- **Producer** → sends messages (inventory updates)
- **Kafka Broker** → handles message delivery
- **Consumer** → processes messages asynchronously using `BackgroundService`

---

## 🧰 Tech Stack

| Component | Description |
|------------|-------------|
| **.NET 9** | For building Producer & Consumer apps |
| **Confluent.Kafka** | Kafka client library for .NET |
| **Docker** | To host Kafka broker locally |
| **KRaft mode** | Kafka running *without ZooKeeper* |

---

## 🐳 Running Kafka via Docker

Make sure Docker is running, then inside your `kafka-kraft` directory run:

```
docker compose up -d
✅ This starts Kafka on:

Broker: localhost:9092

KRaft mode: enabled (no ZooKeeper required)

💻 Running the .NET Projects
Clone the repository:
git clone https://github.com/Moustafa-Eng/InventoryManagementKafka.git
cd InventoryManagementKafka
Run the Producer project:


dotnet run --project InventoryManagement.Producer
Run the Consumer project:


dotnet run --project InventoryManagement.Consumer
🧩 Kafka Configuration (appsettings.json)
Example appsettings.json for both projects:


{
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "Topic": "InventoryUpdates"
  }
}
📨 Producer Example

var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

using var producer = new ProducerBuilder<Null, string>(config).Build();

var message = JsonSerializer.Serialize(new { ProductId = 101, Quantity = 15 });
await producer.ProduceAsync("InventoryUpdates", new Message<Null, string> { Value = message });

Console.WriteLine("📦 Inventory update sent!");
🔁 Consumer Example

protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    _consumer.Subscribe("InventoryUpdates");

    try
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var cr = _consumer.Consume(stoppingToken);
            _logger.LogInformation($"Received update: {cr.Message.Value}");
        }
    }
    catch (OperationCanceledException) { }
    finally
    {
        _consumer.Close();
    }
}
🧠 Notes
Kafka must be running before you start the .NET apps.

Ensure both producer and consumer use the same topic name (InventoryUpdates).

Works without ZooKeeper (KRaft mode).

If using Virtual Machine, expose port 9092 to host.

```
🧑‍💻 Author

Mustafa Khaled

Software Engineer (.NET Core, Node.js, Angular, React)

🏷️ License

MIT License © 2025
