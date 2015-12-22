using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ecapseman
{
    class Server
    {
        static void Main(string[] args) {
            var EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7000);

            // Crea una socket di tipo TCP
            // per creare una connessione UDP occorre usare
            // esplicitamente la new Socket()
            TcpListener listener = new TcpListener(EP);
            listener.Start();
            Socket socket = listener.AcceptSocket(); // blocca

            Thread socketClientThread = new Thread(new ParameterizedThreadStart(Client));
            socketClientThread.Start(socket);

        }

        public static void Client(object o) {
            // opera sullo stream
            Socket socket = (Socket) o;
            using (Stream stream = new NetworkStream(socket))
            using (BinaryWriter writer = new BinaryWriter(stream))
            using (BinaryReader reader = new BinaryReader(stream)) {
                //invio la domanda
                writer.Write("1 + 1 ?");

                //ricevo la risposta
                int risposta = reader.ReadInt32(); //lettura sullo stream (bloccante)

                //controllo la risposta
                if (risposta != 2)
                    writer.Write("Sbagliata");
                else
                    writer.Write("Corretta");
            }

        }


    }
}
