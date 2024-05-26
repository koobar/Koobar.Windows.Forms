using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Koobar.Windows.Forms
{
    /// <summary>
    /// 波形を表示するコントロール
    /// </summary>
    public class WaveformViewer : UserControl
    {
        // 非公開フィールド
        private readonly Timer updateTimer;
        private readonly List<float> samples;
        private int maxSamples;
        private bool realtimeMode;
        private float xInterval;
        private float centerY;
        private Color waveformColor;
        private Color centerLineColor;
        private bool useAntiAlias;
        private float waveformLineWidth;

        // コンストラクタ
        public WaveformViewer()
        {
            this.updateTimer = new Timer();
            this.updateTimer.Interval = 200;
            this.updateTimer.Tick += OnUpdateTimerTick;
            
            this.samples = new List<float>();
            this.maxSamples = 100;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.RealtimeMode = false;
            this.WaveformColor = Color.Green;
            this.CenterLineColor = Color.White;
            this.DrawCenterLine = true;
            this.UseAntiAlias = true;
            this.waveformLineWidth = 1.0f;
        }

        #region プロパティ

        /// <summary>
        /// タイマー駆動時の描画の更新間隔（ミリ秒）
        /// </summary>
        public int UpdateInterval
        {
            set
            {
                this.updateTimer.Interval = value;
            }
            get
            {
                return this.updateTimer.Interval;
            }
        }

        /// <summary>
        /// 表示する最大サンプル数
        /// </summary>
        public int MaxSamples
        {
            set
            {
                this.maxSamples = value;

                Invalidate();
            }
            get
            {
                return this.maxSamples;
            }
        }

        /// <summary>
        /// リアルタイムに描画するかどうか
        /// </summary>
        public bool RealtimeMode
        {
            set
            {
                if (value)
                {
                    this.updateTimer.Stop();
                }

                this.realtimeMode = value;
            }
            get
            {
                return this.realtimeMode;
            }
        }

        /// <summary>
        /// 中央線を描画するかどうか
        /// </summary>
        public bool DrawCenterLine { set; get; }

        /// <summary>
        /// 波形の色
        /// </summary>
        public Color WaveformColor
        {
            set
            {
                this.waveformColor = value;
                Invalidate();
            }
            get
            {
                return this.waveformColor;
            }
        }

        /// <summary>
        /// 中央線の色
        /// </summary>
        public Color CenterLineColor
        {
            set
            {
                this.centerLineColor = value;
                Invalidate();
            }
            get
            {
                return this.centerLineColor;
            }
        }

        /// <summary>
        /// アンチエイリアスを行うかどうか
        /// </summary>
        public bool UseAntiAlias
        {
            set
            {
                this.useAntiAlias = value;
                Invalidate();
            }
            get
            {
                return this.useAntiAlias;
            }
        }

        /// <summary>
        /// 波形の線の幅
        /// </summary>
        public float WaveformLineWidth
        {
            set
            {
                this.waveformLineWidth = value;
                Invalidate();
            }
            get
            {
                return this.waveformLineWidth;
            }
        }

        #endregion
        
        /// <summary>
        /// タイマー駆動時の駆動を開始する。
        /// </summary>
        public void Start()
        {
            if (!this.RealtimeMode)
            {
                this.updateTimer.Start();
            }
        }

        /// <summary>
        /// タイマー駆動時の駆動を停止する。
        /// </summary>
        public void Stop()
        {
            this.updateTimer.Stop();
        }

        /// <summary>
        /// 波形をクリアする。
        /// </summary>
        public void Clear()
        {
            this.samples.Clear();
            Invalidate();
        }

        /// <summary>
        /// サンプルを追加する。
        /// </summary>
        /// <param name="sample"></param>
        public void AddSample(float sample)
        {
            if (sample < -1)
            {
                sample = -1;
            }
            else if (sample > 1)
            {
                sample = 1;
            }

            if (this.samples.Count >= this.maxSamples)
            {
                this.samples.RemoveAt(0);
            }

            this.samples.Add(sample);

            if (this.RealtimeMode)
            {
                Invalidate();
            }
        }

        /// <summary>
        /// タイマー駆動時における描画の更新が発生する場合の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnUpdateTimerTick(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// コントロールのサイズが変更された場合の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            this.xInterval = this.ClientRectangle.Width / (float)this.maxSamples;
            this.centerY = this.ClientRectangle.Height * 0.5f;

            base.OnSizeChanged(e);
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.useAntiAlias)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
            else
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            }

            if (this.DrawCenterLine)
            {
                var centerLinePen = new Pen(this.CenterLineColor, this.waveformLineWidth);

                // 中央線を描画
                e.Graphics.DrawLine(centerLinePen, 0, this.centerY, e.ClipRectangle.Right, this.centerY);

                // 後始末
                centerLinePen.Dispose();
            }

            if (this.samples.Count <= 0)
            {
                return;
            }

            // 波形の描画
            float px = -this.xInterval * e.ClipRectangle.Height;
            float py = 0;
            float x = 0;
            for (int i = 0; i < Math.Min(this.maxSamples, this.samples.Count); ++i)
            {
                float y = this.centerY - (this.samples[i] * (this.centerY - 5));

                // 波形を描画
                var waveformPen = new Pen(this.WaveformColor, this.waveformLineWidth);
                e.Graphics.DrawLine(waveformPen, px, py, x, y);

                // 座標の更新
                px = x;
                py = y;
                x += this.xInterval;

                // リソースを解放
                waveformPen.Dispose();
            }
        }
    }
}
