using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Runtime.InteropServices;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.Msagl;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExpertManagment
{

    public partial class Form1 : Form
    {
        List<PrimaryGraph.Verticle> loaded_graph;
        List<PrimaryGraph.Verticle> new_graph = new List<PrimaryGraph.Verticle>();
        string base_dir;

        input_generator.Classes.GenerationRules ruleDataGlobal = new input_generator.Classes.GenerationRules();
        MatrixJson matrixJsonGlobal = new MatrixJson();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            DataGridViewButtonColumn buttonBoxColumn = new DataGridViewButtonColumn();
            buttonBoxColumn.Text = "X";
            buttonBoxColumn.Width = 35;

            DataGridViewButtonColumn buttonBoxColumn1 = new DataGridViewButtonColumn();
            buttonBoxColumn1.Text = "X";
            buttonBoxColumn1.Width = 35;

            DataGridViewButtonColumn buttonBoxColumn2 = new DataGridViewButtonColumn();
            buttonBoxColumn2.Text = "X";
            buttonBoxColumn2.Width = 35;

            DataGridViewButtonColumn buttonBoxColumn3 = new DataGridViewButtonColumn();
            buttonBoxColumn3.Text = "X";
            buttonBoxColumn3.Width = 35;

            dataGridView2.Columns.Insert(0, buttonBoxColumn);
            dataGridView1.Columns.Insert(0, buttonBoxColumn1);

            dataGridView2.Columns.Insert(1, buttonBoxColumn2);
            dataGridView1.Columns.Insert(1, buttonBoxColumn3);

            dataGridView1.Columns[2].Width = 45;
            dataGridView2.Columns[2].Width = 45;
            dataGridView1.Columns[3].Width = 180;
            dataGridView2.Columns[3].Width = 180;
            dataGridView1.Columns[4].Width = 74;
            dataGridView2.Columns[4].Width = 74;

            AppDomain domain = AppDomain.CreateDomain("/");
            base_dir = domain.BaseDirectory;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(button1, new System.Drawing.Point(0, button1.Height));
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        //Add verticle
        private void button5_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            enableAddVerticle(false);
            List<PrimaryGraph.Connections> VerticleConnection = new List<PrimaryGraph.Connections>();
            new_graph.Add(new PrimaryGraph.Verticle { verticle_id = textBox2.Text, verticle = textBox1.Text, connections = VerticleConnection });

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            renderVerticles();

            textBox1.Clear();
            textBox2.Clear();
        }

        //Save...
        private void button6_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["checkBoxColumn"].Value);
                if (isSelected)
                {
                    message += Environment.NewLine;
                    message += row.Cells[2].Value;
                }
            }

            MessageBox.Show("Selected Values" + message);
        }

        //Add influence factor
        private void button7_Click_1(object sender, EventArgs e)
        {
            primaryGraphBindingSource.Clear();
            addFactor("influence");
            renderPrimaryGraphBinding();
        }

        //Add impact factor
        private void button8_Click(object sender, EventArgs e)
        {
            primaryGraphBindingSource1.Clear();
            addFactor("impact");
            renderPrimaryGraphBinding();
        }

        //Load matrix file
        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            button7.Enabled = false;
            button8.Enabled = false;
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        string filename = openFileDialog1.FileName;
                        string fileText = System.IO.File.ReadAllText(filename);
                        MatrixParser parser = new MatrixParser();
                        new_graph = InputMatrixParser.ParseSquareMatrix(fileText, filename)[0].graph;
                        renderVerticles();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Show full graph
        private void button10_Click(object sender, EventArgs e)
        {
            var form2 = new Form2(new_graph);
            form2.Show();
        }

        //Run math algorithm
        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "All Files|*.*";
            openFileDialog2.FilterIndex = 2;
            openFileDialog2.RestoreDirectory = true;
            Stream myStream = null;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog2.OpenFile()) != null)
                    {
                        string fileName = openFileDialog2.SafeFileName;
                        string filePath = Path.GetDirectoryName(openFileDialog2.FileName);
                        string data = Classes.MathModuleController.rumModule(fileName, filePath);
                        richTextBox1.Text = data;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        //Load input data sample
        private void button12_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        string filename = openFileDialog1.FileName;
                        string fileText = System.IO.File.ReadAllText(filename);
                        MatrixParser parser = new MatrixParser();
                        List<FullMatrixData> parseData = InputMatrixParser.ParseSquareMatrix(fileText, filename);
                        richTextBox2.Text = parseData[0].yamlRules;

                        ruleDataGlobal = input_generator.Classes.RuleParser.ParseRules(parseData[0].yamlRules);
                        //input_generator.Classes.Generator.generateSquareMatrix(ruleData, parseData[0].matrixJson);
                        //richTextBox4.Text = 
                        //System.IO.File.WriteAllText("generated-matrix.txt", matrixJson);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //Load matrix to convert with rules
        private void button14_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        string filename = openFileDialog1.FileName;
                        string fileText = System.IO.File.ReadAllText(filename);
                        MatrixParser parser = new MatrixParser();
                        List<FullMatrixData> parseData = InputMatrixParser.ParseSquareMatrix(fileText, filename);
                        matrixJsonGlobal = parseData[0].matrixJson;
                        richTextBox3.Text = JsonConvert.SerializeObject(matrixJsonGlobal);

                        //GenerationRules ruleData = input_generator.Classes.RuleParser.ParseRules(parseData[0].yamlRules);
                        //input_generator.Classes.Generator.generateSquareMatrix(ruleData, parseData[0].matrixJson);
                        //richTextBox4.Text = 
                        //System.IO.File.WriteAllText("generated-matrix.txt", matrixJson);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Do matrix convert
        private void button15_Click(object sender, EventArgs e)
        {
            input_generator.Classes.MatrixJson matrixJson = new input_generator.Classes.MatrixJson();
            //IMatrixJsonClass matrixJson1 = matrixJsonGlobal;

           // matrixJson.connections = matrixJsonGlobal[0].connections;
            input_generator.Classes.Generator.generateSquareMatrix(ruleDataGlobal, matrixJson);
        }

        private void oPENToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            checkEnableAddVerticle();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            checkEnableAddVerticle();
        }

        private void textBox2_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            checkEnableAddVerticle();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != null)
            {
                button7.Enabled = true;
            }
            else
            {
                button7.Enabled = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != null)
            {
                button8.Enabled = true;
            }
            else
            {
                button8.Enabled = false;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableAddVerticle(true);
            renderPrimaryGraphBinding();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete influence verticle
            if (e.ColumnIndex == 0 && listBox2.Items.Count > 0)
            {
                string verticle_delete;
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                verticle_delete = row.Cells[2].Value.ToString();

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);

                for (int i = 0; i < new_graph.Count; i++)
                {
                    if (new_graph[i].verticle_id == verticleSplit[0])
                    {
                        for (int j = 0; j < new_graph[i].connections.Count; j++)
                        {
                            if (new_graph[i].connections[j].connectedTo == verticle_delete)
                            {
                                new_graph[i].connections.Remove(new_graph[i].connections[j]);
                                renderPrimaryGraphBinding();
                            }
                        }
                    }
                }
            }

            if (e.ColumnIndex == 1 && listBox2.Items.Count > 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                string clicked_verticle_id = row.Cells[2].Value.ToString();
                string clicked_verticle_name = row.Cells[3].Value.ToString();
                string clicked_verticle_value = row.Cells[4].Value.ToString();
                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                string action_verticle = "";

                if (Double.Parse(clicked_verticle_value) > 0)
                {
                    action_verticle = "increases ";
                }
                else
                {
                    action_verticle = "decreases ";
                }

                MessageBox.Show(@"Increasing  attribute """ + verticleSplit[1] + @""" by 1 " + action_verticle + @"""" + clicked_verticle_name + @"""" + " by " + clicked_verticle_value);

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete impact verticle
            if (e.ColumnIndex == 0)
            {
                string verticle_delete;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                verticle_delete = row.Cells[2].Value.ToString();

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);

                for (int i = 0; i < new_graph.Count; i++)
                {
                    if (new_graph[i].verticle_id == verticle_delete)
                    {
                        for (int j = 0; j < new_graph[i].connections.Count; j++)
                        {
                            if (new_graph[i].connections[j].connectedTo == verticleSplit[0])
                            {
                                new_graph[i].connections.Remove(new_graph[i].connections[j]);
                                renderPrimaryGraphBinding();
                            }
                        }
                    }
                }
            }

            if (e.ColumnIndex == 1 && listBox2.Items.Count > 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string clicked_verticle_id = row.Cells[2].Value.ToString();
                string clicked_verticle_name = row.Cells[3].Value.ToString();
                string clicked_verticle_value = row.Cells[4].Value.ToString();
                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                string action_verticle = "";

                if (Double.Parse(clicked_verticle_value) > 0)
                {
                    action_verticle = "increases ";
                }
                else
                {
                    action_verticle = "decreases ";
                }

                MessageBox.Show(@"Increasing  attribute """ + clicked_verticle_name + @""" by 1 " + action_verticle + @"""" + verticleSplit[1] + @"""" + " by " + clicked_verticle_value);

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Edit impact strength
            if (e.ColumnIndex == 4)
            {
                string verticle_edit;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                verticle_edit = row.Cells[2].Value.ToString();

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);

                for (int i = 0; i < new_graph.Count; i++)
                {
                    if (new_graph[i].verticle_id == verticle_edit)
                    {
                        for (int j = 0; j < new_graph[i].connections.Count; j++)
                        {
                            if (new_graph[i].connections[j].connectedTo == verticleSplit[0])
                            {
                                string new_value = row.Cells[4].Value.ToString();
                                new_graph[i].connections[j].strength = Double.Parse(new_value);
                                renderPrimaryGraphBinding();
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Edit influence strength
            if (e.ColumnIndex == 4)
            {
                string verticle_edit;
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                verticle_edit = row.Cells[2].Value.ToString();

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);

                for (int i = 0; i < new_graph.Count; i++)
                {
                    if (new_graph[i].verticle_id == verticleSplit[0])
                    {
                        for (int j = 0; j < new_graph[i].connections.Count; j++)
                        {
                            if (new_graph[i].connections[j].connectedTo == verticle_edit)
                            {
                                string new_value = row.Cells[4].Value.ToString();
                                new_graph[i].connections[j].strength = Double.Parse(new_value);
                                renderPrimaryGraphBinding();
                            }
                        }
                    }
                }
            }

        }

        private void addFactor(string type)
        {
            if (type == "influence")
            {
                string[] verticleSplitAdd = comboBox1.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumberAdd = Int32.Parse(verticleSplitAdd[0]);

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumber = Int32.Parse(verticleSplit[0]);

                for (int i = 0; i < new_graph.Count; i++)
                {
                    if (new_graph[i].verticle_id == verticleSplit[0])
                    {
                        new_graph[i].connections.Add(new PrimaryGraph.Connections { connectedTo = verticleNumberAdd.ToString(), strength = 0 });
                    }
                }
            }
            else if (type == "impact")
            {
                string[] verticleSplitAdd = comboBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumberAdd = Int32.Parse(verticleSplitAdd[0]);

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumber = Int32.Parse(verticleSplit[0]);

                for (int i = 0; i < new_graph.Count; i++)
                {
                    if (new_graph[i].verticle_id == verticleSplitAdd[0])
                    {
                        new_graph[i].connections.Add(new PrimaryGraph.Connections { connectedTo = verticleNumber.ToString(), strength = 0 });
                    }
                }
            }
        }

        private void renderVerticles()
        {
            for (int i = 0; i < new_graph.Count; i++)
            {
                listBox2.Items.Add(new_graph[i].verticle_id + ") " + new_graph[i].verticle);
                comboBox1.Items.Add(new_graph[i].verticle_id + ") " + new_graph[i].verticle);
                comboBox2.Items.Add(new_graph[i].verticle_id + ") " + new_graph[i].verticle);
            }
        }

        private void renderGraph()
        {
            panel1.Controls.Clear();
            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            GraphBuilder graphBuilder = new GraphBuilder();
            Microsoft.Msagl.GraphViewerGdi.GViewer graphView = graphBuilder.BuildGraph(new_graph, verticleSplit[0]);
            panel1.SuspendLayout();
            graphView.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Controls.Add(graphView);
            panel1.ResumeLayout();
        }

        private void renderPrimaryGraphBinding()
        {
            //Show influence
            primaryGraphBindingSource.Clear();
            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            int verticleNumber = Int32.Parse(verticleSplit[0]);

            for (int j = 0; j < new_graph.Count; j++)
            {
                if (verticleSplit[0] == new_graph[j].verticle_id)
                    for (int i = 0; i < new_graph[j].connections.Count; i++)
                    {
                        string connected_to = new_graph[j].connections[i].connectedTo;
                        for (int k = 0; k < new_graph.Count; k++)
                        {
                            if (new_graph[k].verticle_id == connected_to && new_graph[j].verticle != null)
                                primaryGraphBindingSource.Add(new PrimaryGraph(
                                    new_graph[k].verticle,
                                    connected_to,
                                    new_graph[j].connections[i].strength,
                                    true
                                    ));
                        }
                    }
            }

            //Show impact
            primaryGraphBindingSource1.Clear();
            for (int i = 0; i < new_graph.Count; i++)
            {
                for (int j = 0; j < new_graph[i].connections.Count; j++)
                {
                    if (new_graph[i].connections[j].connectedTo == verticleNumber.ToString())
                    {
                        string verticle_id = new_graph[i].verticle_id;
                        string verticle = new_graph[i].verticle;
                        if (verticle != null)
                            primaryGraphBindingSource1.Add(new PrimaryGraph(
                                verticle,
                                verticle_id,
                                new_graph[i].connections[j].strength,
                                true
                                ));
                    }
                }
            }
            renderGraph();
        }

        private void enableAddVerticle(bool enable)
        {
            comboBox1.Enabled = enable;
            comboBox2.Enabled = enable;
        }

        private void checkEnableAddVerticle()
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                button5.Enabled = true;
            }
            else
            {
                button5.Enabled = false;
            }
        }




    }
}
