using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System.IO;
using System.Windows.Forms;
using PSFUpdateUtility.Unpack;
using System;
using System.Collections;
using System.Threading;

namespace PSFUpdateUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeThemeBaseColor(this, ThemeManager.BaseColorLight);
            spinner.IsActive = false;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
                openFileDialog.Filter = "PSF-CAB files (*.cab)|*.cab|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    archivo_escogar.Text = openFileDialog.FileName;
                }
            }
        }

        private void compr_prg_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            prg_value.Text = string.Format("{0}", (int)compr_prg.Value);
        }

        private void proc_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var fp0 = archivo_escogar.Text;
            if (fp0.Trim().Length == 0 || !File.Exists(fp0))
            {
                MessageBox.Show("You must provide a valid file path", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                return;
            }
            var fppsf = System.IO.Path.ChangeExtension(fp0, "psf");
            if (!File.Exists(fppsf))
            {
                MessageBox.Show("There must be a corresponding CAB file along with the PSF package!", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning);
                return;
            }
            string losdir = fp0.Substring(0, fp0.LastIndexOf('.'));

            spinner.IsActive = true;

            Thread thr = new Thread(() =>
             {
                 //Extract CAB+PSF File
                  try
                  {
                      if (!Directory.Exists(losdir)) Directory.CreateDirectory(losdir);
                  }
                  catch (Exception)
                  {
                      MessageBox.Show("Failed to create the directory!", "Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                      return;
                  }
                  try
                  {
                      if (!File.Exists(losdir + "\\express.psf.cix.xml")) PreProcess.Process(fp0, losdir);
                  }
                  catch (IOException)
                  {
                      MessageBox.Show("Expand to folder failed!", "Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                      return;
                  }

                  DeltaFileList.List = new ArrayList();
                  try
                  {
                      GenerateFileList.Generate(fppsf, losdir);
                  }
                  catch (FileNotFoundException)
                  {
                      MessageBox.Show("Invalid CAB file!", "Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                      Console.WriteLine("Error: invalid CAB file.");
                      return;
                  }
                  try
                  {
                      SplitOutput.WriteOutput(fppsf, losdir);
                  }
                  catch (Exception e0)
                  {
                      MessageBox.Show("Failed to write output! Detail: " + e0.ToString() + "\n" + e0.Message, "Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                      return;
                  }
              });
              thr.Start();

            if (thr.ThreadState != ThreadState.Running)
            {
                //////////////////// Now Pack the new cab file! ///////////////////////
              /*  if (!RustFunction.PackCab(losdir, losdir + "_nuevo.cab", (short)compr_prg.Value))
                {
                    MessageBox.Show("Unable to pack the new merged cab file!", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                } */
                spinner.IsActive = false;

            }
        }
    }
}
