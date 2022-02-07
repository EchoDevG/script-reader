using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Speech.Synthesis;
using Microsoft.CSharp;


namespace script_reader
{
    public partial class Form1 : Form
    {

        public bool play = false;
        public bool hasPlayed = false;
        public SpeechSynthesizer voice = new SpeechSynthesizer();
        public string textString;


        public Form1()
        {
            InitializeComponent();

            foreach (InstalledVoice iVoice in voice.GetInstalledVoices())
            {
                VoiceInfo info = iVoice.VoiceInfo;
                comboBox1.Items.Add(info.Name);
            }

            voice.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(voice_SpeakCompleted);

            voice.Volume = trackBar1.Value;

            //voice.Rate = 1;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = "";

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\Documents\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
            }

            try
            {
                textString = File.ReadAllText(filePath);
            }
            catch{}


            textBox1.Text = textString;

            this.ActiveControl = null;

        }



        private void playpause_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            playpauseFunc();
            
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    { playpauseFunc();}
                    break;
                default: { break; }
            }
        }


        
        public void playpauseFunc()
        {
            
            switch (play) { 
                case true:
                    {
                        voice.Pause();
                        play = false;
                        break;
                    }
                case false:
                    {
                        if (hasPlayed == false)
                        {
                            voice.SpeakAsync(textString);
                            hasPlayed = true;
                            comboBox1.Enabled = false;
                            return;
                        }
                        voice.Resume();
                        play = true;

                        break;
                    }
                default: { break; }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            voice.Volume = trackBar1.Value;
            this.ActiveControl = null;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            voice.SelectVoice((string)comboBox1.SelectedItem);
            this.ActiveControl = null;
        }

        public void voice_SpeakCompleted(object sender, EventArgs e)
        {
            hasPlayed = false;
            comboBox1.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            hasPlayed = false;
            play = false;
            comboBox1.Enabled = true;
            voice.SpeakAsyncCancelAll();
        }
    }
}
