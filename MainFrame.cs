using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsGithub
{
    public partial class MainFrame : Form
    {
        TextBox textBox1;
        public MainFrame()
        {
            InitializeComponent();
        }

        private void 開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"File selected: {openFileDialog.FileName}");
            }

        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"File saved: {saveFileDialog.FileName}");
            }

        }

        private void 印刷ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 印刷設定ダイアログ
            PrintDialog printDialog1 = new System.Windows.Forms.PrintDialog();
            // PrintDocumentコンポーネントの生成
            //      　　  PrintDocument printDocument1 = new System.Drawing.Printing.PrintDocument();           // display show dialog and if user selects "Ok" document is printed
            if (printDialog1.ShowDialog() == DialogResult.OK)
                ;
//                printDocument1.Print();
        }

        private void フォントToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                this.Font = fontDialog.Font;
            }

        }

        private void カラーToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = colorDialog.Color;
            }

        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void 元に戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determine if last operation can be undone in text box.   
            if (textBox1.CanUndo)
            {
                // Undo the last operation.
                textBox1.Undo();
                // Clear the undo buffer to prevent last action from being redone.
                textBox1.ClearUndo();
            }
        }

        private void 切り取りToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ensure that text is currently selected in the text box.   
            if (textBox1.SelectedText != "")
                // Cut the selected text in the control and paste it into the Clipboard.
                textBox1.Cut();
        }

        private void 複製ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ensure that text is selected in the text box.   
            if (textBox1.SelectionLength > 0)
                // Copy the selected text to the Clipboard.
                textBox1.Copy();

        }

        private void 貼り付けToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determine if there is any text in the Clipboard to paste into the text box.
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                // Determine if any text is selected in the text box.
                if (textBox1.SelectionLength > 0)
                {
                    // Ask user if they want to paste over currently selected text.
                    if (MessageBox.Show("Do you want to paste over current selection?", "Cut Example", MessageBoxButtons.YesNo) == DialogResult.No)
                        // Move selection to the point after the current selection and paste.
                        textBox1.SelectionStart = textBox1.SelectionStart + textBox1.SelectionLength;
                }
                // Paste current text in Clipboard into text box.
                textBox1.Paste();
            }
        }

        private void メッセージ01ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Do you want to continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        }

        private void ダイアログ01ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dialog = new Dialogbox01();
            dialog.Text = "My Dialog";
            dialog.ShowDialog();

        }
    }
}
