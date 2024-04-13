using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
//RabbitMQ sunucusuna bağlanmak için kullanılacak URI (Uniform Resource Identifier) belirlenir. 
factory.Uri = new Uri("amqps://xszhhevo:iTMLo1FgUGUKgaTpGHOiwlcVxF4pyidX@fish.rmq.cloudamqp.com/xszhhevo");

//Belirtilen bağlantı parametreleri ile RabbitMQ sunucusuna bir bağlantı oluşturulur ve bu bağlantı using bloğu içinde kullanılır. Bu, bağlantının kullanımı bittikten sonra otomatik olarak kapatılmasını sağlar.
using var connection = factory.CreateConnection();

//Bağlantı üzerinden bir kanal oluşturulur. Kanal, RabbitMQ üzerindeki iletişim için kullanılır.
var channel = connection.CreateModel();

channel.BasicQos(0, 1, false);

//"hello-queue" adında bir kuyruk (queue) tanımlanır. Bu kuyruk, mesajların geçici olarak depolandığı bir konum olarak kullanılır. Otomatik silme şuanda kapalı istediğimiz yerde silebiliriz bu şekilde channel.BasicAck(e.DeliveryTag, false);

channel.QueueDeclare("hello-queue", true, false, false);
//Not: Eğer burada kuyruğu oluşturduğumuz yerdeki true false parametrelerini publisherdaki yerde farklı kullanırsam uygulama hata verir !!!


var consumer = new EventingBasicConsumer(channel);
//Autoack parametresini eğer true yaparsak mesaj gönderildikten sonra direkt mesajı siler.Eğer false yaparsak silip silmemesini biz karar vereceğiz.
channel.BasicConsume("hello-queue", false, consumer);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine("Gelen Mesaj:" + message);


    channel.BasicAck(e.DeliveryTag, false);
};





Console.ReadLine();
