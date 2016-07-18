using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Peasant
{

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartingForm());
        }
    }

    /** 
    Classe per le funzioni ausiliarie di tipo matematico/statistico vitali per la riuscita delle operazioni fondamentali.
    Contiene anche le funzioni di scrittura su csv.        
    */
    static class Functions
    {

        static bool critic_region = false; // Booleano per gestire accesso a csv da parte dei threads.

        // Metodo per la scrittura e creazione del .csv che contiene i dati.
        public static void createCsv(float[,,] sampwin, string path)
        {
            StreamWriter file = new StreamWriter(@path, true);
            string stream = "";

            for (int s = 0; s < sampwin.GetLength(0); s++)
            {
                stream = stream + "SENSORE " + (s + 1) + ":" + "\n" + "\n";
                for (int i = 0; i < sampwin.GetLength(1); i++)
                {
                    for (int j = 0; j < sampwin.GetLength(2); j++)
                    {
                        stream = stream + sampwin[s, i, j].ToString() + ";";
                    }
                    stream = stream + "\n";
                }
                stream = stream + "\n";

                try
                {
                    file.Write(stream);
                    stream = "";
                }
                catch (Exception e)
                {
                    file.Close(); stream = "";
                }
            }

            file.Close();
        }

        // Metodo per la scrittura e creazione del .csv per gli eventi.
        public static void EventsCsv(string events, string path)
        {
            while (critic_region)
            {
                Random r = new Random(10);
                Thread.Sleep(r.Next(1, 10));
            }

            critic_region = true;

            path = path.Insert(path.Length - 4, "_Events");
            StreamWriter file = new StreamWriter(@path, true);

            events = events + ";\n";

            try
            {
                file.Write(events);
            }
            catch (Exception e)
            {
                file.Close();
            }

            file.Close();
            critic_region = false;
        }

        [Obsolete("Test Case Method -- Only For Debugging")]
        public static void ModAccCsv(float[] modAcc, string path)
        {
            while (critic_region)
            {
                Random r = new Random(10);
                Thread.Sleep(r.Next(1, 10));
            }

            critic_region = true;

            path = path.Insert(path.Length - 4, "_ModAccSD");
            StreamWriter file = new StreamWriter(@path, true);

            string stream = "";

            for (int i = 0; i < modAcc.Length; i++) stream = stream + modAcc[i] + "; \n";

            try
            {
                file.Write(stream); stream = "";
            }
            catch (Exception e)
            {
                file.Close();
            }

            file.Close();
            critic_region = false;
        }

        /**
        Implementazione della prima operazione base: calcolo del valore assoluto. 
        Funzione che calcola il modulo di una tripla di floating point(x,y,z).
        
        start_index indice d'inizio per calcolare il modulo.
        end_index indice di fine per calcolare il modulo.
        end_index-start_index+1 -> elementi su cui calcolo il modulo.
        */
        public static float[] Mod(float[,,] sampwin, int sensor_index, int start_index, int end_index)
        {
            float[] mod = new float[sampwin.GetLength(1)];

            for (int i = 0; i < sampwin.GetLength(1); i++)
            {
                float sum = 0;
                for (int j = start_index; j <= end_index; j++) sum = sum + (float)Math.Pow(sampwin[sensor_index, i, j], 2);
                mod[i] = (float)Math.Sqrt(sum);
            }
            return mod;
        }

        /**
        Implementazione della seconda operazione di base: smoothing.
        */
        public static float[] Smooth(float[] sequence, int window)
        {
            float[] smooth = new float[sequence.Length];

            for (int i = 0; i < sequence.Length; i++)
            {
                if (i - window >= 0 && i + window < sequence.Length) smooth[i] = Mean(sequence, i - window, i + window);
                if (i - window >= 0 && i + window >= sequence.Length) smooth[i] = Mean(sequence, i - window, sequence.Length - 1);
                if (i - window < 0 && i + window < sequence.Length) smooth[i] = Mean(sequence, 0, i + window);
                if (i - window < 0 && i + window >= sequence.Length) smooth[i] = Mean(sequence, 0, sequence.Length - 1);
            }
            return smooth;
        }

        /**
        Funzione che calcola la media di n valori.
        Utile per la funzione di smoothing e per la quarta operazione: deviazione standard.
        */
        public static float Mean(float[] sequence, int start_index, int end_index)
        {
            float sum = 0;
            for (int j = start_index; j <= end_index; j++) sum = sum + sequence[j];
            return sum / ((end_index - start_index) + 1);
        }

        /**
        Implementazione della terza operazione base: calcolo della derivata tramite rapporti incrementali.
        Definizione di Rapporto incrementale: (f(x)-f(x0))/(x-x0) dove x >= x0.
        I valori di sequence[] sono f(x), la differenza x-x0 è 1/frequenza, in quanto tra un dato e l'altro mi sposto in "avanti" di un'unità.

        RI[*sequence*] = 0 perchè non avviene alcun incremento.
        */
        public static float[] Incremental_Ratio(float[] sequence)
        {
            float[] RI = new float[sequence.Length];
            for (int i = 0; i < sequence.Length - 1; i++) RI[i] = (sequence[i + 1] - sequence[i]);
            RI[sequence.Length - 1] = 0;
            return RI;
        }

        [Obsolete("Test Case Method -- Only For Debugging")]
        public static float[] Single_Smooth(float[,,] sampwin, int sensor_index, int column_index, int window)
        {
            int length = sampwin.GetLength(1);
            float[] smooth = new float[length];

            for (int i = 0; i < sampwin.GetLength(1); i++)
            {
                if (i - window >= 0 && i + window < length) { smooth[i] = Single_Mean(sampwin, sensor_index, column_index, i - window, i + window); }
                if (i - window >= 0 && i + window >= length) { smooth[i] = Single_Mean(sampwin, sensor_index, column_index, i - window, length - 1); }
                if (i - window < 0 && i + window < length) { smooth[i] = Single_Mean(sampwin, sensor_index, column_index, 0, i + window); }
                if (i - window < 0 && i + window >= length) { smooth[i] = Single_Mean(sampwin, sensor_index, column_index, 0, length - 1); }
            }
            return smooth;
        }

        [Obsolete("Test Case Method -- Only For Debugging")]
        public static float Single_Mean(float[,,] sampwin, int sensor_index, int column_index, int start_index, int end_index)
        {
            float sum = 0;
            for (int j = start_index; j <= end_index; j++) sum = sum + sampwin[sensor_index, j, column_index];
            return sum / ((end_index - start_index) + 1);
        }

        /**
        Implementazione della quarta operazione base: deviazione standard.
        */
        public static float[] Standard_Deviation(float[] sequence, int window)
        {
            float[] smooth = Smooth(sequence, window);
            float[] SD = new float[sequence.Length];

            float variance;

            for (int j = 0; j < sequence.Length; j++)
            {
                variance = (float)Math.Pow(sequence[j], 2);
                int elements = 1;
                for (int i = 1; i <= window; i++)
                {
                    if (j - i >= 0 && j + i < sequence.Length)
                    {
                        variance = variance + (float)Math.Pow(sequence[j - i], 2) + (float)Math.Pow(sequence[j + 1], 2);
                        elements = elements + 2;
                    }
                    if (j - i >= 0 && j + i >= sequence.Length)
                    {
                        variance = variance + (float)Math.Pow(sequence[j - i], 2);
                        elements = elements + 1;
                    }
                    if (j - i < 0 && j + i < sequence.Length)
                    {
                        variance = variance + (float)Math.Pow(sequence[j + 1], 2);
                        elements = elements + 1;
                    }
                    if (j - i < 0 && j + i >= sequence.Length)
                    {
                        break; // 'Tis barbaric!
                    }
                }
                variance = variance / elements - (float)Math.Pow(smooth[j], 2);
                float Standard_Deviation = (float)Math.Sqrt(variance);

                if (float.IsNaN(Standard_Deviation)) { SD[j] = 0; }
                else { SD[j] = Standard_Deviation; }
            }
            return SD;
        }

        [Obsolete("Summation is crying in a corner -- Do Not Disturb")]
        public static float Sum(float[] sequence, int start_index, int end_index)
        {
            float sum = 0;
            for (int j = start_index; j <= end_index; j++) { sum = sum + sequence[j]; }
            return sum;
        }

        /**
        Implementazione della quinta operazione fondamentale: angoli di Eulero.
        */
        public static float[,] Eulero_Angles(float[,,] sampwin, int sensor_index)
        {
            float[,] angles = new float[sampwin.GetLength(1), 3];

            for (int i = 0; i < sampwin.Length; i++)
            {
                angles[i, 0] = (float)Math.Atan(((2 * sampwin[sensor_index, i, 11] * sampwin[sensor_index, i, 12]) + (2 * sampwin[sensor_index, i, 9] * sampwin[sensor_index, i, 10])) / ((2 * (float)Math.Pow(sampwin[sensor_index, i, 9], 2)) + (2 * (float)Math.Pow(sampwin[sensor_index, i, 12], 2)) - 1));
                angles[i, 1] = - (float)Math.Asin((2 * sampwin[sensor_index, i, 10] * sampwin[sensor_index, i, 12]) - (2 * sampwin[sensor_index, i, 9] * sampwin[sensor_index, i, 11]));
                angles[i, 2] = (float)Math.Atan(((2 * sampwin[sensor_index, i, 10] * sampwin[sensor_index, i, 11]) + (2 * sampwin[sensor_index, i, 9] * sampwin[sensor_index, i, 12])) / ((2 * (float)Math.Pow(sampwin[sensor_index, i, 9], 2)) + (2 * (float)Math.Pow(sampwin[sensor_index, i, 10], 2)) - 1));
            }
            return angles;
        }

        /**
        Funzione che calcolo l'angolo di Eulero Yaw.
        */
        public static float[] Yaw_Angles(float[,,] sampwin, int sensor_index)
        {
            float[] angles = new float[sampwin.GetLength(1)];

            for (int i = 0; i < sampwin.GetLength(1); i++) angles[i] = (float)Math.Atan2(((2 * sampwin[sensor_index, i, 10] * sampwin[sensor_index, i, 11]) + (2 * sampwin[sensor_index, i, 9] * sampwin[sensor_index, i, 12])), ((2 * (float)Math.Pow(sampwin[sensor_index, i, 9], 2)) + (2 * (float)Math.Pow(sampwin[sensor_index, i, 10], 2)) - 1));
            return angles;
        }

        /**
        Funzione che calcolo l'angolo di Eulero Roll.
        */
        public static float[] Roll_Angles(float[,,] sampwin, int sensor_index)
        {
            float[] angles = new float[sampwin.GetLength(1)];

            for (int i = 0; i < sampwin.GetLength(1); i++) angles[i] =(float)Math.Atan2(((2 * sampwin[sensor_index, i, 11] * sampwin[sensor_index, i, 12]) + (2 * sampwin[sensor_index, i, 9] * sampwin[sensor_index, i, 10])) , ((2 * (float)Math.Pow(sampwin[sensor_index, i, 9], 2)) + (2 * (float)Math.Pow(sampwin[sensor_index, i, 12], 2)) - 1));
            return angles;
        }

        /**
        Funzione che calcolo l'angolo di Eulero Pitch.
        */
        public static float[] Pitch_Angles(float[,,] sampwin, int sensor_index)
        {
            float[] angles = new float[sampwin.GetLength(1)];

            for (int i = 0; i < sampwin.GetLength(1); i++) angles[i] = - (float)Math.Asin((2 * sampwin[sensor_index, i, 10] * sampwin[sensor_index, i, 12]) - (2 * sampwin[sensor_index, i, 9] * sampwin[sensor_index, i, 11]));
            return angles;
        }

        static float vel_Prec = 0; // Contiene il valore della velocità all'ultimo istante della finestra precedente.

        // Metodo che calcola la velocità come integrale dell'accelerazione.
        public static float[] Integral_Velocity(float[] values, double frequency, double read_window)
        {
            float[] integral = new float[values.Length];
            if (read_window <= 200) vel_Prec = 0;
            integral[0] = vel_Prec + (float)(values[0] / frequency);

            for (int i = 1; i < values.Length; i++) integral[i] = integral[i - 1] + (float)(values[i] / frequency);

            vel_Prec = values[values.Length - 1];

            float[] smoothVelocity = Smooth(integral, 10);
            return smoothVelocity;
        }

        // Metodo che calcola la variazione dello spazio per ogni coppia (i, i+1) come integrale della velocità.
        public static float[] Integral_Space(float[] velocity, double frequency, double read_window)
        {
            float[] integral = new float[velocity.Length];

            for (int i = 0; i < velocity.Length; i++) integral[i] = (float)(velocity[i] / frequency);
            return integral;
        }

        // Algoritmo fondamentale per il Dead Reckoning: calcola la lunghezza delle falcate e le ritorna al chiamante.
        public static float[] Zupt_Algorithm(float[,,] sampwin, int sensor_index, double frequency, double read_window)
        {

            float[] yAcc = new float[sampwin.GetLength(1)];
            float[] zAcc = new float[sampwin.GetLength(1)];

            for (int i = 0; i < sampwin.GetLength(1); i++)
            {
                yAcc[i] = sampwin[sensor_index, i, 1];
                zAcc[i] = sampwin[sensor_index, i, 2];
            }

            float[] smoothYAcc = Smooth(yAcc, 10);
            float[] smoothZAcc = Smooth(zAcc, 10);

            float[] yVelocity = Integral_Velocity(smoothYAcc, frequency, read_window);
            float[] zVelocity = Integral_Velocity(smoothZAcc, frequency, read_window);

            float[] ySpace = Integral_Space(yVelocity, frequency, read_window);
            float[] zSpace = Integral_Space(zVelocity, frequency, read_window);

            float[] stride = new float[ySpace.Length];
            for (int i = 0; i < ySpace.Length; i++) stride[i] = (float)Math.Sqrt(Math.Pow(ySpace[i], 2) + Math.Pow(zSpace[i], 2));

            return stride;
        }

        [Obsolete("The princess is in another castle")]
        public static List<int> Step_Detection(float[,,] sampwin, int sensor_index)
        {
            float[] modAcc = Mod(sampwin, sensor_index, 0, 2);
            float[] smoothAcc = Smooth(modAcc, 10);
            float[] sdAcc = Standard_Deviation(smoothAcc, 10);

            float[] swing_start = new float[sdAcc.Length];
            float[] stance_start = new float[sdAcc.Length];

            for (int i = 0; i < sdAcc.Length; i++)
            {
                if (sdAcc[i] >= 0.8)
                {
                    swing_start[i] = (float)0.8;
                    stance_start[i] = 0;
                }
                else if (sdAcc[i] <= 0.3)
                {
                    swing_start[i] = 0;
                    stance_start[i] = (float)0.3;
                }
                else
                {
                    swing_start[i] = 0;
                    stance_start[i] = 0;
                }
            }

            List<int> step_detection = new List<int>();

            for (int i = 1; i < swing_start.Length; i++)
            {
                if (swing_start[i - 1] == 0 && swing_start[i] == (float)0.8)
                {
                    int sup;
                    if (i + 10 >= swing_start.Length) sup = swing_start.Length - 1;
                    else sup = i + 10;
                    for (int j = i; j < sup; j++)
                    {
                      if (stance_start[j] == (float)0.3)
                      {
                         step_detection.Add(i);
                         break;
                      }
                   }
                }
            }
            return step_detection;
        }

    }
}
