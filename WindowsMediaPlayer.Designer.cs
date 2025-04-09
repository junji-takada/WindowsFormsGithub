//https://effect.hatenablog.com/entry/2020/03/29/024702
//
using System;
using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace WindowsFormsGithub
{
    public partial class WindowsMediaPlayer : Form, IMessageFilter
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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
            this.Text = "WindowsMediaPlayer";
        }

		#endregion
		string videoPath, videoTitle;
		WMPPlayState videoState;
		int srcWidth = 0;
		int srcHeight = 0;
		double loopSta = 0.0;
		double loopEnd = 0.0;
		bool ready = false;

		public WindowsMediaPlayer()
		{
			InitializeComponent();
			//wmpVideo.uiMode = "none";

			this.KeyPreview = true;
			wmpVideo.AllowDrop = true;
			wmpVideo.enableContextMenu = false;
		}

		// ロード時
		private void WindowsMediaPlayer_Load(object sender, EventArgs e)
		{
			// アプリケーションの設定を読み込む
			Properties.Settings.Default.Reload();

			try
			{
				wmpVideo.settings.mute = Properties.Settings.Default.isMute;
				wmpVideo.settings.volume = Properties.Settings.Default.volume;
			}
			catch (Exception)
			{

			}
		}

		// 終了時
		private void WindowsMediaPlayer_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				Properties.Settings.Default.isMute = wmpVideo.settings.mute;
				Properties.Settings.Default.volume = wmpVideo.settings.volume;
			}
			catch (Exception)
			{

			}

			// アプリケーションの設定を保存する
			Properties.Settings.Default.Save();
		}

		// フルスクリーンモード時にキーを受け付けるためのもの
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Application.AddMessageFilter(this);
		}
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			Application.RemoveMessageFilter(this);
		}
		public bool PreFilterMessage(ref Message m)
		{
			const int WM_KEYDOWN = 0x100;
			if (m.Msg == WM_KEYDOWN)
			{
				Keys keyCode = (Keys)m.WParam & Keys.KeyCode;
				if (keyCode == Keys.Escape)
				{
					this.wmpVideo.fullScreen = false;
					this.ActiveControl = this.wmpVideo;
					return true;
				}
				else if (keyCode == Keys.D1)
				{
					this.wmpVideo.fullScreen = false;
					this.ActiveControl = this.wmpVideo;
					changeScreen(1);
					return true;
				}
				else if (keyCode == Keys.D2)
				{
					this.wmpVideo.fullScreen = false;
					this.ActiveControl = this.wmpVideo;
					changeScreen(2);
					return true;
				}
				else if (keyCode == Keys.D3)
				{
					this.wmpVideo.fullScreen = false;
					this.ActiveControl = this.wmpVideo;
					changeScreen(3);
					return true;
				}
			}
			return false;
		}

		// ショートカットキー
		private void WmpVideo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (ready)
			{
				// 動画の状態を更新
				videoState = wmpVideo.playState;

				// スペースキー
				if (e.KeyCode == Keys.Space)
				{
					if ("wmppsPlaying" == videoState.ToString())
					{
						wmpVideo.Ctlcontrols.pause();
					}
					else if ("wmppsPaused" == videoState.ToString())
					{
						wmpVideo.Ctlcontrols.play();
					}
				}
				else if (e.KeyCode == Keys.E)
				{
					// 一時停止
					wmpVideo.Ctlcontrols.pause();

					// コマ送り
					((IWMPControls2)wmpVideo.Ctlcontrols).step(1);
				}
				else if (e.KeyCode == Keys.W)
				{
					// 一時停止
					wmpVideo.Ctlcontrols.pause();

					// コマ戻し
					((IWMPControls2)wmpVideo.Ctlcontrols).step(-1);
				}
				else if (e.KeyCode == Keys.OemOpenBrackets)
				{
					// 現在の再生位置を取得
					loopSta = wmpVideo.Ctlcontrols.currentPosition;
				}
				else if (e.KeyCode == Keys.OemCloseBrackets)
				{
					// 現在の再生位置を取得
					loopEnd = wmpVideo.Ctlcontrols.currentPosition;
				}
				else if (e.KeyCode == Keys.D1)
				{
					changeScreen(1);
				}
				else if (e.KeyCode == Keys.D2)
				{
					changeScreen(2);
				}
				else if (e.KeyCode == Keys.D3)
				{
					changeScreen(3);
				}
				else if (e.KeyCode == Keys.D4)
				{
					// フルスクリーン
					wmpVideo.fullScreen = true;
				}
			}
		}

		// ドラッグ＆ドロップ
		private void WindowsMediaPlayer_DragEnter(object sender, DragEventArgs e)
		{
			// コントロール内にドラッグされたとき実行される
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				// ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
				e.Effect = DragDropEffects.Copy;
			else
				// ファイル以外は受け付けない
				e.Effect = DragDropEffects.None;
		}
		private void WindowsMediaPlayer_DragDrop(object sender, DragEventArgs e)
		{
			// コントロール内にドロップされたとき実行される
			// ドロップされたすべてのファイル名を取得する
			string[] fileName =
				(string[])e.Data.GetData(DataFormats.FileDrop, false);

			// 動画のファイルパスを取得
			videoPath = fileName[0];
			videoTitle = Path.GetFileName(videoPath);

			setUpValue();
		}

		// マウスクリックで再生/一時停止をトグル
		private void WmpVideo_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
		{
			if (ready)
			{
				// 動画の状態を更新
				videoState = wmpVideo.playState;

				if ("wmppsPlaying" == videoState.ToString())
				{
					//wmpVideo.Ctlcontrols.pause();
				}
				else if ("wmppsPaused" == videoState.ToString())
				{
					//wmpVideo.Ctlcontrols.play();
				}

				//srcWidth = wmpVideo.currentMedia.imageSourceWidth;
				//srcHeight = wmpVideo.currentMedia.imageSourceHeight;
				//MessageBox.Show(srcWidth.ToString() + " : " + srcHeight.ToString() + "\r\n" +
				//"Form " + this.Width.ToString() + " : " + this.Height.ToString() + "\r\n" +
				//"Screen" + wmpVideo.Width.ToString() + " : " + wmpVideo.Height.ToString());
			}
		}

		// ボタンのアイコン差し替え
		private void BtnOpenVideo_MouseEnter(object sender, EventArgs e)
		{
			btnOpenVideo.BackgroundImage = Properties.Resources.Icon_Open_on;
		}
		private void BtnOpenVideo_MouseLeave(object sender, EventArgs e)
		{
			btnOpenVideo.BackgroundImage = Properties.Resources.Icon_Open;
		}

		// 開くボタン
		private void BtnOpenVideo_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = false, Filter = "MP4 File|*.mp4|All File|*.*" };
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				videoPath = openFileDialog.FileName;
				videoTitle = openFileDialog.SafeFileName;

				setUpValue();
			}
		}

		// 動画を開いた際の準備
		public void setUpValue()
		{
			wmpVideo.URL = videoPath;
			this.Text = videoTitle;
			ready = true;
		}

		// 動画サイズを変える
		public void changeScreen(int sizeID)
		{
			wmpVideo.fullScreen = false;
			wmpVideo.stretchToFit = true;

			srcWidth = wmpVideo.currentMedia.imageSourceWidth;
			srcHeight = wmpVideo.currentMedia.imageSourceHeight;

			switch (sizeID)
			{
				case 1:
					this.Width = (srcWidth / 2) + 16;
					this.Height = (srcHeight / 2) + 39;
					break;

				case 2:
					this.Width = (srcWidth) + 16;
					this.Height = (srcHeight) + 39;
					break;

				case 3:
					this.Width = (srcWidth * 2) + 16;
					this.Height = (srcHeight * 2) + 39;
					break;

				default:
					break;
			}
		}

	}
}