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
using System.Drawing.Drawing2D;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.Msagl;

namespace ExpertManagment
{

    public partial class Form1 : Form
    {
        List<PrimaryGraph.Verticle> loaded_graph;
        List<PrimaryGraph.Verticle> new_graph = new List<PrimaryGraph.Verticle>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "";
            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = "checkBoxColumn";

            DataGridViewCheckBoxColumn checkBoxColumn1 = new DataGridViewCheckBoxColumn();
            checkBoxColumn1.HeaderText = "";
            checkBoxColumn1.Width = 30;
            checkBoxColumn1.Name = "checkBoxColumn";

            DataGridViewButtonColumn buttonBoxColumn = new DataGridViewButtonColumn();
            buttonBoxColumn.Text = "X";
            buttonBoxColumn.Width = 30;

            DataGridViewButtonColumn buttonBoxColumn1 = new DataGridViewButtonColumn();
            buttonBoxColumn1.Text = "X";
            buttonBoxColumn1.Width = 30;

            dataGridView2.Columns.Insert(0, checkBoxColumn);
            dataGridView1.Columns.Insert(0, checkBoxColumn1);
            dataGridView2.Columns.Insert(0, buttonBoxColumn);
            dataGridView1.Columns.Insert(0, buttonBoxColumn1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(button1, new System.Drawing.Point(0, button1.Height));
        }

        private void oPENToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            if (listBox1.SelectedItem != null)
            {
                string[] verticleSplit = listBox1.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumber = Int32.Parse(verticleSplit[0]) - 1;
                label2.Text = "Verticle " + '"' + verticleSplit[1] + '"' + " connections:";
                for (int i = 0; i < loaded_graph[verticleNumber].connections.Count; i++)
                {
                    if (i > 0) richTextBox1.Text += '\n';
                    richTextBox1.Text += "Verticle: " + '"' + loaded_graph[verticleNumber].connections[i].connectedTo + '"' + " / Strength = " + '"' + loaded_graph[verticleNumber].connections[i].strength + '"';
                }
            }
        }

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

        private void renderVerticles()
        {
            for (int i = 0; i < new_graph.Count; i++)
            {
                listBox2.Items.Add(new_graph[i].verticle_id + ") " + new_graph[i].verticle);
                comboBox1.Items.Add(new_graph[i].verticle_id + ") " + new_graph[i].verticle);
                comboBox2.Items.Add(new_graph[i].verticle_id + ") " + new_graph[i].verticle);
            }
        }

        private void textBox2_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

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

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableAddVerticle(true);
            renderPrimaryGraphBinding();
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

        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();

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
                        new_graph = parser.ParseMatrix(fileText);
                        renderVerticles();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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

        private void button10_Click(object sender, EventArgs e)
        {
            var form2 = new Form2(new_graph);
            form2.Show();
        }
    }
}
