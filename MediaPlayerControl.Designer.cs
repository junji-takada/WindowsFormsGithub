//https://www.chuken-engineer.com/entry/2019/06/24/092055
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace WindowsFormsGithub
{
    partial class MediaPlayerControl : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private string[] playList;
        private int LIST_NUM;
        private int nowPlayNum = 0;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        public MediaPlayerControl()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 自身のサイズを変更
            this.Bounds = new Rectangle(0, 0, 1920, 1080);

            // フォルダ内の再生可能な動画ファイル(mp4)の一覧を取得する。
            playList = System.IO.Directory.GetFiles(@"C:\work\sampleMedia", "*.mp4", System.IO.SearchOption.AllDirectories); LIST_NUM = playList.Length;

            // 動画プレイヤーの設定
            mediaPlayer1 = axWindowsMediaPlayer1;
            mediaPlayer1.stretchToFit = true;			// 表示領域にフィットするようにサイズを変更する
            mediaPlayer1.uiMode = "none";               // UIを消す
            mediaPlayer1.Location = new Point(0, 0);
            mediaPlayer1.Dock = DockStyle.Fill;
            mediaPlayer1.settings.autoStart = false;	// 自動再生無効
            mediaPlayer1.Ctlenabled = false;            // ダブルクリックによるフルスクリーン出力を無効化
            mediaPlayer1.enableContextMenu = false;     // 右クリックによるコンテキストメニューの出力を無効化

            // 再生開始
            mediaPlayer1.URL = playList[nowPlayNum];
            playNumIncrement();
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            switch (e.newState)
            {
                case (int)WMPLib.WMPPlayState.wmppsStopped:
                    //停止時
                    break;

                case (int)WMPLib.WMPPlayState.wmppsPlaying:
                    //再生時

                    break;

                case (int)WMPLib.WMPPlayState.wmppsMediaEnded:
                    //再生終了時
                    mediaPlayer1.URL = playList[nowPlayNum];
                    playNumIncrement();
                    break;

                case (int)WMPLib.WMPPlayState.wmppsTransitioning:
                    //再生準備中()

                    break;

                case (int)WMPLib.WMPPlayState.wmppsReady:
                    //再生準備完了
                    timer1.Start();
                    break;

                default:
                    break;
            }
        }

        private void playNumIncrement()
        {
            nowPlayNum++;
            if (nowPlayNum >= LIST_NUM)
            {
                nowPlayNum = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            mediaPlayer1.Ctlcontrols.play();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "MediaPlayerControl";
        }

        #endregion
    }
}