using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer_Bellacci
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creo il mio socketlistener (server)
            //parametri
            //1) versione IP
            //2) tipo di socket (stream nel nostro caso)
            //3) che protocollo a livello di trasporto uso
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //configurazione IP dove ascoltare
            //usiamo opzione "Any", ascolta da ogni tipo di interfraccia del PC (wireless, ethernet ecc)
            IPAddress ipaddr = IPAddress.Any;

            //configurazione dell'endpoint
            //definizione della porta + IP
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            //collegamento = BIND
            //collego il listenerSocket all'endpoint
            //per funzionare il server ha bisogno di un endpoint
            listenerSocket.Bind(ipep);

            //metto in ascolto il server
            //il parametro è il numero massimo di connessioni da mettere in coda
            listenerSocket.Listen(5);

            Console.WriteLine("Server in ascolto...");
            Console.WriteLine("in attesa di connessione da pparte del client...");

            //è un istruzione bloccante
            //il programma rimane qui sino a che qualcuno non si connette
            //restituisce una variabile di tipo socket
            //nella variabile client salvo tutte le info del client che si è collegato
            Socket client = listenerSocket.Accept();

            //visualizzo le informazioni del client
            Console.WriteLine("Client IP: " + client.RemoteEndPoint.ToString());

            //sono pronto a ricevere un messaggio dal client
            //siccome è di tipo stream io riceverò un byte (solo uno perchè il tipo byte ne ha uno solo)
            //ricevo anche il numero di byte
            byte[] buff = new byte[128];
            int receivedBytes = 0;
            //la funzione "Receive" restituisce il numero di bite ricevuti
            //nel primo paraemtro vengono messi i byte effettivamente ricevuti
            receivedBytes = client.Receive(buff);
            Console.WriteLine("Numero di byte ricevuti: " + receivedBytes);
            //I bytes devono essere convertiti in stringa
            //Parametri: i byte, da dove inziare a convertirli (0), quanti convertirne
            //per fare queste cose serve aggiungere la using: System.Text;
            string receivedString = Encoding.ASCII.GetString(buff, 0, receivedBytes);
            Console.WriteLine("Stringa ricevuta: " + receivedString);

            //Inviare messaggio al client
            //per inviarlo riutilizzo lo stesso buffer
            //pulisco il buffer
            Array.Clear(buff, 0, buff.Length);
            receivedBytes = 0;

            //crea il messaggio
            string response = "Benvenuto " + client.RemoteEndPoint.ToString() + "! Al tuo servizio!\n" +
                                "Il tuo ultimo messaggio è stato: " + receivedString;

            //lo converto in byte
            buff = Encoding.ASCII.GetBytes(response);

            //Invio al client il messaggio
            client.Send(buff);

            //Termina il programma
            Console.ReadLine();
        }
    }
}
