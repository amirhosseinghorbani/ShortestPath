using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShortestPath.Illustrator
{
    public partial class AddVertex : Window
    {
        GraphFile GPH;
        public AddVertex()
        {
            InitializeComponent();
            GPH = new GraphFile();
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string vertexName = txtVertextName.Text.Trim().ToUpper();
                var graph = GPH.GetGraph();
                if (!(graph.Header.Vertices.Exists(x => x.ToLower().Equals(vertexName.ToLower()))))
                {
                    graph.Header.AddVertex(vertexName);
                    List<string> edges = new List<string>();
                    for (int i = 0; i < graph.Records.Count; i++) //reset the edges weights(doesn't have a path to any vertices)
                    {
                        edges.Add("Inf");
                        graph.Records[i].AddEdge("Inf");
                    }
                    edges.Add("0");
                    graph.AddRecord(new GraphRecord()
                    {
                        VertexName = vertexName,
                        Edges = edges
                    });
                    GPH.SetGraph(graph); //write it to file and save.
                }
                else
                    throw new Exception("this vertex already exist.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning!");
            }
            this.Close(); //close the app
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            txtVertextName.Focus();
        }

        private void txtVertextName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnOK_Click(null, null);
        }
    }
}
