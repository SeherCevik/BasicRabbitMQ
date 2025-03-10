
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

internal class Program
{
    //RabbitMQ Consumer(Tüketici) uygulamasıdır."messageQueue" kuyruğundaki mesajları dinleyip almak için kullanılır.
    static void Main(string[] args)
    {
        try
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            channel.QueueDeclare("messageQueue", true, false, false);

            //kuyruktan mesajı alma
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: "messageQueue",
                                 autoAck: true,
                                 consumer: consumer);
            //         queue =>   Dinlenecek kuyruğun adı
            //                AutoAck =>  Mesajın otomatik olarak onaylanıp onaylanmayacağını belirler.
            //true olursa, mesaj alındığında RabbitMQ'ya "işlem tamam" bilgisi otomatik gider ve mesaj silinir.
            //false olursa, tüketici(consumer) mesajı işlediğini elle bildirmelidir(BasicAck ile).
            //                consumer => RabbitMQ'dan gelen mesajları alacak ve işleyecek tüketici nesnesidir (EventingBasicConsumer).
            

            //consumer ı dinlemek
            //Tüketici (consumer), kuyruktan yeni mesaj geldiğinde Consumer_Received metodunu çalıştıracak.
            consumer.Received += Consumer_Received;

            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    private static void Consumer_Received(object? sender, BasicDeliverEventArgs e)
    {
        //yakalayacağımız veriyi binary den string e çevirme
        Console.WriteLine("Gelen Mesaj: " + Encoding.UTF8.GetString(e.Body.ToArray()));
    }
}
