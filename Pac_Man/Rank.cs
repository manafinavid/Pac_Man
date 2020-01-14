using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pac_Man
{
    public partial class Rank : Form
    {
        public Rank()
        {
            InitializeComponent();


        }

        private void Rank_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape|| e.KeyCode == Keys.Enter)
            {
                Close();
            }
        }

        private void Rank_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Score == "")
            {
                label1.Text += " 0";
            }
            else
            {
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.DataConnectionString);
                sql.Open();
                SqlCommand command = new SqlCommand("select player,score from Data", sql);
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader[0], reader[1]);
                }
                label1.Text += Properties.Settings.Default.Score;
            }
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Descending);
        }
    }
}
