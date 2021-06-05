using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AudioMonitor
{
    public partial class Form1 : Form
    {
        private SWHearFFT swhear;
        public bool autoScaleHappened = false;
        private Tuner tuner;
        string ReturnNote;
        Thread thread;
        public static Form1 EventSend;
        BackgroundWorker bw = new BackgroundWorker();
        public Form1(Thread thread)
        {
            InitializeComponent();
            this.thread = thread;
        }
      


        public Form1()
        {
            InitializeComponent();
         
           
        }
     


        private void Form1_Load(object sender, EventArgs e)
        {
            ScanForMicrophones();
           
        }

        private void ScanForMicrophones()
        {
            List<string> audioDeviceProductNames = new List<string>();

            for (int devIndex = 0; devIndex < NAudio.Wave.WaveIn.DeviceCount; devIndex++)
            {
                string devName = NAudio.Wave.WaveIn.GetCapabilities(devIndex).ProductName;
                audioDeviceProductNames.Add($"Device {devIndex}: {devName}");
            }
                

            comboMicrophone.Items.Clear();
            if (audioDeviceProductNames.Count > 0)
            {
                comboMicrophone.Items.AddRange(audioDeviceProductNames.ToArray());
                comboMicrophone.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("ERROR: no recording device is plugged in!");
            }
        }

        private void MicrophoneSelected()
        {
            btnStart_Click(null, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        // GUI BINDINGS

        private void btnMicScan_Click(object sender, EventArgs e)
        {
            ScanForMicrophones();
        }

        private void comboMicrophone_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MicrophoneSelected();
        }

        private  void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            

            swhear = new SWHearFFT(comboMicrophone.SelectedIndex);
            swhear.Open();
            tuner = new Tuner();

            double fftMaxFreq = swhear.SampleRate/2;
            double fftHzPerPoint = fftMaxFreq / swhear.fftData.Length;
            tuner.StartDetect(comboMicrophone.SelectedIndex);


            scottPlotUC1.plt.data.Clear();
            scottPlotUC1.plt.data.AddSignal(swhear.fftData, fftHzPerPoint);
            scottPlotUC1.plt.settings.figureBgColor = SystemColors.Control;
            scottPlotUC1.plt.settings.title = "Fft Display";
            scottPlotUC1.plt.settings.axisLabelY = "Power";
            timer1.Enabled = true;
            lblStatus.Text = $"Listening to device ID {swhear.DeviceIndex} ...";

            //очистка файлов

            string path2 = @"../../Вывод общего массива.txt";
            if (File.Exists(path2))
            {
                StreamWriter Writer = new StreamWriter(path2, false);
               
                Writer.Close();
            }

            string path = @"../../Проверка настройки гитары.txt";
            if (File.Exists(path))
            {
                StreamWriter Writer = new StreamWriter(path, false);
                
                Writer.Close();
            }
            string path3 = @"../../line.txt";
            if (File.Exists(path3))
            {
                StreamWriter Writer = new StreamWriter(path3, false);

                Writer.Close();
            }

            string path4 = @"../../line2.txt";
            if (File.Exists(path4))
            {
                StreamWriter Writer = new StreamWriter(path4, false);

                Writer.Close();
            }

            string path5 = @"../../dewfuze.txt";
            if (File.Exists(path5))
            {
                StreamWriter Writer = new StreamWriter(path5, false);

                Writer.Close();
            }

            string path6 = @"../../Отсортированный массив.txt";
            if (File.Exists(path6))
            {
                StreamWriter Writer = new StreamWriter(path6, false);

                Writer.Close();
            }

            string path8 = @"../../Вывод результата.txt";
            if (File.Exists(path8))
            {
                StreamWriter Writer = new StreamWriter(path8, false);

                Writer.Close();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;

            tuner.waveIn.StopRecording();
            //thread.Abort();
            swhear.Close();
            timer1.Enabled = false;



            string tuningresult = @"../../Проверка настройки гитары.txt";
            string tuning = @"../../Вывод общего массива.txt";
            string tuningplay = @"../../line.txt"; 
            string checkmass = @"../../dewfuze.txt";
            string endarray = @"../../Отсортированный массив.txt";
            string Base = @"../../База.txt";
            string rezult = @"../../Вывод результата.txt";
            // правильность настройки гитары

            using (StreamReader sr = new StreamReader(tuning, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string str1 = "E4";
                    string str2 = "B3";
                    string str3 = "G3";
                    string str4 = "D3";
                    string str5 = "A2";
                    string str6 = "E2";


                    if (line.Contains(str1))
                    {
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("1 струна настроена верно!");
                        }
                    }

                    else
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("1 струна настроена не верно или не была сыграна");
                        }
                    if (line.Contains(str2))
                    {
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("2 струна настроена верно!");
                        }
                    }

                    else
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("2 струна настроена не верно или не была сыграна");
                        }
                    if (line.Contains(str3))
                    {
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("3 струна настроена верно!");
                        }
                    }

                    else
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("3 струна настроена не верно или не была сыграна");
                        }
                    if (line.Contains(str4))
                    {
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("4 струна настроена верно!");
                        }
                    }

                    else
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("4 струна настроена не верно или не была сыграна");
                        }
                    if (line.Contains(str5))
                    {
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("5 струна настроена верно!");
                        }
                    }

                    else
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("5 струна настроена не верно или не была сыграна");
                        }

                    if (line.Contains(str6))
                    {
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("6 струна настроена верно!");
                        }
                    }

                    else
                        using (StreamWriter sw = new StreamWriter(tuningresult, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine("6 струна настроена не верно или не была сыграна");
                        }
                }

            }

            // удаление помех

         /*   using (StreamReader sr = new StreamReader(tuningplay, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string str1 = "A2";
                    string str2 = "C3";
                    string str3 = "D3";
                    string str4 = "Eb3";



                    if (line.Contains(str1) || line.Contains(str2) || line.Contains(str3) || line.Contains(str4))
                    {
                        using (StreamWriter sw = new StreamWriter(tuningplay2, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine(line);
                        }
                    }



                }
            }
*/
            //Сортировка массива

            using (StreamReader sr = new StreamReader(tuningplay, System.Text.Encoding.Default))
            {
                var list = new List<string>();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    list.Add(line);
                    Console.WriteLine(list[list.Count - 1]);
                }

                //Массив string[]              
                var arrTheoria = list.ToArray();

                for (int i = 0; i < arrTheoria.Length; i++)
                {
                    string x = arrTheoria[i];
                    char y = x[x.Length - 1];
                    char z = x[x.Length - 2];
                    char o = x[x.Length - 3];
                }

                for (int i = 0; i < arrTheoria.Length; i++)
                {
                    string x = arrTheoria[i];
                    char y = x[x.Length - 1];
                    char z = x[x.Length - 2];
                    char o = x[x.Length - 3];
                    string t = Convert.ToString(o) + Convert.ToString(z) + Convert.ToString(y) ;
                    if (i + 2 == arrTheoria.Length)
                    {
                        break;
                    }
                    if (arrTheoria[i] != arrTheoria[i + 1])
                    {
                        using (StreamWriter sw = new StreamWriter(checkmass, true, System.Text.Encoding.Default))
                        {
                          /*  if (t == "b3")
                            {
                                sw.WriteLine("E" + t);
                            }
                            else
                            {
                          
                         */
                         if (t=="e: ")
                                {
                                    continue;
                                }
                         else{
                                sw.WriteLine(t);
                           }
                        }
                    }
                }
                
                using (StreamReader sr2 = new StreamReader(checkmass, System.Text.Encoding.Default))
                {
                    var list2 = new List<string>();
                    while (!sr2.EndOfStream)
                    {
                        string line2 = sr2.ReadLine();
                        list2.Add(line2);

                    }

                    //Массив string[]              
                    var arrTheoria2 = list2.ToArray();
                    for (int i = 0; i < arrTheoria2.Length; i++)
                    {
                        if (i + 1 == arrTheoria2.Length)
                        {
                            using (StreamWriter sw = new StreamWriter(endarray, true, System.Text.Encoding.Default))
                            {
                                sw.WriteLine(arrTheoria2[i]);
                            }

                            break;
                        }
                        else if (arrTheoria2[i] != arrTheoria2[i + 1])
                        {
                            using (StreamWriter sw = new StreamWriter(endarray, true, System.Text.Encoding.Default))
                            {
                                sw.WriteLine(arrTheoria2[i]);
                            }
                        }
                    }
                }
            }

            //проверка и вывод результата
            using (StreamReader sr = new StreamReader(Base, System.Text.Encoding.Default))
            {
                var list = new List<string>();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    list.Add(line);

                }

                //Массив string[]              
                var arrTheoria = list.ToArray();

                using (StreamReader sr2 = new StreamReader(endarray, System.Text.Encoding.Default))
                {
                    var list2 = new List<string>();
                    while (!sr2.EndOfStream)
                    {
                        string line2 = sr2.ReadLine();
                        list2.Add(line2);
                      
                    }

                    //Массив string[]              
                    var arrTheoria2 = list2.ToArray();
                    int xa = 12 - arrTheoria2.Length;
                    if (xa > 0)
                    {
                        for(int i = 0; i < 12; i++)
                        {
                            list2.Add("Null");
                        }
                    }


                    int x = 0;
                    for (int i = 0; i < arrTheoria.Length; i++) {
                        

                        if (list2[i] == list[i])
                        {
                            x++;
                        }

                    }

                   /* int z = (x * 100) / arrTheoria.Length;
                    string f;
                    if (z < 25)
                    {
                        f = "нужно больше практики";
                    }
                    else if (z < 100)
                    {
                        f = "неплохо, но можно и лучше";
                    }
                    else if (z == 100)
                    {
                        f = "Умница, все верно";
                    }
                    else
                    {
                        f = "Ты вообще эту песню играешь?";
                    }
                    */
                    using (StreamWriter sw = new StreamWriter(rezult, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine("Результаты игры:");
                        sw.WriteLine();                    
                        sw.WriteLine("Было сыграно правильно - " + x +" нот из " + arrTheoria.Length);
                        sw.WriteLine();

                        sw.WriteLine("Сыгранное вами произведение" +"---"+"Произведение из базы");   
                        sw.WriteLine();
                        for(int i = 0; i < arrTheoria.Length; i++)
                        {
                            sw.WriteLine("           " + list2[i] + "----------------------------" + list[i] );
                        }

                       
                    }
                }
               
            }

        }
        int lastProcessedBuffer = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (swhear.BuffersRead == lastProcessedBuffer)
                return;
            scottPlotUC1.plt.settings.axisLabelX = tuner.Nots();
           string text = tuner.Nots();
            // set level meter
            lastProcessedBuffer = swhear.BuffersRead;
            double maxAmplitude = Math.Pow(2, 16);
            double lastBufferPeakFrac = swhear.lastBufferAmplitude / maxAmplitude;
            pbLevelMask.Height = pnlLevel.Height - (int)(lastBufferPeakFrac * pnlLevel.Height);
            
            // force a ScottPlot render
            scottPlotUC1.plt.data.ClearAxisLines();
            if (scottPlotUC1.plt.settings.axisLabelX != null)
            {
                File.AppendAllText(@"../../line.txt", scottPlotUC1.plt.settings.axisLabelX + Environment.NewLine);
            }
            File.AppendAllText(@"../../Вывод общего массива.txt", scottPlotUC1.plt.settings.axisLabelX ); //запись в файл общего массива нот.
            // auto-scale after a few recordings 
            if (swhear.BuffersRead > 10 && autoScaleHappened == false)
            {
                scottPlotUC1.plt.settings.AxisFit(0, .5);
                autoScaleHappened = true;
            }

            // render the image


            //

            
            //
            scottPlotUC1.Render();
            //Thread.Sleep(1000);
            
        }
       
        private async void scottPlotUC1_Load(object sender, EventArgs e)
        {
            
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

       

        private void timer2_Tick(object sender, EventArgs e)
        {
           // textBox1.Text = swhear.Nota;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process txt = new System.Diagnostics.Process();
            txt.StartInfo.FileName = "notepad.exe";
            txt.StartInfo.Arguments = @"../../Вывод результата.txt";
            txt.Start();
        }
    }
}
