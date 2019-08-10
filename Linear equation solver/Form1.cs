using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Reflection;

namespace Linear_equation_solver
{
    public partial class Main_solver : Form
    {
        OleDbConnection db_connection = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = ..\\..\\..\\Linear equation system solver database.mdb;");

        float[] first_equation = new float[4];
        float[] second_equation = new float[4];
        float[] third_equation = new float[4];

        string v2_first_equation_string, v2_second_equation_string;
        string v3_first_equation_string, v3_second_equation_string, v3_third_equation_string;

        public Main_solver()
        {
            InitializeComponent();
        }

        //functions for systems with 2 variables
        
        private void v2_solve_Button_Click(object sender, EventArgs e)
        {
            if(v2_Fill())
            {
                v2_Print(v2_Calculate_X(), v2_Calculate_Y());
                v2_Database_Fill();
            }
            
        }

        private bool v2_Fill()
        {
            try
            {   //parsing textbox values to arrays for each function respectively
                first_equation[0] = float.Parse(v2_a1.Text.Replace('.',','));
                first_equation[1] = float.Parse(v2_b1.Text.Replace('.', ','));
                first_equation[2] = float.Parse(v2_c1.Text.Replace('.', ','));

                second_equation[0] = float.Parse(v2_a2.Text.Replace('.', ','));
                second_equation[1] = float.Parse(v2_b2.Text.Replace('.', ','));
                second_equation[2] = float.Parse(v2_c2.Text.Replace('.', ','));

                return true;
            }
            catch
            {   //error message in case of invalid entry
                MessageBox.Show("An error has occured. Check the entered values and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreach (int i in first_equation)
                {
                    first_equation[i] = 0;
                    second_equation[i] = 0;
                }
                return false;
            }
        }

        private float v2_Calculate_X()
        {
            float d, dx, x;
            
            //calculating x using the matrix method
            d = first_equation[0] * second_equation[1] - second_equation[0] * first_equation[1];
            dx = first_equation[2] * second_equation[1] - second_equation[2] * first_equation[1];
            
            x = dx / d;
            return x;
        }

        private float v2_Calculate_Y()
        {
            float d, dy, y;

            //calculating y using the matrix method
            d = first_equation[0] * second_equation[1] - second_equation[0] * first_equation[1];
            dy = first_equation[0] * second_equation[2] - second_equation[0] * first_equation[2];

            y = dy / d;
            return y;
        }

        private void v2_Print(float x, float y)
        {
            v2_solution.Clear();

            //the following if statements check whether the numbers in the first equation are negative, positive or 0 in order to print the equation in a 'human' way
              
            if(first_equation[0] == 1)
                v2_solution.Text = "x ";
            else if(first_equation[0] == -1)
                v2_solution.Text = "- " + "x ";
            else
            { 
                if(first_equation[0] != 0)
                    v2_solution.Text = first_equation[0] + "x ";
            } 

            if (first_equation[1] == 1)
            {
                if(first_equation[0] != 0)
                        v2_solution.Text += "+ " + "y";
                    else
                        v2_solution.Text += "y";
            }

            else if(first_equation[1] == -1)
                v2_solution.Text += "- " + "y ";

            else
            { 
                if(first_equation[1] < 0)
                    v2_solution.Text += first_equation[1] + "y";
                else if(first_equation[1] > 0 && first_equation[0] != 0)
                    v2_solution.Text += "+ " + first_equation[1] + "y";
                else if (first_equation[1] > 0 && first_equation[0] == 0)
                    v2_solution.Text += first_equation[1] + "y";
            }

            v2_solution.Text += " = " + first_equation[2];

            v2_first_equation_string = v2_solution.Text;

            v2_solution.Text += Environment.NewLine;


            //the following if statements check whether the numbers in the second equation are negative, positive or 0 in order to print the equation in a 'human' way
            
            if(second_equation[0] == 1)
                v2_solution.Text += "x ";
            else if(second_equation[0] == -1)
                v2_solution.Text += "- " + "x ";
            else
            { 
                if(second_equation[0] != 0)
                    v2_solution.Text += second_equation[0] + "x ";
            } 

            if (second_equation[1] == 1)
            {
                if(second_equation[0] == 0)
                    v2_solution.Text += "y";   
                else
                    v2_solution.Text += "+ " + "y";
            }

            else if(second_equation[1] == -1)
                v2_solution.Text += "- " + "y";

            else
            { 
                if(second_equation[1] < 0)
                    v2_solution.Text += second_equation[1] + "y";
                else if(second_equation[1] > 0 && second_equation[0] != 0)
                    v2_solution.Text += "+ " + second_equation[1] + "y";
                else if (second_equation[1] > 0 && second_equation[0] == 0)
                    v2_solution.Text += second_equation[1] + "y";
            }

            v2_solution.Text += " = " + second_equation[2];

            v2_second_equation_string = v2_solution.Text.Remove(0, v2_first_equation_string.Length+1);

            v2_solution.Text += Environment.NewLine + Environment.NewLine;


            //the result of X and Y are printed
            v2_solution.Text += "x = " + x;

            v2_solution.Text += Environment.NewLine;

            v2_solution.Text += "y = " + y;
        }

        private void v2_Database_Fill()
        {
            try
            {
                db_string_initialization();
                db_connection.Open();
                OleDbCommand command = db_connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = ("INSERT INTO 2_variable_history(first_equation_string, second_equation_string, x_value, y_value, a1_value, b1_value, c1_value, a2_value, b2_value, c2_value)values('" +
                    v2_first_equation_string + "', '" + v2_second_equation_string + "','" + v2_Calculate_X() + "','" + v2_Calculate_Y() + "','" + first_equation[0] + "','" + first_equation[1] + "','" + first_equation[2] + "','" +
                    second_equation[0] + "','" + second_equation[1] + "','" + second_equation[2] + "')");
                command.ExecuteNonQuery();
                db_connection.Close();
            }
            catch
            {
                db_connection.Close();
                v2_solution.Clear();
                MessageBox.Show("An error has occured. Check the entered values and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void v2_new_Button_Click(object sender, EventArgs e)
        {
            v2_solution.Clear();
            v2_a1.Text = "0";
            v2_b1.Text = "0";
            v2_c1.Text = "0";

            v2_a2.Text = "0";
            v2_b2.Text = "0";
            v2_c2.Text = "0";
        }

        private void v2_graph_Button_Click(object sender, EventArgs e)
        {
            v2_Fill();

            Graph graph = new Graph(v2_first_equation_string, v2_second_equation_string, first_equation, second_equation);
            graph.ShowDialog();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////functions for systems with 3 variables/////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void v3_solve_Button_Click(object sender, EventArgs e)
        {
            if(v3_Fill())
            { 
                v3_Print(v3_Calculate_X(), v3_Calculate_Y(), v3_Calculate_Z());
                v3_Database_Fill();
            }
        }

        private bool v3_Fill()
        {
            try
            {   //parsing textbox values to arrays for each function respectively
                first_equation[0] = float.Parse(v3_a1.Text.Replace('.', ','));
                first_equation[1] = float.Parse(v3_b1.Text.Replace('.', ','));
                first_equation[2] = float.Parse(v3_c1.Text.Replace('.', ','));
                first_equation[3] = float.Parse(v3_d1.Text.Replace('.', ','));

                second_equation[0] = float.Parse(v3_a2.Text.Replace('.', ','));
                second_equation[1] = float.Parse(v3_b2.Text.Replace('.', ','));
                second_equation[2] = float.Parse(v3_c2.Text.Replace('.', ','));
                second_equation[3] = float.Parse(v3_d2.Text.Replace('.', ','));

                third_equation[0] = float.Parse(v3_a3.Text.Replace('.', ','));
                third_equation[1] = float.Parse(v3_b3.Text.Replace('.', ','));
                third_equation[2] = float.Parse(v3_c3.Text.Replace('.', ','));
                third_equation[3] = float.Parse(v3_d3.Text.Replace('.', ','));

                return true;
            }
            catch
            {   //error message in case of invalid entry
                MessageBox.Show("An error has occured. Check the entered values and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreach (int i in first_equation)
                {
                    first_equation[i] = 0;
                    second_equation[i] = 0;
                    third_equation[i] = 0;
                }
                return false;
            }
        }

        private float v3_Calculate_X()
        {
            float d = 0, dx = 0, x;

            //calculating x using the matrix method
            d += first_equation[0] * second_equation[1] * third_equation[2];
            d += first_equation[1] * second_equation[2] * third_equation[0];
            d += first_equation[2] * second_equation[0] * third_equation[1];
            d -= first_equation[2] * second_equation[1] * third_equation[0];
            d -= first_equation[0] * second_equation[2] * third_equation[1];
            d -= first_equation[1] * second_equation[0] * third_equation[2];

            dx += first_equation[3] * second_equation[1] * third_equation[2];
            dx += first_equation[1] * second_equation[2] * third_equation[3];
            dx += first_equation[2] * second_equation[3] * third_equation[1];
            dx -= first_equation[2] * second_equation[1] * third_equation[3];
            dx -= first_equation[3] * second_equation[2] * third_equation[1];
            dx -= first_equation[1] * second_equation[3] * third_equation[2];
 
            x = dx / d;
            return x;
        }

        private float v3_Calculate_Y()
        {
            float d = 0, dy = 0, y;

            //calculating x using the matrix method
            d += first_equation[0] * second_equation[1] * third_equation[2];
            d += first_equation[1] * second_equation[2] * third_equation[0];
            d += first_equation[2] * second_equation[0] * third_equation[1];
            d -= first_equation[2] * second_equation[1] * third_equation[0];
            d -= first_equation[0] * second_equation[2] * third_equation[1];
            d -= first_equation[1] * second_equation[0] * third_equation[2];

            dy += first_equation[0] * second_equation[3] * third_equation[2];
            dy += first_equation[3] * second_equation[2] * third_equation[0];
            dy += first_equation[2] * second_equation[0] * third_equation[3];
            dy -= first_equation[2] * second_equation[3] * third_equation[0];
            dy -= first_equation[0] * second_equation[2] * third_equation[3];
            dy -= first_equation[3] * second_equation[0] * third_equation[2];

            y = dy / d;
            return y;
        }

        private float v3_Calculate_Z()
        {
            float d = 0, dz = 0, z;

            //calculating x using the matrix method
            d += first_equation[0] * second_equation[1] * third_equation[2];
            d += first_equation[1] * second_equation[2] * third_equation[0];
            d += first_equation[2] * second_equation[0] * third_equation[1];
            d -= first_equation[2] * second_equation[1] * third_equation[0];
            d -= first_equation[0] * second_equation[2] * third_equation[1];
            d -= first_equation[1] * second_equation[0] * third_equation[2];

            dz += first_equation[0] * second_equation[1] * third_equation[3];
            dz += first_equation[1] * second_equation[3] * third_equation[0];
            dz += first_equation[3] * second_equation[0] * third_equation[1];
            dz -= first_equation[3] * second_equation[1] * third_equation[0];
            dz -= first_equation[0] * second_equation[3] * third_equation[1];
            dz -= first_equation[1] * second_equation[0] * third_equation[3];

            z = dz / d;
            return z;
        }

        private void v3_Print(float x, float y, float z)
        {
            v3_solution.Clear();

            //the following if statements check whether the numbers in the first equation are negative, positive or 0 in order to print the equation in a 'human' way

            if (first_equation[0] == 1)
                v3_solution.Text = "x ";
            else if (first_equation[0] == -1)
                v3_solution.Text = "- " + "x ";
            else
            {
                if (first_equation[0] != 0)
                    v3_solution.Text = first_equation[0] + "x ";
            }

            if (first_equation[1] == 1)
            {
                if (first_equation[0] != 0)
                    v3_solution.Text += "+ " + "y ";
                else
                    v3_solution.Text += "y ";
            }

            else if (first_equation[1] == -1)
                v3_solution.Text += "- " + "y ";

            else
            {
                if (first_equation[1] < 0)
                    v3_solution.Text += first_equation[1] + "y ";
                else if (first_equation[1] > 0 && first_equation[0] != 0)
                    v3_solution.Text += "+ " + first_equation[1] + "y ";
                else if (first_equation[1] > 0 && first_equation[0] == 0)
                    v3_solution.Text += first_equation[1] + "y ";
            }

            if (first_equation[2] == 1)
            {
                if (first_equation[1] == 0 && first_equation[0] == 0)
                    v3_solution.Text += "z";
                else
                    v3_solution.Text += "+ " + "z";
            }

            else if (first_equation[2] == -1)
                v3_solution.Text += "- " + "z";

            else
            {
                if (first_equation[2] < 0)
                    v3_solution.Text += first_equation[2] + "z";
                else if (first_equation[2] > 0 && (first_equation[1] != 0 || first_equation[0] != 0))
                    v3_solution.Text += "+ " + first_equation[2] + "z";
                else if (first_equation[2] > 0 && first_equation[1] == 0 && first_equation[0] == 0)
                    v3_solution.Text += first_equation[2] + "z";
            }

            v3_solution.Text += " = " + first_equation[3];
            v3_first_equation_string = v3_solution.Text;

            v3_solution.Text += Environment.NewLine;

            if (second_equation[0] == 1)
                v3_solution.Text += "x ";
            else if (second_equation[0] == -1)
                v3_solution.Text += "- " + "x ";
            else
            {
                if (second_equation[0] != 0)
                    v3_solution.Text += second_equation[0] + "x ";
            }

            if (second_equation[1] == 1)
            {
                if (second_equation[0] != 0)
                    v3_solution.Text += "+ " + "y ";
                else
                    v3_solution.Text += "y ";
            }

            else if (second_equation[1] == -1)
                v3_solution.Text += "- " + "y ";

            else
            {
                if (second_equation[1] < 0)
                    v3_solution.Text += second_equation[1] + "y ";
                else if (second_equation[1] > 0 && second_equation[0] != 0)
                    v3_solution.Text += "+ " + second_equation[1] + "y ";
                else if (second_equation[1] > 0 && second_equation[0] == 0)
                    v3_solution.Text += second_equation[1] + "y ";
            }

            if (second_equation[2] == 1)
            {
                if (second_equation[1] == 0 && second_equation[0] == 0)
                    v3_solution.Text += "z";
                else
                    v3_solution.Text += "+ " + "z";
            }

            else if (second_equation[2] == -1)
                v3_solution.Text += "- " + "z";

            else
            {
                if (second_equation[2] < 0)
                    v3_solution.Text += second_equation[2] + "z";
                else if (second_equation[2] > 0 && (second_equation[1] != 0 || second_equation[0] != 0))
                    v3_solution.Text += "+ " + second_equation[2] + "z";
                else if (second_equation[2] > 0 && second_equation[1] == 0 && second_equation[0] == 0)
                    v3_solution.Text += second_equation[2] + "z";
            }

            v3_solution.Text += " = " + second_equation[3];
            v3_second_equation_string = v3_solution.Text.Remove(0, v3_first_equation_string.Length + 1);

            v3_solution.Text += Environment.NewLine;

            if (third_equation[0] == 1)
                v3_solution.Text += "x ";
            else if (third_equation[0] == -1)
                v3_solution.Text += "- " + "x ";
            else
            {
                if (third_equation[0] != 0)
                    v3_solution.Text += third_equation[0] + "x ";
            }

            if (third_equation[1] == 1)
            {
                if (third_equation[0] != 0)
                    v3_solution.Text += "+ " + "y ";
                else
                    v3_solution.Text += "y ";
            }

            else if (third_equation[1] == -1)
                v3_solution.Text += "- " + "y ";

            else
            {
                if (third_equation[1] < 0)
                    v3_solution.Text += third_equation[1] + "y ";
                else if (third_equation[1] > 0 && third_equation[0] != 0)
                    v3_solution.Text += "+ " + third_equation[1] + "y ";
                else if (third_equation[1] > 0 && third_equation[0] == 0)
                    v3_solution.Text += third_equation[1] + "y ";
            }

            if (third_equation[2] == 1)
            {
                if (third_equation[1] == 0 && third_equation[0] == 0)
                    v3_solution.Text += "z";
                else
                    v3_solution.Text += "+ " + "z";
            }

            else if (third_equation[2] == -1)
                v3_solution.Text += "- " + "z";

            else
            {
                if (third_equation[2] < 0)
                    v3_solution.Text += third_equation[2] + "z";
                else if (third_equation[2] > 0 && (third_equation[1] != 0 || third_equation[0] != 0))
                    v3_solution.Text += "+ " + third_equation[2] + "z";
                else if (third_equation[2] > 0 && third_equation[1] == 0 && third_equation[0] == 0)
                    v3_solution.Text += third_equation[2] + "z";
            }

            v3_solution.Text += " = " + third_equation[3];
            v3_third_equation_string = v3_solution.Text.Remove(0, v3_first_equation_string.Length + v3_second_equation_string.Length + 1);

            v3_solution.Text += Environment.NewLine;
            v3_solution.Text += Environment.NewLine;

            //the result of X, Y and Z are printed
            v3_solution.Text += "x = " + x;
            v3_solution.Text += Environment.NewLine;

            v3_solution.Text += "y = " + y;
            v3_solution.Text += Environment.NewLine;

            v3_solution.Text += "z = " + z;
        }
        private void v3_Database_Fill()
        {
            try
            {
                db_string_initialization();
                db_connection.Open();
                OleDbCommand command = db_connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = ("INSERT INTO 3_variable_history(first_equation_string, second_equation_string, third_equation_string, x_value, y_value, z_value, " +
                                        "a1_value, b1_value, c1_value, d1_value, a2_value, b2_value, c2_value, d2_value, a3_value, b3_value, c3_value, d3_value)values('" +
                                        v3_first_equation_string + "', '" + v3_second_equation_string + "','" + v3_third_equation_string + "','" + 
                                        v3_Calculate_X() + "','" + v3_Calculate_Y() + "','" + v3_Calculate_Z() + "','" +
                                        first_equation[0] + "','" + first_equation[1] + "','" + first_equation[2] + "','" + first_equation[3] + "','" +
                                        second_equation[0] + "','" + second_equation[1] + "','" + second_equation[2] + "','" + second_equation[3] + "','" +
                                        third_equation[0] + "','" + third_equation[1] + "','" + third_equation[2] + "','" + third_equation[3] + "')");
                command.ExecuteNonQuery();
                db_connection.Close();
            }
            catch
            {
                db_connection.Close();
                v3_solution.Clear();
                MessageBox.Show("An error has occured. Check the entered values and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void v3_new_Button_Click(object sender, EventArgs e)
        {
            v3_solution.Clear();
            v3_a1.Text = "0";
            v3_b1.Text = "0";
            v3_c1.Text = "0";
            v3_d1.Text = "0";

            v3_a2.Text = "0";
            v3_b2.Text = "0";
            v3_c2.Text = "0";
            v3_d2.Text = "0";

            v3_a3.Text = "0";
            v3_b3.Text = "0";
            v3_c3.Text = "0";
            v3_d3.Text = "0";
        }

        private void dataSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataSelectionComboBox.SelectedIndex == 0)
            {
                v2_Data_Show();
            }
            else
            {
                v3_Data_Show();
            }
        }

        private void v2_Data_Show()
        {
            string query = "SELECT first_equation_string AS First_equation, second_equation_string AS Second_equation, x_value AS X, y_value AS Y FROM 2_variable_history";
            
            db_string_initialization();
            data_Fill(query);
        }

        private void v3_Data_Show()
        {
            string query = "SELECT first_equation_string AS First_equation, second_equation_string AS Second_equation, third_equation_string AS Third_equation" + 
                            "x_value AS X, y_value AS Y, z_value AS Z FROM 3_variable_history";
            
            db_string_initialization();
            data_Fill(query);
        }

        private void db_string_initialization()
        {
            db_connection.ConnectionString = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = ..\\..\\..\\Linear equation system solver database.mdb;";
        }

        private void data_Fill(string query)
        {
            using (db_connection)
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, db_connection))
                {
                    db_string_initialization();
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView.DataSource = ds.Tables[0];
                }
            }
            db_connection.Close();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (dataSelectionComboBox.SelectedIndex == 0)
            {
                try
                {
                    int rowIndex = dataGridView.CurrentCell.RowIndex;

                    db_string_initialization();
                    string query = "SELECT * FROM 2_variable_history";
                    data_Fill(query);

                    dataGridView.Rows[rowIndex].Selected = true;

                    v2_a1.Text = dataGridView.SelectedRows[0].Cells[5].Value.ToString();
                    v2_b1.Text = dataGridView.SelectedRows[0].Cells[6].Value.ToString();
                    v2_c1.Text = dataGridView.SelectedRows[0].Cells[7].Value.ToString();
                    v2_a2.Text = dataGridView.SelectedRows[0].Cells[8].Value.ToString();
                    v2_b2.Text = dataGridView.SelectedRows[0].Cells[9].Value.ToString();
                    v2_c2.Text = dataGridView.SelectedRows[0].Cells[10].Value.ToString();

                    v2_Data_Show();
                    tab_Selection.SelectedIndex = 0;
                }
                catch
                {
                    MessageBox.Show("An error has occured. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    int rowIndex = dataGridView.CurrentCell.RowIndex;

                    db_string_initialization();
                    string query = "SELECT * FROM 3_variable_history";
                    data_Fill(query);

                    dataGridView.Rows[rowIndex].Selected = true;

                    v3_a1.Text = dataGridView.SelectedRows[0].Cells[7].Value.ToString();
                    v3_b1.Text = dataGridView.SelectedRows[0].Cells[8].Value.ToString();
                    v3_c1.Text = dataGridView.SelectedRows[0].Cells[9].Value.ToString();
                    v3_d1.Text = dataGridView.SelectedRows[0].Cells[10].Value.ToString();
                    v3_a2.Text = dataGridView.SelectedRows[0].Cells[11].Value.ToString();
                    v3_b2.Text = dataGridView.SelectedRows[0].Cells[12].Value.ToString();
                    v3_c2.Text = dataGridView.SelectedRows[0].Cells[13].Value.ToString();
                    v3_d2.Text = dataGridView.SelectedRows[0].Cells[14].Value.ToString();
                    v3_a3.Text = dataGridView.SelectedRows[0].Cells[15].Value.ToString();
                    v3_b3.Text = dataGridView.SelectedRows[0].Cells[16].Value.ToString();
                    v3_c3.Text = dataGridView.SelectedRows[0].Cells[17].Value.ToString();
                    v3_d3.Text = dataGridView.SelectedRows[0].Cells[18].Value.ToString();

                    v3_Data_Show();
                    tab_Selection.SelectedIndex = 1;
                }
                catch
                {
                    MessageBox.Show("An error has occured. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the marked entry from the database?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if(dialogResult == DialogResult.Yes)
            {
                    if (dataSelectionComboBox.SelectedIndex == 0)
                    {
                        try
                        {
                            int rowIndex = dataGridView.CurrentCell.RowIndex;

                            string query = "SELECT * FROM 2_variable_history";
                            data_Fill(query);

                            db_string_initialization();
                            db_connection.Open();

                            dataGridView.Rows[rowIndex].Selected = true;

                            OleDbCommand command = db_connection.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = ("DELETE FROM 2_variable_history WHERE id = " + dataGridView.SelectedRows[0].Cells[0].Value);
                            command.ExecuteNonQuery();
                            db_connection.Close();

                            v2_Data_Show();
                        }
                        catch
                        {
                            MessageBox.Show("An error has occured. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            db_connection.Close();
                        }
                    }
                    else
                    {
                        try
                        {
                            int rowIndex = dataGridView.CurrentCell.RowIndex;

                            string query = "SELECT * FROM 3_variable_history";
                            data_Fill(query);

                            db_string_initialization();
                            db_connection.Open();

                            dataGridView.Rows[rowIndex].Selected = true;

                            OleDbCommand command = db_connection.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = ("DELETE FROM 3_variable_history WHERE id = " + dataGridView.SelectedRows[0].Cells[0].Value);
                            command.ExecuteNonQuery();
                            db_connection.Close();

                            v3_Data_Show();
                        }
                        catch
                        {
                            MessageBox.Show("An error has occured. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            db_connection.Close();
                        }
                    }
            }
        }

        //TOOLTIPS:

        private void v2_solve_Button_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(v2_solve_Button, "Solve the system of equations");
        }

        private void v2_new_Button_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(v2_new_Button, "Clear all the fields");
        }

        private void v2_graph_Button_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(v2_graph_Button, "Graph the system of equations");
        }

        private void v3_solve_Button_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(v3_solve_Button, "Solve the system of equations");
        }

        private void v3_new_Button_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(v3_new_Button, "Clear all the fields");
        }

        private void dataSelectionComboBox_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(dataSelectionComboBox, "Select which data to display");
        }

        private void loadButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(loadButton, "Load the selected data to the solver interface");
        }

        
    }
}