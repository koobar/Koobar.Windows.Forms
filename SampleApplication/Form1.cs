using Koobar.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SampleApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        /// <summary>
        /// デモ表示用のWaveformViewerを作成する。
        /// </summary>
        /// <returns></returns>
        private WaveformViewer CreateDemonstrationWaveformViewer()
        {
            float phase = 0;            // 正弦波の位相
            float freq = 440;           // 正弦波の周波数
            float sampleRate = 44100;   // 正弦波のサンプリング周波数

            var viewer = new WaveformViewer();
            viewer.BackColor = Color.Black;
            viewer.WaveformColor = Color.Green;
            viewer.CenterLineColor = Color.White;
            viewer.DrawCenterLine = true;
            viewer.RealtimeMode = false;
            viewer.UpdateInterval = 20;
            viewer.MaxSamples = 100;
            viewer.UseAntiAlias = true;
            viewer.WaveformLineWidth = 2;

            // 一定間隔でWaveformViewerに正弦波を与えるためのタイマー
            var timer = new Timer();
            timer.Interval = 15;
            timer.Tick += delegate
            {
                phase += freq / sampleRate;
                phase -= (float)Math.Floor(phase);
                viewer.AddSample((float)Math.Sin(2 * Math.PI * phase));
            };

            viewer.Start();
            timer.Start();

            return viewer;
        }

        private Koobar.Windows.Forms.TabPage CreateWaveformViewerTabPage()
        {
            var page = new Koobar.Windows.Forms.TabPage("WaveformViewer");
            page.Control = CreateDemonstrationWaveformViewer();

            return page;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.closableTabControl1.AddTabPage(CreateWaveformViewerTabPage());
        }
    }
}
