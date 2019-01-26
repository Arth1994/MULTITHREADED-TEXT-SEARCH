/*******
 * 
 * 
 *A Windows Form Application Developed by Arth Shah
 *to demonstrate Multi-Threaded Text Search.
 * 
 * 
 * The program is completely multi threaded and works according to the specifications.
 * 
 * It follows the UI design guidelines and is able to search the text file and
 * display the required results according to the specifications provided  
 * in the assignment document
 * 
 * The program has a responsive UI and has
 * features for learnability and usability implemented in it.
 * 
 * Written by Arth Shah - axs175430 starting  at 10/13/2018
 * 
 *
 */
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

//NameSpace for the Assignment
namespace axs175430_Asg4
{
    //Default Class for the form element
    public partial class Form1 : Form
    {
        //file path
        String selectedPath = "";

        //BackgroundWorker Object
        public BackgroundWorker backgroundWorker;

        //global search keyword
        string keyWord;

        //Concurrent Queue for progress and result report (Thread Safe)
        ConcurrentQueue<ListViewItem> queue = new ConcurrentQueue<ListViewItem>();

        //Default Constructor when form is first loaded
        public Form1()
        {

            InitializeComponent();

            //this.WindowState = FormWindowState.Maximized;
            ExpandScreen();

            toolStripStatusLabel1.Text = "WELCOME";

            //new constructor for the backgroundWorker Object
            backgroundWorker = new BackgroundWorker();
            {
                //supports the progress
                backgroundWorker.WorkerReportsProgress = true;
                //supports cancellation
                backgroundWorker.WorkerSupportsCancellation = true;
            };

            /**
             * Initializations on the Background worker
             * **/
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

            //Setting Up The Progress Bar to be Invisible initially
            toolStripProgressBar1.Enabled = toolStripProgressBar1.Visible = false;
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

        }

        /*********
         * 
         * function for 
         * column width resizing and adjustment
         * 
         * *******/
        private void listView1_ColumnWidthChanging_1(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }


        /**
         * function for final updating of the UI when 
         * background thread stops running
         */
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //error checking 
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled == true)
            {
                //When cancel button is pressed we do the following 
                toolStripStatusLabel1.Text = "OPERATION CANCELLED";
                toolStripProgressBar1.Enabled = toolStripProgressBar1.Visible = false;
            }
            else
            {
                //when we press the search button
                toolStripProgressBar1.Value = 100;
                button3.Text = "Search";
                toolStripStatusLabel1.Text = "SEARCH COMPLETED";
                toolStripProgressBar1.Enabled = toolStripProgressBar1.Visible = false;
            }
        }

        /*****
         * 
         * 
         * This is the UI thread which adds the line from the multithreaded queue object
         * into the listView element alongwith the line number and updates the progrss bar
         * 
         * 
         * *****/

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //loop to check if any lines which matched the search in the queue
            while (queue.Count > 0)
            {
                ListViewItem dequeued = null;
                if (queue.TryDequeue(out dequeued))
                {
                    listView1.Items.Add(dequeued);
                }
            }
        }

        /*
        *
        * Function to Start performing the reading of the file and search for the keyword
        * by the background worker thread
        * 
        */
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Initializing Counter to keep track of line Number
            int lineCounter = 0;
            //Word to be searched from the Search TextBox
            String wordToBeSearched = textBox3.Text;
            StreamReader streamReader;
            try
            {

                streamReader = new StreamReader(selectedPath);

            }
            catch (Exception e1)
            {
                e1.StackTrace.ToString();
                MessageBox.Show(e1.Message);
                return;
            }
            try
            {
                //reading the file
                while (!streamReader.EndOfStream)
                {
                    lineCounter++;
                    string line = streamReader.ReadLine();

                    //1 millisecond pause after reading every line from the file
                    Thread.Sleep(1);

                    //case insensitive comparison being implemented
                    if (line.ToLower().Contains(wordToBeSearched.ToLower()))
                    {
                        string searchedLine = line;
                        ListViewItem listViewItem = new ListViewItem(lineCounter.ToString());
                        //adding searched line to the listView subitem
                        listViewItem.SubItems.Add(searchedLine);
                        //ListView Entry containing LineNumber and Content is added in the concurrent queue
                        queue.Enqueue(listViewItem);
                        backgroundWorker.ReportProgress(0);
                        if (backgroundWorker.CancellationPending == true)
                        {
                            streamReader.Close();
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
            catch (FileLoadException e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        //function to adjust the listView size and the form size on inital loading.
        public void ExpandScreen()
        {
            int iHeight = Screen.PrimaryScreen.WorkingArea.Height - Height;
            Height += iHeight;
            statusStrip1.Height += iHeight;
            listView1.Height += iHeight;
            int iWidth = Screen.PrimaryScreen.WorkingArea.Width - Width;
            statusStrip1.Width += iWidth;
            Point pt = listView1.Location;
            pt.X += iWidth;
            listView1.Width += iWidth;
            Width += iWidth;
            CenterToScreen();
        }

        /*
         *
         *The code below Opens Dialog Box when you click on the browse button
         * and Adds the file path 
         * Updates Status Strip accordingly 
         * 
         ***/

        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {

                toolStripStatusLabel1.Text = "Adding File Path";
                DialogResult dialogResult = openFileDialog1.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    toolStripStatusLabel1.Text = "File Path Selected";
                    textBox2.Text = openFileDialog1.FileName;
                    selectedPath = openFileDialog1.FileName;
                }
            }
            catch (Exception e1)
            {
                e1.StackTrace.ToString();
            }
        }

        /**
         * 
         * This function is implemented when the search button is clicked
         *  
         * ***/

        private void Button3_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "SEARCHING";

            //checks if background is available
            if (backgroundWorker.IsBusy)
            {
                button3.Text = "Search";
                backgroundWorker.CancelAsync();
            }
            else
            {
                //the code below is implemented when worker is available
                backgroundWorker.RunWorkerAsync();
                button3.Text = "Cancel";
                keyWord = textBox3.Text;
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Enabled = true;
                listView1.Items.Clear();
            }
        }

        private void S(object sender, EventArgs e)
        {

        }
    }
}
