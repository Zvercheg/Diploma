using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioMonitor
{
    class Tuner
    {
        BufferedWaveProvider bufferedWaveProvider = null;
        public bool isSound;
        public WaveInEvent waveIn;
        Dictionary<string, float> noteBaseFreqs = new Dictionary<string, float>()
            {
                { "C", 16.35f },
                { "C#", 17.32f },
                { "D", 18.35f },
                { "Eb", 19.45f },
                { "E", 20.60f },
                { "F", 21.83f },
                { "F#", 23.12f },
                { "G", 24.50f },
                { "G#", 25.96f },
                { "A", 27.50f },
                { "Bb", 29.14f },
                { "B", 30.87f },
            };
        public string ReturnNote { get; private set; }
        IWaveProvider stream;
        public void StartDetect(int inputDevice)
        {
            waveIn = new WaveInEvent();

            waveIn.DeviceNumber = inputDevice;
            waveIn.WaveFormat = new WaveFormat(44100, 1);
            waveIn.DataAvailable += WaveIn_DataAvailable;

            bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);

            // begin record
            waveIn.StartRecording();

            

        }
        public string Nots()
        {
            return ReturnNote;
        }
        ~Tuner()
        {
            waveIn.Dispose();
          
        }
        void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (bufferedWaveProvider != null)
            {
                bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
                bufferedWaveProvider.DiscardOnBufferOverflow = true;

            }
            stream = new Wave16ToFloatProvider(bufferedWaveProvider);
            Pitch pitch = new Pitch(stream);

            byte[] buffer = new byte[8192];
            int bytesRead;
            bytesRead = stream.Read(buffer, 0, buffer.Length);

            float freq = pitch.Get(buffer);

            if (freq != 0)
            {
                ReturnNote = "Freq: " + freq + " Hz | Note: " + GetNote(freq);
                      
            }
        }

        public string GetNote(float freq)
        {
            float baseFreq;

            foreach (var note in noteBaseFreqs)
            {
                baseFreq = note.Value;

                for (int i = 0; i < 9; i++)
                {
                    if ((freq >= baseFreq - 2.1925) && (freq < baseFreq + 2.1925) || (freq == baseFreq))
                    {
                        return note.Key + i;
                    }

                    baseFreq *= 2;
                }
            }

            return null;
        }
    }
}

