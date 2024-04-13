
using RabbitMQ.Client;
using System.Text;


var factory = new ConnectionFactory();
//RabbitMQ sunucusuna bağlanmak için kullanılacak URI (Uniform Resource Identifier) belirlenir. 
factory.Uri = new Uri("amqps://xszhhevo:iTMLo1FgUGUKgaTpGHOiwlcVxF4pyidX@fish.rmq.cloudamqp.com/xszhhevo");

//Belirtilen bağlantı parametreleri ile RabbitMQ sunucusuna bir bağlantı oluşturulur ve bu bağlantı using bloğu içinde kullanılır. Bu, bağlantının kullanımı bittikten sonra otomatik olarak kapatılmasını sağlar.
using var connection = factory.CreateConnection();

//Bağlantı üzerinden bir kanal oluşturulur. Kanal, RabbitMQ üzerindeki iletişim için kullanılır.
var channel = connection.CreateModel();

//"hello-queue" adında bir kuyruk (queue) tanımlanır. Bu kuyruk, mesajların geçici olarak depolandığı bir konum olarak kullanılır.
channel.QueueDeclare("hello-queue", true, false, false);
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"Message {x}";
    // Mesaj, UTF-8 karakter kodlaması kullanılarak byte dizisine dönüştürülür. RabbitMQ, iletileri genellikle byte dizisi olarak alır.
    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

    Console.WriteLine($"Mesaj gönderilmiştir: {message}");
});

Console.ReadLine();






