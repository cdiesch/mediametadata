using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace MediaMetadata.cs
{
    public partial class fMetadata : Form
    {
        private static String Parent_Dir = @"Z:\Movies";
        private static String Match_Repository = @"Z:\Logs\Repository";
        private static String Label_Text = "";
        private static String Working_Label_Text = "Getting and saving informatino for ";
        private static bool Save_In_Repository = true;
        private static bool Save_In_Folders = true;
        private static FileSystemWatcher watchAll;
        private BackgroundWorker Data_Retriever = new BackgroundWorker();

        public fMetadata()
        {
            Parent_Dir = "";

            InitializeComponent();
            InitializeBackgroundWorker();

            txtRepositoryDir.Text = Match_Repository;
            txtParentDir.Text = Parent_Dir;
            cbxFolders.Checked = true;
            cbxRepository.Checked = true;
            btnStop.Visible = false;
            btnStop.Enabled = false;
            txtParentDir.Enabled = false;
            txtRepositoryDir.Enabled = false;

            Update();
        }

        // Set up the BackgroundWorker object by  
        // attaching event handlers.  
        private void InitializeBackgroundWorker()
        {
            Data_Retriever.DoWork += new DoWorkEventHandler(Data_Retriever_GetMovieData);

            Data_Retriever.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Data_Retriever_WorkCompleted);
            Data_Retriever.WorkerSupportsCancellation = true;

            Data_Retriever.ProgressChanged += new ProgressChangedEventHandler(Data_Retriever_Update);
            Data_Retriever.WorkerReportsProgress = true;
        }

        private void Data_Retriever_GetMovieData(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //convert the argument to a string
            string[] movieNames = (String[])e.Argument;
            //get movie data
            writeAndSaveMovie(movieNames, worker, e);
        }

        private void Data_Retriever_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if there is an error
            if (e.Error != null)
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //if the user canceled the run
            else if (e.Cancelled)
                upDateLabel("Canceled");
        }

        private void Data_Retriever_Update(object sender, ProgressChangedEventArgs e)
        {
            prgUpdate.Value = e.ProgressPercentage;
            upDateLabel(prgUpdate.Value+"%");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStart.Visible = false;

            btnStop.Visible = true;
            btnStop.Enabled = true;

            btnParentDir.Enabled = false;
            btnRepository.Enabled = false;

            cbxRepository.Enabled = false;
            cbxFolders.Enabled = false;

            Save_In_Folders = cbxFolders.Checked;
            Save_In_Repository = cbxRepository.Checked;

            txtRepositoryDir.Enabled = false;
            txtParentDir.Enabled = false;

            if (!txtRepositoryDir.Text.EndsWith("\\"))
                txtRepositoryDir.Text += '\\';

            if (!txtParentDir.Text.EndsWith("\\"))
                txtParentDir.Text += '\\';

            Match_Repository = txtRepositoryDir.Text+@"Matched XMLs";

            if (!Directory.Exists(Match_Repository))
                Directory.CreateDirectory(Match_Repository);

            if(!Directory.Exists(Match_Repository.Replace("Matched XMLs", "Notmatched Files\\")))
                Directory.CreateDirectory(Match_Repository.Replace("Matched XMLs", "Notmatched Files\\"));

            watchAll = new FileSystemWatcher(Parent_Dir);
            watchAll.Filter = "";    //Add filter
            watchAll.Created += new FileSystemEventHandler(OnFileImport);   //Add the event action

            //Activate all file watcher
            try
            {
                watchAll.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //get all the sub directories
            String[] directories = Directory.GetDirectories(Parent_Dir);

            //run the background process
            Data_Retriever.RunWorkerAsync(directories);

            //enable the filewatcher to do some stuff
            watchAll.EnableRaisingEvents = true;
        }

        private static void OnFileImport(object source, FileSystemEventArgs e)
        {
            String fileName = e.FullPath;
            writeAndSaveMovie(fileName);
        }

        private static void writeAndSaveMovie(object ofileName)
        {
            String fileName = (string)ofileName;
            int year = 0;
            String movieName = "";

            IMDBSearch search = new IMDBSearch();
            //if it's a directory
            if (Directory.Exists(fileName))
            {
                Regex yearReg = new Regex(addSlashes(Parent_Dir) + @"\\*(?<name>.*) \((?<year>[0-9]{4})\)");

                if (yearReg.IsMatch(fileName))
                {
                    Match m = yearReg.Match(fileName);
                    year = Int32.Parse(m.Groups["year"].Value);
                    movieName = m.Groups["name"].Value;
                }

                else
                    movieName = fileName.Substring(fileName.LastIndexOf('\\') + 1);

                //update the UI
                Label_Text = Working_Label_Text + " " + movieName;

                String RepositorySaveName = Match_Repository + "\\" + movieName + ".xml";
                String misMatchRepository = Match_Repository.Replace("Matched XMLs", "Notmatched Files\\") + movieName + ".txt";
                String folderSaveName = fileName + "\\" + movieName + ".xml";
                String folderImageName = fileName + "\\folder.jpg";

                Movie movie = new Movie();
                //do some simple substitutions
                movieName = simpleSubs(movieName);
                //remove "bad" characters from the movie title
                movie.Title = removeDupSpaces(removePunc(movieName));

                //if (!File.Exists(RepositorySaveName) && !File.Exists(folderSaveName))
                //{
                    //try to match the movie
                    movie = search.getData(movieName, year);

                    //if the movie was matched
                    if (movie.WasMatched)
                    {
                        if (Save_In_Folders)
                        {
                            //save the folder in the file
                            StreamWriter writer = new StreamWriter(folderSaveName, false, Encoding.UTF8);
                            writer.Write(movie.ToString());
                            writer.Close();
                        }


                        if (Save_In_Repository)
                        {
                            //save the xml file in the repository
                            StreamWriter writer = new StreamWriter(RepositorySaveName, false, Encoding.UTF8);
                            writer.Write(movie.ToString());
                            writer.Close();
                        }

                        //if there isn't already an image for the folder
                        if (!File.Exists(folderImageName) && !String.IsNullOrEmpty(movie.ImageURL))
                        {
                            //donload and save the image
                            WebClient client = new WebClient();
                            client.DownloadFile(movie.ImageURL, folderImageName);
                        }
                    }

                    else
                    {
                        //save the xml file in the repository
                        StreamWriter writer = new StreamWriter(misMatchRepository, false);
                        writer.Write(movie.ToString());
                        writer.Close();
                    }
                //}
            }
        }

        private void writeAndSaveMovie(String[] fileNames, BackgroundWorker worker, DoWorkEventArgs e)
        {
            int highestPercent = 0;
            for (int i = 0; i < fileNames.Length; i++)
            {
                //update the UI 
                int percentComplete = (int)(((float)i / (float)fileNames.Length) * 100);
                if (percentComplete > highestPercent)
                {
                    highestPercent = percentComplete;
                    Data_Retriever.ReportProgress(percentComplete);
                }

                String fileName = fileNames[i];
                String movieName = "";
                //if the user stoped
                if (Data_Retriever.CancellationPending)
                {
                    e.Cancel = true;
                }

                //otherwise
                else
                {
                    //do work
                    int year = 0;

                    IMDBSearch search = new IMDBSearch();
                    //if it's a directory
                    if (Directory.Exists(fileName))
                    {
                        Regex yearReg = new Regex(addSlashes(Parent_Dir) + @"\\*(?<name>.*) \((?<year>[0-9]{4})\)");

                        if (yearReg.IsMatch(fileName))
                        {
                            Match m = yearReg.Match(fileName);
                            year = Int32.Parse(m.Groups["year"].Value);
                            movieName = m.Groups["name"].Value;
                        }

                        else
                            movieName = fileName.Substring(fileName.LastIndexOf('\\') + 1);

                        String RepositorySaveName = Match_Repository + "\\" + movieName + ".xml";
                        String misMatchRepository = Match_Repository.Replace("Matched XMLs", "Notmatched Files\\") + movieName + ".txt";
                        String folderSaveName = fileName + "\\" + movieName + ".xml";
                        String folderImageName = fileName + "\\folder.jpg";

                        Movie movie = new Movie();
                        //do some simple substitutions
                        movieName = simpleSubs(movieName);
                        //remove "bad" characters from the movie title
                        movie.Title = removeDupSpaces(removePunc(movieName));

                        //if(!File.Exists(RepositorySaveName) && !File.Exists(folderSaveName))
                        //{
                        //try to match the movie
                        movie = search.getData(movieName, year);

                        //if the movie was matched
                        if (movie.WasMatched)
                        {
                            if (Save_In_Folders)
                            {
                                //save the folder in the file
                                StreamWriter writer = new StreamWriter(folderSaveName, false, Encoding.UTF8);
                                writer.Write(movie.ToString());
                                writer.Close();
                            }


                            if (Save_In_Repository)
                            {
                                //save the xml file in the repository
                                StreamWriter writer = new StreamWriter(RepositorySaveName, false, Encoding.UTF8);
                                writer.Write(movie.ToString());
                                writer.Close();
                            }

                            //if there isn't already an image for the folder
                            if (!File.Exists(folderImageName) && !String.IsNullOrEmpty(movie.ImageURL))
                            {
                                //donload and save the image
                                WebClient client = new WebClient();
                                client.DownloadFile(movie.ImageURL, folderImageName);
                            }
                        }

                        else
                        {
                            //save the xml file in the repository
                            StreamWriter writer = new StreamWriter(misMatchRepository, false);
                            writer.Write(movie.ToString());
                            writer.Close();
                        }

                        //}
                    }
                }
            }
        }

        private static String removePunc(String movieName)
        {
            Char[] chars = movieName.ToArray();
            String result = "";

            foreach (char c in chars)
            {
                if (c == 'Â' || c == '-' || c == ':')
                    result += ' ';

                if (c == 'é')
                    result += 'e';

                if (c == 'ô')
                    result += 'o';

                else if (c != '\'' && c != ',' && c != '\"')
                    result += c;
            }

            return result;
        }

        private static String addSlashes(String Dir)
        {
            String result = "";
            foreach (Char c in Dir.ToArray())
            {
                if (c == '\\')
                    result += "\\\\";

                else result += c;
            }

            return result;
        }

        private static String removeDupSpaces(String movieName)
        {
            if (String.IsNullOrEmpty(movieName))
                return movieName;

            String result = "";
            for (int i = 1; i < movieName.Count(); i++)
            {
                char prev = movieName.ElementAt(i - 1);
                char curr = movieName.ElementAt(i);

                if (prev == ' ' && curr == ' ')
                {
                    result += " ";
                    i += 2;
                }
                else
                    result += prev;
            }
            result += movieName.Last();

            return result;
        }

        /**
         * Some movies on IMDb have different titles that are either extremely different, or they are a portion of the full name
         * as a temperory solution, this function will replace some of those issues
         * 
         * TODO: Allow fuzziness by checking to see if the entire name before a punctuation mark matches or the entire name on 
         * the IMDb or Movie Name side matches. This should only be allowed if the movie name does not appear on the results page
         * at all.
         * 
         */
        private static String simpleSubs(String movieName)
        {
            if (movieName.Equals("Star Wars Episode IV - A New Hope"))
                movieName = "Star wars";

            if (movieName.Contains(" - "))
                movieName = movieName.Replace(" - ", "-");

            return movieName;
        }

        private void upDateLabel(String text)
        {
            lblPercent.Text = text;
            //half the progressbar width - hald the label width + the progress bar x
            int x = ((prgUpdate.Width - lblPercent.Width) / 2) + prgUpdate.Location.X;
            lblPercent.Left = x;
            Update();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //stop the background worker
            this.Data_Retriever.CancelAsync();
            watchAll.EnableRaisingEvents = false;

            //update the UI
            upDateLabel("Canceled.");

            btnStart.Visible = true;
            btnStart.Enabled = true;

            btnStop.Enabled = false;
            btnStop.Visible = false;

            cbxFolders.Enabled = true;
            cbxRepository.Enabled = true;

            Update();
        }

        private void btnParentDir_Click(object sender, EventArgs e)
        {
            //create the folder browser and get the result
            FolderBrowserDialog browser = new FolderBrowserDialog();
            DialogResult result = browser.ShowDialog();

            //if the OK button was clicked
            if (result.Equals(DialogResult.OK))
                //save the result in the parent directory text box
                Parent_Dir = txtParentDir.Text = browser.SelectedPath;

            //otherwise don't do anything
        }

        private void btnRepository_Click(object sender, EventArgs e)
        {
            //create the folder browser and get the result
            FolderBrowserDialog browser = new FolderBrowserDialog();
            DialogResult result = browser.ShowDialog();

            //if the OK button was clicked
            if (result.Equals(DialogResult.OK))
                //save the result in the parent directory text box
                Match_Repository = txtRepositoryDir.Text = browser.SelectedPath;    

            //otherwise don't do anything
        }
    }
}
