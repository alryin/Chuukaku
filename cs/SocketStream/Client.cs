using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ecapseman
{
    class Client
    {
        static void Main(string[] args) {
            int serverPort = 7000;
            IPAddress serverIPAddress = IPAddress.Loopback;  // il localhost

            var tcpClient = new TcpClient();
            // la connessione UDP va definita usando esplicitamente la classe socket

            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort));
            using (Stream stream = tcpClient.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream)) {
                string domanda = reader.ReadString();
                Console.WriteLine(domanda);

                string risposta = Console.ReadLine();
                int rispostaInt;

                if (Int32.TryParse(risposta, out rispostaInt)){ 
                  writer.Write(rispostaInt); 
                }
                string risultato = reader.ReadString();

                Console.WriteLine("La tua risposta e' {0}", risultato);
            }
        }
    }
}
