using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;
using System.Data.SqlClient;

namespace YTLearningForms
{
    public partial class IO : Form
    {
        public IO()
        {
            InitializeComponent();
        }

        private async void IO_Load(object sender, EventArgs e)
        {
            await Task.Delay(1);
            List<string> commands = new List<string>();
            commands.Add("Make directory");
            commands.Add("New File");
            commands.Add("PHP or Composer Command");
            foreach (string item in commands)
            {
                await Task.Delay(1);
                metroSetComboBoxActions.Items.Add(item.ToString());
            }
            string connectionstring = @"Data Source=DESKTOP-9CS6TTK\SQLEXPRESS;Initial Catalog=YTLearning;User ID=abdelouahedlabrigui;Password=rootroot.;Trusted_Connection=yes;";
            using (SqlConnection connection = new SqlConnection(connectionstring.ToString()))
            {
                connection.Open();
                SqlCommand select = new SqlCommand("SELECT Id,actions,absolute_path,command FROM _io ORDER BY Id ASC;", connection);
                using (SqlDataAdapter reader = new SqlDataAdapter(select))
                {
                    DataTable dataTable = new DataTable();
                    reader.Fill(dataTable);
                    dataGridViewYTLearning.DataSource = dataTable;
                }
                connection.Close();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private async void metroSetButtonCommand_Click(object sender, EventArgs e)
        {
            await Task.Delay(1);
            string fullPath = metroSetTextBoxAbsolutePath.Text.ToString();
            string command = metroSetTextBoxCommand.Text.ToString();
            string connectionstring = @"Data Source=DESKTOP-9CS6TTK\SQLEXPRESS;Initial Catalog=YTLearning;User ID=abdelouahedlabrigui;Password=rootroot.;Trusted_Connection=yes;";
            using (SqlConnection connection = new SqlConnection(connectionstring.ToString()))
            {
                string insert = "INSERT INTO _io (actions,absolute_path,command) VALUES (@actions,@absolute_path,@command)";
                connection.Open();
                if (metroSetComboBoxActions.Text == "New File")
                {
                    using (SqlCommand sqlCommand = new SqlCommand(insert, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@actions", metroSetComboBoxActions.Text.ToString());
                        sqlCommand.Parameters.AddWithValue("@absolute_path", metroSetTextBoxAbsolutePath.Text.ToString());
                        sqlCommand.Parameters.AddWithValue("@command", "cd " + fullPath.ToString() + " ; New-Item -Path . -Name " + '"' + command.ToString() + '"' + "".ToString());
                        sqlCommand.ExecuteNonQuery();
                        await Task.Delay(1);
                    }
                    using (PowerShell powerShell = PowerShell.Create())
                    {
                        powerShell.AddScript(@"cd " + fullPath.ToString() + " ; New-Item -Path . -Name " + '"' + command.ToString() + '"' + "");
                        powerShell.Invoke();
                        await Task.Delay(1);
                    }
                }
                if (metroSetComboBoxActions.Text == "Make directory")
                {
                    using (SqlCommand sqlCommand = new SqlCommand(insert, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@actions", metroSetComboBoxActions.Text.ToString());
                        sqlCommand.Parameters.AddWithValue("@absolute_path", metroSetTextBoxAbsolutePath.Text.ToString());
                        sqlCommand.Parameters.AddWithValue("@command", "cd " + fullPath.ToString() + " ; mkdir " + command.ToString() + "".ToString());
                        sqlCommand.ExecuteNonQuery();
                        await Task.Delay(1);
                    }
                    using (PowerShell powerShell = PowerShell.Create())
                    {
                        powerShell.AddScript(@"cd " + fullPath.ToString() + " ; mkdir " + command.ToString());
                        powerShell.Invoke();
                        await Task.Delay(1);
                    }
                }
                if (metroSetComboBoxActions.Text == "PHP or Composer Command")
                {
                    using (SqlCommand sqlCommand = new SqlCommand(insert, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@actions", metroSetComboBoxActions.Text.ToString());
                        sqlCommand.Parameters.AddWithValue("@absolute_path", metroSetTextBoxAbsolutePath.Text.ToString());
                        sqlCommand.Parameters.AddWithValue("@command", "cd " + fullPath.ToString() + " ; " + command.ToString() + "".ToString());
                        sqlCommand.ExecuteNonQuery();
                        await Task.Delay(1);
                    }
                    using (PowerShell powerShell = PowerShell.Create())
                    {
                        try
                        {
                            powerShell.AddScript(@"cd " + fullPath.ToString() + " ; " + command.ToString());
                            powerShell.Invoke();
                            await Task.Delay(2);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("[Notification!!]" + ex.Message);
                        }
                    }
                }
                await Task.Delay(1);
                SqlCommand select = new SqlCommand("SELECT Id,actions,absolute_path,command FROM _io ORDER BY Id ASC;", connection);
                using (SqlDataAdapter reader = new SqlDataAdapter(select))
                {
                    DataTable dataTable = new DataTable();
                    reader.Fill(dataTable);
                    dataGridViewYTLearning.DataSource = dataTable;
                }
                connection.Close();
            }

        }

        private async void metroSetComboBoxActions_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(1);
            if (metroSetComboBoxActions.Text == "New File")
            {
                metroSetTextBoxAbsolutePath.Text = "";
                metroSetTextBoxAbsolutePath.Enabled = true;
                metroSetLabel3.Text = "Absolute Path For New File";
                metroSetLabel2.Text = "New File Name";
                await Task.Delay(1);
            }
            if (metroSetComboBoxActions.Text == "Make directory")
            {
                metroSetTextBoxAbsolutePath.Text = "";
                metroSetTextBoxAbsolutePath.Enabled = true;
                metroSetLabel3.Text = "Absolute Path Directory";
                metroSetLabel2.Text = "New Folder Name";
                await Task.Delay(1);
            }
            if (metroSetComboBoxActions.Text == "PHP or Composer Command")
            {
                metroSetTextBoxAbsolutePath.Text = "D:\\Entrepreneurship\\YTlearning\\web";
                metroSetTextBoxAbsolutePath.Enabled = false;
                metroSetLabel3.Text = "Absolute Path Is: ";
                metroSetLabel2.Text = "PHP or Composer Command";
                await Task.Delay(1);
            }
        }

        private async void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Delay(1);
            Form1 form1 = new Form1();
            form1.Show();
        }
    }
}
