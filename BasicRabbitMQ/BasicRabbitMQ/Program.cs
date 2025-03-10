using RabbitMQ.Client;
using System;
using System.Text;
namespace Send;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            var connection = factory.CreateConnection();
            Console.WriteLine("Bağlantı başarılı!");

            var channel = connection.CreateModel();
            channel.QueueDeclare("messageQueue", true, false, false);

            //QueueDeclare, eğer belirtilen adla bir kuyruk yoksa yeni bir tane oluşturur.
            //"messageQueue" → Kuyruğun adı
            //Parametreler:
            //true → Kalıcı bir kuyruk(mesajlar RabbitMQ yeniden başlatıldığında kaybolmaz).
            //false → Bu kuyruk sadece bu bağlantıya özel değil, başka bağlantılar da kullanabilir.
            //false → Eğer kuyruk boşsa otomatik silinmez.

            var message = "deneme birkiii";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "messageQueue",
                                 basicProperties: null,
                                 body: body);

            channel.WaitForConfirmsOrDie();
            //exchange: "" → Doğrudan kuyruğa mesaj gönderildiğini belirtir.
            //routingKey: "messageQueue" → Mesajın "messageQueue" adlı kuyruğa gitmesini sağlar.
            //basicProperties: null → Ekstra bir özellik eklenmemiş.
            //body: body → Gönderilecek mesajın içeriği.

            Console.WriteLine("Mesaj gönderildi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }

    }
}
