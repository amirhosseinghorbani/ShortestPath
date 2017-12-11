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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using Microsoft.Msagl.Drawing;
using System.Data;

namespace ShortestPath.Illustrator
{
    /// <summary>
    /// Main window contain the different parts such as graph section which shows the graph
    /// Matrix section that shows the graph weights and details.
    /// Control section which execute the shortest path algorithm and etc.
    /// </summary>
    public partial class MainWindow : Window
    {
        GraphFile GPH;
        public MainWindow()
        {
            InitializeComponent();
            GPH = new GraphFile();

            var graph = GPH.GetGraph();
            GenerateGraph(graph);
            RefreshList();
        }
        private void GenerateGraph(GraphStructure stGraph)
        {
            Graph graph = new Graph(stGraph.Header.GraphName);

            for (int i = 0; i < stGraph.Records.Count; i++)
                for (int j = 0; j < stGraph.Records.Count; j++)
                {
                    string source = stGraph.Records.ElementAt(i).VertexName;
                    string target = stGraph.Header.Vertices.ElementAt(j);
                    string label = stGraph.Records.ElementAt(i).Edges.ElementAt(j);
                    if (i != j && !(label.ToLower().Equals("inf"))) //if it's not pointing to itself or Inf(doesn't has an edge to target vertex)
                    {
                        graph.AddEdge(source: source,
                                      edgeLabel: label,
                                      target: target); //add vertices and make an edge between them
                        RequireAttributes(graph.FindNode(source) as Node);
                    }
                }
            this.gViewer.Graph = graph;
        }
        private static void RequireAttributes(Node a)
        {
            a.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
            a.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
        }

        private void RefreshList()
        {
            var graph = GPH.GetGraph();
            DataTable dt = new DataTable();
            //design columns
            for (int i = -1; i < graph.Header.Vertices.Count; i++)
                if (i == -1)
                    dt.Columns.Add(new DataColumn("Matrix"));
                else
                    dt.Columns.Add(new DataColumn($"{graph.Header.Vertices[i]}"));
            for (int i = 0; i < graph.Records.Count; i++)
            {
                var row = dt.NewRow();
                for (int j = -1; j < graph.Records.Count; j++)
                    if (j == -1)
                        row["Matrix"] = graph.Records[i].VertexName;
                    else
                        row[$"{graph.Header.Vertices[j]}"] = graph.Records[i].Edges[j];
                dt.Rows.Add(row);
            }
            lst_matrix.AutoGenerateColumns = true;
            lst_matrix.ItemsSource = dt.DefaultView;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }




        private void btn_addVertex_Click(object sender, RoutedEventArgs e)
        {
            AddVertex vertex = new AddVertex();
            vertex.ShowDialog();
            GenerateGraph(GPH.GetGraph());
            RefreshList();
        }
    }
}
