using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ZedGraph;

namespace Peasant
{

    public partial class StartingForm : Form
    {

        String path = ""; // Path per dove salvare i .csv di output. Deciso dall'utente prima di leggere l'input stream.
        float[,,] sampwin; // Matrice tridimensionale: numeroSensori(0), numeroCampionamenti(1), numeroAttributi(2)
        float[] angle_helper = { 0, 0 }; // Array di supporto per il riconoscimento delle girate.
        float[] position_helper = { 0, 0 }; // Array di supporto per l'algoritmo di Dead Reckoning.
        int sensor_number; // Numero del sensore di cui visualizzare i dati.
        double read_window; // Indica l'ultimo campionamento totale che si è arrivati a processare (il settaggio avviene prima della fase di processing).
        double frequency; // Frequenza di invio dati. Coincide con quella di campionamento.
        bool eof; // Per riconoscere l'eof del file grezzo.
        int[] time = new int[5]; // Array che contiene per ogni grafico (cinque totali) il numero di punti inseriti.
        string[,] events = new string[3, 2]; // Struttura dati ausiliaria per il riconoscimento degli eventi cross-windows.
        float precAngle = 0; // Variabile di supporto per il plotting degli angoli theta. Rappresenta l'ultimo valore dell'array angle della finestra precedente.
        float precGraphAngle = 0; // Variabile di supporto per il plotting degli angoli theta. Rappresenta l'ultimo valore dell'array graphAngle della finestra precedente.

        // Dichiarazione dei delegati con signature corrispondente.
        delegate void button_change(bool b);
        delegate void Box(string s, bool b, Color color);
        delegate void startgraph(ZedGraphControl zgc, int num);
        delegate void refresh(ZedGraphControl zgc);

        TcpListener listener;
        Socket client;
        NetworkStream stream;
        BinaryReader reader;
        DateTime date; // Data iniziale da cui calcolare dinamicamente il tempo quando necessario, i.e durante il riconoscimento degli eventi.

        public StartingForm()
        {
            InitializeComponent();
        }

        // Handler del caricamento della Windows Form.
        private void StartingForm_Load(object sender, EventArgs e)
        {
            // Creazione dei grafici.
            CreateGraph(zedGraphControl1, new string[] { "Acceleration Modulus Graph", "Time", "Acceleration", "Acceleration" }, Color.Red);
            CreateGraph(zedGraphControl2, new string[] { "Gyroscope Modulus Graph", "Time", "Angular Velocity", "Angular Velocity" }, Color.DarkBlue);
            CreateGraph(zedGraphControl3, new string[] { "Theta Angles", "Time", "Theta", "Theta" }, Color.DarkGreen);
            CreateGraph(zedGraphControl4, new string[] { "Body Position Graph", "Time", "Body Position", "Body Position" }, Color.DarkMagenta);
            CreateGraph(zedGraphControl5, new string[] { "Dead Reckoning", "XAxis (m)", "YAxis (m)", "Position" }, Color.DodgerBlue);

            richTextBox1.Text = "Disconnesso";
            richTextBox1.ForeColor = Color.Crimson;

            // Si esegue Sending_Server solo se non sono presenti altre istanze del processo.
            if (!(Process.GetProcessesByName("Sending_Server").Length > 0)) Process.Start(@"Sending_Server.exe");
        }

        // Handler di un click sul bottone ReadData.
        private void ReadData_Click(object sender, EventArgs e)
        {
            // Salvataggio del path da parte dell'utente. Se si chiude la finestra il path è scelto per default.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "File csv|*.csv";
            saveFileDialog1.Title = "Salva i dati";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName.Equals("")) path = @"C:\Temp\" + DateTime.Now.ToString("yyyy MMMMMMMMMMMMM dd HH_mm") + ".csv";
            else path = saveFileDialog1.FileName;

            Thread streaming = new Thread(new ThreadStart(Streaming));
            streaming.Start(); // Avvio thread principale per la ricezione dati e l'avvio delle operazioni lato server.
        }

        // Handler di un click sul bottone Stop.
        private void Stop_Click(object sender, EventArgs e)
        {
            Self_Destruction();
        }

        // Handler di un click sul bottone OpenCsv.
        private void OpenCsv_Click(object sender, EventArgs e)
        {
            if (SampCsv.Checked == true) Process.Start(path);
            if (EventsCsv.Checked == true) Process.Start(path.Insert(path.Length - 4, "_Events"));
        }

        // Handler dell'hover sulla legenda.
        private void legenda_MouseHover(object sender, EventArgs e)
        {
            legenda.Size = new System.Drawing.Size(63, 83);
            legenda.Text = "Legenda: \n \n 0.0: Lay \n 0.5: LaySit \n 1.0: Sit \n 1.5: Stand";
        }

        // Handler della uscita del cursore dalla legenda.
        private void legenda_MouseLeave(object sender, EventArgs e)
        {
            legenda.Size = new System.Drawing.Size(55, 16);
            legenda.Text = "Legenda: ";
        }

        // Handler dell'hover sulla legenda.
        private void legenda_2_MouseHover(object sender, EventArgs e)
        {
            legenda_2.Size = new System.Drawing.Size(83, 59);
            legenda_2.Text = "Legenda: \n \n Cerchio: Inizio \n Quadrato: Fine";
        }

        // Handler della uscita del cursore dalla legenda.
        private void legenda_2_MouseLeave(object sender, EventArgs e)
        {
            legenda_2.Size = new System.Drawing.Size(55, 16);
            legenda_2.Text = "Legenda: ";
        }

        // Metodo per la gestione dei messaggi di console del server. 
        private void BoxSet(string s, bool b, Color color)
        {
            if (b)
            {
                richTextBox1.ForeColor = color;
                richTextBox1.AppendText(s);
                richTextBox1.Update();
            }
            else
            {
                richTextBox1.ForeColor = color;
                richTextBox1.Text = s;
                richTextBox1.Update();
            }
        }

        // Metodo per la gestione della disponibilità dei bottoni, textboxes e caselle di selezione.
        private void ButtonChange(bool b)
        {
            ReadData.Enabled = b;
            textBox1.Enabled = b;
            numericUpDown1.Enabled = b;
            OpenCsv.Enabled = b;
        }

        // Metodo per la gestione della disponibilità del bottone di stop.
        private void StopEnabler(bool b)
        {
            Stop.Enabled = b;
        }

        // Metodo per il refreshing dei dati graficati.
        private void RefreshGraph(ZedGraphControl zgc)
        {
            zgc.Refresh();
        }

        // Procedura principale.
        private void Streaming()
        {
            // Dichiarazione ed inizializzazione dei delegati.
            button_change change = new button_change(ButtonChange);
            button_change stopEnabler = new button_change(StopEnabler);
            Box logText = new Box(BoxSet);
            startgraph StartGraph = new startgraph(startGraph);

            Invoke(change, false);
            Invoke(logText, "In Attesa Di Una Connessione", false, Color.Orchid);

            // Reset delle variabili.
            for (int i = 0; i < 2; i++)
            {
                angle_helper[i] = 0;
                position_helper[i] = 0;
            }

            read_window = 0;
            precAngle = 0;
            precGraphAngle = 0;

            // Valori sentinella che sostituiscono il rozzo null.
            events[0, 1] = "Ghepardo Treddoso"; // So cute! :3
            events[1, 1] = "Squalodonte Ricorsivo"; // Adorable! <3
            events[2, 1] = "Delfino Iterativo"; // Aww, I'd cuddle up with him! (*^▽^*)

            // Ritorno dei grafici allo stato iniziale prima di leggere e processare nuovi dati.
            Invoke(StartGraph, zedGraphControl1, 0);
            Invoke(StartGraph, zedGraphControl2, 1);
            Invoke(StartGraph, zedGraphControl3, 2);
            Invoke(StartGraph, zedGraphControl4, 3);
            Invoke(StartGraph, zedGraphControl5, 4);

            // Reset dell'array time solo dopo le soprascritte chiamate.        
            for (int i = 0; i < 5; i++) time[i] = 0;

            int port;
            sensor_number = (int)numericUpDown1.Value;

            try
            {
                port = int.Parse(textBox1.Text);
            }
            catch (System.FormatException)
            {
                port = 0;
            }

            // La porta non deve essere tra quelle standard.
            if (port <= 4269 || port >= 65525)
            {
                MessageBox.Show("Errore: inserire numero di porta compreso tra 4269 e 65525", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Invoke(change, true);
                return;
            }

            // Dimensione della finestra dinamica.
            const int window = 200;

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            listener = new TcpListener(ipAddress, port); // Crea una socket di tipo TCP.
            listener.Start();

            client = listener.AcceptSocket();   // In attesa del client...
            stream = new NetworkStream(client); // Apre lo stream con il client.
            reader = new BinaryReader(stream); // Oggetto di tipo BinaryReader per leggere con certezza tutti i byte.

            Invoke(logText, "Connesso... \nIn Attesa Di Ricevere Dati...", false, Color.Green);

            /**
            Questo passaggio legge a quale frequenza emulata viene spedito il file.
            Frequenza emulata e frequenza di campionamento non corrispondono necessariamente.
            Con Xbus le due frequenze coincidono.
            */
            byte[] stream_frequency = new byte[4];
            try
            {
                reader.ReadBytes(10); // Salto i primi 10 bytes (ID dispositivo trasmittente).
                stream_frequency = reader.ReadBytes(4); // Leggo la frequenza.
            }
            catch (Exception ex)
            {
                Self_Destruction();
            }
            frequency = stream_frequency[0]; // Leggo solo il primo byte perchè il numero è minore di 2^8.

            int byte_length; // Dimensione dati da leggere e copiare.
            int sensors; // Numero di sensori.
            byte[] package; // Pacchetto dati.
            byte stream_length; // Variabile di supporto per il campo dati.
            byte[] stream_Ext_Length = new byte[2];
            eof = false;

            /** 
            Cerca la sequenza FF-32 che identifica il campo dati.
            0x32 MID 0xFF BID
            */
            byte[] tmp = new byte[3];
            while (!(tmp[0] == 0xFF && tmp[1] == 0x32))
            {
                try
                {
                    tmp[0] = tmp[1];
                    tmp[1] = tmp[2];
                    byte[] read = reader.ReadBytes(1);
                    tmp[2] = read[0];
                }
                catch (Exception ex)
                {
                    Self_Destruction();
                }
            }

            stream_length = tmp[2]; // tmp[2] contiene il byte del campo LEN.

            /**
            Lunghezza di byte da leggere:
            Il primo se in modalità normale.
            Il secondo in modalità Extended Length (5 a 8 sensori).
            */
            if (stream_length != 0xFF) byte_length = stream_length;
            else
            {
                try
                {
                    stream_Ext_Length = reader.ReadBytes(2); // Leggo i due bytes aggiuntivi per la lunghezza.
                }
                catch (Exception ex)
                {
                    Self_Destruction();
                }
                byte_length = (stream_Ext_Length[0] * 256) + stream_Ext_Length[1]; // Lunghezza di byte modalità estesa considerando i due bytes aggiuntivi.  
            }

            byte[] data = new byte[byte_length + 1];
            try
            {
                data = reader.ReadBytes(byte_length + 1); // Lettura effettiva dei dati.
            }
            catch (Exception ex)
            {
                Self_Destruction();
            }

            sensors = (byte_length - 2) / 52; // Causa due byte di contatore -> (byte_length - 2). 

            if (stream_length != 0xFF) package = new byte[byte_length + 4]; // +4 per il BID, MID e LEN.
            else package = new byte[byte_length + 6]; // +6 per il BID, MID, LEN ed i due bytes successivi della modalità estesa.

            /**
            Copia primi elementi.
            */
            package[0] = 0xFF; // BID.
            package[1] = 0x32; // MID.
            package[2] = stream_length; // LEN.

            // Copia dei dati.
            if (stream_length != 0xFF) data.CopyTo(package, 3);
            else
            {
                package[3] = stream_Ext_Length[0];
                package[4] = stream_Ext_Length[1];
                data.CopyTo(package, 5);
            }

            /* 
            Numero di bytes da leggere per ogni sensore.
            L'array conterrà per ogni sensore l'offset di inizio di tutta la sequenza.
            */
            int[] t = new int[8];

            List<List<float>> array = new List<List<float>>();

            // Calcolo per ogni sensore a quale byte devo iniziare a leggere (compresi nel calcolo i bytes di comunicazione e quelli già letti dei sensori precedenti).            
            for (int i = 0; i < sensors; i++)
            {
                array.Add(new List<float>());
                t[i] = 5 + (52 * i);
            }

            Invoke(stopEnabler, true);

            /**
            Interpretazione dati e salvataggio.	
            */
            while (!eof)
            {
                for (int i = 0; i < sensors; i++)
                {
                    byte[] temp = new byte[4]; // Per conversione a float.                  
                    for (int tr = 0; tr < 13; tr++)
                    { // I campi sono 13, quindi quanti valori/volte per sensore nel singolo pacchetto devo convertire a float.
                        if (sensors < 5)
                        {
                            temp[0] = package[t[i] + 3]; // Lettura inversa byte per byte.
                            temp[1] = package[t[i] + 2];
                            temp[2] = package[t[i] + 1];
                            temp[3] = package[t[i]];

                        }
                        else
                        {
                            temp[0] = package[t[i] + 5]; // Lettura inversa byte per byte.
                            temp[1] = package[t[i] + 4];
                            temp[2] = package[t[i] + 3];
                            temp[3] = package[t[i] + 2];
                        }

                        float valore = BitConverter.ToSingle(temp, 0); // Conversione a float.
                        array[i].Add(valore);

                        /**
                        Creazione della matrice sampwin, copiando i dati dalla lista di lista.
                        */
                        if ((array[sensors - 1].Count()) / 13 == window)
                        {
                            sampwin = new float[sensors, window, 13];
                            for (int x = 0; x < sensors; x++)
                            {
                                int position = 0;
                                for (int y = 0; y < window; y++)
                                {
                                    for (int z = 0; z < 13 && position < array[1].Count; z++, position++)
                                    {
                                        sampwin[x, y, z] = array[x][position];
                                    }
                                }
                            }

                            // Reset della lista di lista.
                            array = new List<List<float>>();
                            for (int ind = 0; ind < sensors; ind++) array.Add(new List<float>());

                            // Se l'esecuzione si trova allo stato iniziale (prima finestra), si setta al tempo corrente la variabile "date" e la prima colonna della struttura ausiliaria.
                            if (read_window == 0)
                            {
                                date = DateTime.Now;
                                for (int k = 0; k < 3; k++) events[k, 0] = date.ToString("hh:mm:ss");
                            }
                            read_window = read_window + window; // Incremento read_window.
                            Invoke(logText, "Pacchetti Letti: " + read_window + " \n" + "Tempo Trascorso: " + (double)(read_window * (double)(1 / frequency)) + " Secondi\n", false, Color.DarkGoldenrod);
                            Computator(); // Chiamata al gestore delle computazioni.
                        }

                        t[i] += 4; // Incremento al successivo valore da leggere (4 bytes dopo).
                    }
                }

                // Reinizializzazione dell'offset.
                for (int i = 0; i < sensors; i++) t[i] = 5 + (52 * i);

                // Lettura pacchetto seguente.
                if (sensors < 5)
                {
                    try
                    {
                        package = reader.ReadBytes(byte_length + 4);
                    }
                    catch (Exception ex)
                    {
                        Self_Destruction();
                    }
                }
                else
                {
                    try
                    {
                        package = reader.ReadBytes(byte_length + 6);
                    }
                    catch (Exception ex)
                    {
                        Self_Destruction();
                    }
                }

                // Gestione eof e chiusura stream.
                try
                {
                    if (package[0] != 0xFA) { }
                }
                catch (Exception ex)
                {
                    Self_Destruction();
                }
            }

            if (eof)
            {
                int numerosity = array[sensors - 1].Count(); // Conta i valori contenuti nella lista associata al sensore.
                int rows = (numerosity / 13); // Conta quanti campionamenti hanno effettuato i sensori (numero uguale per tutti).

                sampwin = new float[sensors, rows, 13];

                /**
                Creazione della matrice sampwin, copiando i dati dalla lista di lista. 
                Il caso seguente viene esegutio quando il numero di elementi è inferiore alla dimensione della finestra.
                */
                for (int x = 0; x < sensors; x++)
                {
                    int position = 0;
                    for (int y = 0; y < rows; y++)
                    {
                        for (int z = 0; z < 13; z++)
                        {
                            sampwin[x, y, z] = array[x][position];
                            position++;
                        }
                    }
                }

                // Se l'esecuzione si trova allo stato iniziale (prima finestra), si setta al tempo corrente la variabile "date" e la prima colonna della struttura ausiliaria.
                if (read_window == 0)
                {
                    date = DateTime.Now;
                    for (int k = 0; k < 3; k++) events[k, 0] = date.ToString("hh:mm:ss");
                }

                read_window = read_window + rows; // Incremento read_window.
                Invoke(logText, "Pacchetti Letti: " + read_window + " \n" + "Tempo Trascorso: " + (read_window * (1 / frequency)) + " Secondi\n", false, Color.DarkGoldenrod);

                Computator(); // Chiamata al gestore delle computazioni.
            }

            DateTime finish = DateTime.Now;
            TimeSpan duration = finish.Subtract(date);

            Invoke(logText, "Disconnesso\n", true, Color.OrangeRed);
            Invoke(logText, "Tempo Totale Esecuzione: " + duration.ToString("mm\\:ss\\.ffff"), true, Color.OrangeRed);
            Invoke(stopEnabler, false);
            Invoke(change, true);
        }

        private void Computator()
        {
            refresh _refresh = new refresh(RefreshGraph);

            // Creazione dei threads.
            Thread accGraph = new Thread(new ThreadStart(ModAccGraph));
            Thread gyrGraph = new Thread(new ThreadStart(ModGyrGraph));
            Thread motion = new Thread(new ThreadStart(Motion));
            Thread turn = new Thread(new ThreadStart(Turn));
            Thread laysit = new Thread(new ThreadStart(LaySit));
            Thread deadRec = new Thread(new ThreadStart(DeadReckoning));
            Thread savedata = new Thread(new ThreadStart(SaveData));

            // Run!
            deadRec.Start();
            accGraph.Start();
            gyrGraph.Start();
            motion.Start();
            turn.Start();
            laysit.Start();
            savedata.Start();

            // Il chiamante attende che i threads completino le loro operazioni.
            while (accGraph.IsAlive || gyrGraph.IsAlive || turn.IsAlive || laysit.IsAlive || deadRec.IsAlive) ;

            // Refresh dei grafici.
            Invoke(_refresh, zedGraphControl1);
            Invoke(_refresh, zedGraphControl2);
            Invoke(_refresh, zedGraphControl3);
            Invoke(_refresh, zedGraphControl4);
            Invoke(_refresh, zedGraphControl5);

            // Non vi è bisogno che i seguenti due threads completino le loro operazioni prima di fare il refresh dei grafici, per cui vi è un ciclo loro.
            while (savedata.IsAlive || motion.IsAlive) ;
        }

        // Metodo per il plotting del modulo dell'accelerazione.
        private void ModAccGraph()
        {
            float[] modAcc = Functions.Mod(sampwin, sensor_number - 1, 0, 2);
            updateGraph(zedGraphControl1, modAcc, 0);
        }

        // Metodo per il plotting del modulo della velocità angolare (giroscopio).
        private void ModGyrGraph()
        {
            float[] modGyr = Functions.Mod(sampwin, sensor_number - 1, 3, 5);
            updateGraph(zedGraphControl2, modGyr, 1);
        }

        // Operazione fondamentale: riconoscimento moto/stazionamento.
        private void Motion()
        {
            float[] accY = new float[sampwin.GetLength(1)];
            for (int i = 0; i < sampwin.GetLength(1); i++) accY[i] = sampwin[sensor_number - 1, i, 1];

            float[] smoothAccY = Functions.Smooth(accY, 10);

            for (int i = 0; i < smoothAccY.Length; i++)
            {
                if (smoothAccY[i] >= 4 && !events[0, 1].Equals("Fermo"))
                {
                    if (!events[0, 1].Equals("Ghepardo Treddoso")) Events_Help(0, i);
                    events[0, 1] = "Fermo";
                }
                else if (smoothAccY[i] < 3.9 && !events[0, 1].Equals("Non Fermo"))
                {
                    if (!events[0, 1].Equals("Ghepardo Treddoso")) Events_Help(0, i); ;
                    events[0, 1] = "Non Fermo";
                }
            }
            if (eof) Events_Help(0, smoothAccY.Length);
        }

        // Metodo ausiliario per scrittura su csv ed update struttura dati.
        private void Events_Help(int index, int shift)
        {
            DateTime tmp = date.AddSeconds(read_window / frequency + shift / frequency);
            string ev = events[index, 0] + " - " + tmp.ToString("hh:mm:ss") + " " + events[index, 1];
            Functions.EventsCsv(ev, path);
            events[index, 0] = tmp.ToString("hh:mm:ss");
        }

        // Operazione fondamentale: riconoscimento girata.
        private void Turn()
        {
            float[] angle = new float[sampwin.GetLength(1)];
            float[] yAngle = new float[sampwin.GetLength(1)];
            float[] zAngle = new float[sampwin.GetLength(1)];

            float[] modGyr = Functions.Mod(sampwin, sensor_number - 1, 3, 5);
            float[] smoothGyr = Functions.Smooth(modGyr, 10);

            // Estrazioni delle componenti assiali dal sampwin.
            for (int i = 0; i < sampwin.GetLength(1); i++)
            {
                yAngle[i] = sampwin[sensor_number - 1, i, 7];
                zAngle[i] = sampwin[sensor_number - 1, i, 8];
            }

            float[] smoothYAngle = Functions.Smooth(yAngle, 10);
            float[] smoothZAngle = Functions.Smooth(zAngle, 10);

            // Calcolo degli angoli per ogni istante i.
            for (int i = 0; i < sampwin.GetLength(1); i++) angle[i] = (float)Math.Atan2(smoothYAngle[i] , smoothZAngle[i]);

            float[] gAngle = new float[angle.Length];

            for (int i = 0; i < sampwin.GetLength(1); i++) gAngle[i] = (float)Math.Atan(smoothYAngle[i] / smoothZAngle[i]);

            // Plotting degli angoli.
            float[] graphAngle = Turn_Graph(gAngle);

            float[] smoothAngle = Functions.Smooth(angle, 10);


            for (int i = 0; i < angle.Length; i++)
            {
                if (smoothGyr[i] > 1.3)
                {
                    if (events[1, 1].Equals("Squalodonte Ricorsivo"))
                    {
                        events[1, 1] = "Girata";
                        angle_helper[0] = angle[i]; // Tengo memoria dell'angolo del campionamento in cui riscontro l'inizio della girata.
                        DateTime tmp = date.AddSeconds(read_window / frequency + i / frequency);
                        events[1, 0] = tmp.ToString("hh:mm:ss");
                    }
                }
                else if (smoothGyr[i] < 1.3 && events[1, 1].Equals("Girata"))
                {
                    angle_helper[1] = angle[i] - angle_helper[0]; // Calcolo approssimativamente di quanto mi sono girato.                   
                    if (Math.Abs(angle_helper[1]) >= Math.PI / 2)
                    { // Girate sotto i 30° non sono considerate.
                        if (angle_helper[1] < 0) Turn_Helper("Girata a Destra Di " + Math.Ceiling(Math.Abs(angle_helper[1] * 180 / Math.PI)) + " Gradi", i);
                        else Turn_Helper("Girata a Sinistra Di " + Math.Ceiling(Math.Abs(angle_helper[1] * 180 / Math.PI)) + " Gradi", i);
                   }
                    events[1, 1] = "Squalodonte Ricorsivo";
                }
            }

            if (eof && events[1, 1].Equals("Girata"))
            {
                angle_helper[1] = angle[angle.Length - 1] - angle_helper[0];
                if (angle_helper[1] < 0) Turn_Helper("Girata Incompleta a Destra Di " + Math.Ceiling(Math.Abs(angle_helper[1] * 180 / Math.PI)) + " Gradi", angle.Length);
                else Turn_Helper("Girata Incompleta a Sinistra Di " + Math.Ceiling(Math.Abs(angle_helper[1] * 180 / Math.PI)) + " Gradi", angle.Length);
                events[1, 1] = "Squalodonte Ricorsivo";
            }
            
        }
        
        // Metodo ausiliario solo per la girata.
        private void Turn_Helper(string builder, int shift)
        {
            DateTime tmp = date.AddSeconds(read_window / frequency + shift / frequency);
            string tmp1 = events[1, 0] + " - " + tmp.ToString("hh:mm:ss") + " " + builder;
            Functions.EventsCsv(tmp1, path);
            events[1, 0] = tmp.ToString("hh:mm:ss");
        }

        // Metodo per graficare gli angoli theta eliminando le discontinuità.
        private float[] Turn_Graph(float[] angle)
        {
            float[] graphAngle = new float[angle.Length]; // Nuovo array che contiene gli angoli opportunamente modificati.

            for (int i = 0; i < graphAngle.Length; i++)
            {
                if (i > 0)
                {
                    if ((angle[i] - angle[i - 1]) > 3)
                    { // Down -> up, caso scientificamente valido (salto di lunghezza ~PI).                 
                        graphAngle[i] = graphAngle[i - 1] + (angle[i] - angle[i - 1]) - (float)Math.PI;
                    }
                    else if ((angle[i] - angle[i - 1]) < -3)
                    { // Up -> down, caso scientificamente valido (salto di lunghezza ~PI).
                        graphAngle[i] = graphAngle[i - 1] + (angle[i] - angle[i - 1]) + (float)Math.PI;
                    }
                    else if ((angle[i] - angle[i - 1]) > 0.5)
                    { // Down -> up, eliminazione dei micro salti.
                        graphAngle[i] = graphAngle[i - 1];
                    }
                    else if ((angle[i] - angle[i - 1]) < -0.5)
                    { // Up -> down, eliminazione dei micro salti.
                        graphAngle[i] = graphAngle[i - 1];
                    }
                    else graphAngle[i] = graphAngle[i - 1] + (angle[i] - angle[i - 1]); // Caso senza salto.
                }
                else if (read_window > 200)
                { // Gestione discontinuità cross-windows.
                    if ((angle[i] - precAngle) > 3)
                    { // Down -> up, caso scientificamente valido (salto di lunghezza ~PI).                      
                        graphAngle[i] = precGraphAngle + (angle[i] - precAngle) - (float)Math.PI;
                    }
                    else if ((angle[i] - precAngle) < -3)
                    { // Up -> down, caso scientificamente valido (salto di lunghezza ~PI).                     
                        graphAngle[i] = precGraphAngle + (angle[i] - precAngle) + (float)Math.PI;
                    }
                    else if ((angle[i] - precAngle) > 0.5)
                    { // Down -> up, eliminazione dei micro salti.
                        graphAngle[i] = precGraphAngle;
                    }
                    else if ((angle[i] - precAngle) < -0.5)
                    { // Up -> down, eliminazione dei micro salti.
                        graphAngle[i] = precGraphAngle;
                    }
                    else graphAngle[i] = precGraphAngle + (angle[i] - precAngle); // Caso senza salto.
                }
                else
                { // Caso prima finestra ed i = 0.
                    graphAngle[0] = angle[0];
                }
            }
            precAngle = angle[angle.Length - 1];
            precGraphAngle = graphAngle[graphAngle.Length - 1];

            updateGraph(zedGraphControl3, graphAngle, 2);
            return graphAngle;

        }

        // Operazione fondamentale: riconoscimento Lay-Sit-Stand. 
        private void LaySit()
        {
            float[] accX = new float[sampwin.GetLength(1)];
            for (int i = 0; i < sampwin.GetLength(1); i++) accX[i] = sampwin[sensor_number - 1, i, 0];
            float[] smoothAccX = Functions.Smooth(accX, 10);
            float[] LaySitGraph = new float[smoothAccX.Length]; // Contiene i dati da plottare.

            // Per la lettura del codice seguente tenere in considerazione l'automa a stati finiti con relative funzioni di transizione.
            for (int i = 0; i < smoothAccX.Length; i++)
            {
                if (events[2, 1].Equals("Delfino Iterativo"))
                {
                    if (Math.Abs(smoothAccX[i]) <= 2.5) events[2, 1] = "Lay";
                    else if (Math.Abs(smoothAccX[i]) >= 2.9 && Math.Abs(smoothAccX[i]) <= 3.5) events[2, 1] = "LaySit";
                    else if (Math.Abs(smoothAccX[i]) >= 4 && Math.Abs(smoothAccX[i]) <= 7) events[2, 1] = "Sit";
                    else if (Math.Abs(smoothAccX[i]) >= 7.5) events[2, 1] = "Stand";
                }
                else if (Math.Abs(smoothAccX[i]) <= 2.5 && !events[2, 1].Equals("Lay"))
                {
                    if ((events[2, 1].Equals("LaySit") || events[2, 1].Equals("Delfino Iterativo")))
                    {
                        if (!events[2, 1].Equals("Delfino Iterativo")) Events_Help(2, i);
                        events[2, 1] = "Lay";
                    }
                }
                else if (Math.Abs(smoothAccX[i]) >= 2.9 && Math.Abs(smoothAccX[i]) <= 3.5 && !events[2, 1].Equals("LaySit"))
                {
                    if (events[2, 1].Equals("Lay") || events[2, 1].Equals("Sit") || events[2, 1].Equals("Delfino Iterativo"))
                    {
                        if (!events[2, 1].Equals("Delfino Iterativo")) Events_Help(2, i);
                        events[2, 1] = "LaySit";
                    }
                }
                else if (Math.Abs(smoothAccX[i]) >= 4 && Math.Abs(smoothAccX[i]) <= 7 && !events[2, 1].Equals("Sit"))
                {
                    if (events[2, 1].Equals("LaySit") || events[2, 1].Equals("Stand") || events[2, 1].Equals("Delfino Iterativo"))
                    {
                        if (!events[2, 1].Equals("Delfino Iterativo")) Events_Help(2, i);
                        events[2, 1] = "Sit";
                    }
                }
                else if (Math.Abs(smoothAccX[i]) >= 7.5 && !events[2, 1].Equals("Stand"))
                {
                    if (events[2, 1].Equals("Sit") || events[2, 1].Equals("Delfino Iterativo"))
                    {
                        if (!events[2, 1].Equals("Delfino Iterativo")) Events_Help(2, i);
                        events[2, 1] = "Stand";
                    }
                }

                // Riempimento LaySitGraph con i valori da plottare; il grafico è a scala. 
                if (events[2, 1].Equals("Lay")) LaySitGraph[i] = 0;
                else if (events[2, 1].Equals("LaySit")) LaySitGraph[i] = 0.5F;
                else if (events[2, 1].Equals("Sit")) LaySitGraph[i] = 1;
                else if (events[2, 1].Equals("Stand")) LaySitGraph[i] = 1.5F;
            }
            if (eof) Events_Help(2, smoothAccX.Length);

            updateGraph(zedGraphControl4, LaySitGraph, 3);
        }

        // Operazione fondamentale: Dead Reckoning, i.e stima della posizione della persona nello spazio bidimensionale.
        private void DeadReckoning()
        {
            float[] yaw = Functions.Yaw_Angles(sampwin, sensor_number - 1); // Calcolo dell'angolo di Eulero Yaw.
            float[] smoothYaw = Functions.Smooth(yaw, 10);

            float[] xPosition = new float[sampwin.GetLength(1)];
            float[] yPosition = new float[sampwin.GetLength(1)];

            float[] stride = Functions.Zupt_Algorithm(sampwin, sensor_number - 1, frequency, read_window); // Calcolo della lunghezza delle falcate/passi.

            // Calcolo delle coordinate nel piano.
            xPosition[0] = position_helper[0] + (float)(stride[0] * Math.Cos(smoothYaw[0]));
            yPosition[0] = position_helper[1] + (float)(stride[0] * Math.Sin(smoothYaw[0]));

            for (int i = 1; i < xPosition.Length; i++)
            {
                xPosition[i] = xPosition[i - 1] + (float)(stride[i] * Math.Cos(smoothYaw[i]));
                yPosition[i] = yPosition[i - 1] + (float)(stride[i] * Math.Sin(smoothYaw[i]));
            }

            position_helper[0] = xPosition[xPosition.Length - 1];
            position_helper[1] = yPosition[yPosition.Length - 1];

            updateGraph(zedGraphControl5, xPosition, yPosition, 4);
        }

        // Metodo che invoca la funzione che si occuperà di scrivere su csv i dati letti.
        void SaveData()
        {
            Functions.createCsv(sampwin, path);
        }

        // Metodo che si occupa di aggiungere i punti alla curva del grafico i-esimo.
        private void updateGraph(ZedGraphControl zedGraphControl1, float[] mod, int index)
        {
            for (int i = 0; i < mod.GetLength(0); i++, time[index]++) zedGraphControl1.GraphPane.CurveList[0].AddPoint(time[index], mod[i]);

            zedGraphControl1.GraphPane.XAxis.Scale.Min = time[index] - mod.GetLength(0);
            zedGraphControl1.GraphPane.XAxis.Scale.Max = time[index];
            zedGraphControl1.AxisChange();
        }

        // Metodo che si occupa di aggiungere i punti alla curva del grafico per il Dead Reckoning.
        private void updateGraph(ZedGraphControl zedGraphControl5, float[] xPoint, float[] yPoint, int index)
        {
            //Setto i pallini
            if (read_window <= 200) zedGraphControl5.GraphPane.CurveList[1].AddPoint(xPoint[0], yPoint[0]);
            else zedGraphControl5.GraphPane.CurveList[2].RemovePoint(0);

            for (int i = 0; i < xPoint.Length; i++, time[index]++) zedGraphControl5.GraphPane.CurveList[0].AddPoint(xPoint[i], yPoint[i]);

            zedGraphControl5.GraphPane.CurveList[2].AddPoint(xPoint[xPoint.Length - 1], yPoint[yPoint.Length -1]);
            zedGraphControl5.AxisChange();
        }

        // Metodo per il reset dei grafici prima della successiva lettura.
        private void startGraph(ZedGraphControl zedGraphControl, int index)
        {
            for (int i = time[index] - 1; i >= 0; i--) zedGraphControl.GraphPane.CurveList[0].RemovePoint(i);

            //Per togliere i segnalibri d'inizio e fine posizione per il dead reckoning
            if (zedGraphControl.GraphPane.Title.Text.Equals("Dead Reckoning"))
            {
                try
                {
                    zedGraphControl.GraphPane.CurveList[1].RemovePoint(0);
                    zedGraphControl.GraphPane.CurveList[2].RemovePoint(0);
                    zedGraphControl.GraphPane.CurveList[1].IsVisible = true;
                    zedGraphControl.GraphPane.CurveList[2].IsVisible = true;
                }
                catch (Exception e) { }
            }
            zedGraphControl.Refresh();
        }

        // Creazione dei grafici. L'array "info" detiene le informazioni di visualizzazione di ognuno.
        private void CreateGraph(ZedGraphControl zedGraphControl, string[] info, Color color)
        {
            // Riferimetno al GraphPane.
            GraphPane myPane = zedGraphControl.GraphPane;
            // Set dei titoli ed altri parametri.
            myPane.Title.Text = info[0];
            myPane.XAxis.Title.Text = info[1];
            myPane.YAxis.Title.Text = info[2];

            myPane.Title.FontSpec.FontColor = Color.Black;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.LightGray;
            myPane.YAxis.MajorGrid.Color = Color.LightGray;
            myPane.Legend.Position = ZedGraph.LegendPos.Bottom;

            PointPairList list = new PointPairList();
            list.Add(0, 0);
            LineItem myCurve;
            myCurve = myPane.AddCurve(info[3], list, color, SymbolType.None);
            myCurve.RemovePoint(0);

            //Inizializzazione segnalibro d'inizio e fine movimento nello spazio (per il dead reckoning)
            if (info[0].Equals("Dead Reckoning"))
            {
                PointPairList list_1 = new PointPairList();
                list_1.Add(0, 0);
                myPane.AddCurve("", list_1, Color.Red, SymbolType.Circle);
                LineItem point_1 = (LineItem)myPane.CurveList[1];           
                point_1.Line.IsVisible = false;
                point_1.IsVisible = false;
                point_1.Symbol.Fill = new Fill(Color.Red);
                point_1.Symbol.Size = 15.0F;

                PointPairList list_2 = new PointPairList();
                list_2.Add(0, 0);
                myPane.AddCurve("", list_2, Color.Red, SymbolType.Diamond);
                LineItem point_2 = (LineItem)myPane.CurveList[2];
                point_2.Line.IsVisible = false;
                point_2.IsVisible = false;
                point_2.Symbol.Fill = new Fill(Color.Red);
                point_2.Symbol.Size = 15.0F;
            }

            // Aggiunta di un gradiente sul colore.
            myPane.Chart.Fill = new Fill(Color.PaleTurquoise,
            Color.FromArgb(255, 255, 210), -45F);
        }

        // Metodo per la self-destruction causa eccezioni, crashes etc...
        private void Self_Destruction()
        {
            eof = true;
            reader.Close();
            stream.Close();
            listener.Stop();
            client.Close();
        }
    }
}
