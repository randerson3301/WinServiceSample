using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WinServiceSample
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Serviço iniciou em " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //milisegundos
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("O serviço parou em " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs args)
        {
            WriteToFile("O serviço fez recall em " + DateTime.Now);
        }

        //server core functionality
        public void WriteToFile(string Message)
        {
            //diretório responsável por armazenar arquivos de logs
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

            //se o path especificado n existir, ele será criado
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //nome do arquivo que armazenará os logs
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_"
                + DateTime.Now.Date.ToShortDateString().Replace('/','_') + ".txt";

            /*
             Se o arquivo não existir ele irá criar o arquivp e inserir a mensagem dentro,
             caso ele já exista, o serviço apenas adicionará uma nova linha dentro do arquivo 
             com o conteúdo de Message
             */
            if (!File.Exists(filePath))
            {
                using(StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(Message);
                }
            } else
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
