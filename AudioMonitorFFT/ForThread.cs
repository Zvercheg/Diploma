using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AudioMonitor
{
    class ForThread
    {
        
        static Tuner tuner = new Tuner();
        public static string retS()
        {
            
            tuner.StartDetect(0);

            
            return tuner.ReturnNote;
        }
        public static void stap()
        {
            tuner.waveIn.StopRecording();
        }
        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        public static async Task<string> DoIt()
        {
            // тут появляется магия вашего потока и генерируются names, 
            // а для примера:
                
                tuner.StartDetect(0);

                return tuner.ReturnNote;
            
            
            //await Task.Delay(5000);
              
        }
    }
}
