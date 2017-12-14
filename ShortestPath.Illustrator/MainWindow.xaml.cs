using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Msagl.Drawing;
using System.Data;
using ShortestPath.Library;
using System.Diagnostics;
using System.Timers;

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
        Stopwatch watch = new Stopwatch();
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
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph(stGraph.Header.GraphName);
            if (stGraph.Records.Count > 1)
            {
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




        private void btn_addVertex_Click(object sender, RoutedEventArgs e)
        {
            AddVertex vertex = new AddVertex();
            vertex.ShowDialog();
            GenerateGraph(GPH.GetGraph());
            RefreshList();
        }

        private void btn_addEdge_Click(object sender, RoutedEventArgs e)
        {
            AddEdge edge = new AddEdge();
            edge.ShowDialog();
            GenerateGraph(GPH.GetGraph());
            RefreshList();
        }

        private void btn_cleanGraph_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to clean the graph?(delete anything)", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                GPH.CleanGraph();
                GenerateGraph(GPH.GetGraph());
                RefreshList();
            }
        }

        private void btn_saveGraph_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Destination = string.Empty;
                using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        Destination = dlg.SelectedPath;
                        string fname = System.IO.Path.Combine(Destination, "graph.gph");
                        string[] content = GPH.GetFile();

                        File.WriteAllLines(fname, content);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void btn_openFromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filename = string.Empty;
                System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                dialog.Filter = "Graph|*.gph";
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filename = dialog.FileName;
                    GPH.OpenFile(filename);
                    GenerateGraph(GPH.GetGraph());
                    RefreshList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private Library.Graph GetGraphFromGPH()
        {
            var gph = GPH.GetGraph();
            Library.Graph gp = new Library.Graph(gph.Records.Count);
            for (long i = 0; i < gph.Records.Count; i++)
            {
                gp[i] = new Library.Vertex(name: gph.Records[(int)i].VertexName,
                                            edgesSize: gph.Records[(int)i].Edges.Count);
                for (int j = 0; j < gph.Records[(int)i].Edges.Count; j++)
                {
                    string weight = gph.Records[(int)i].Edges[j];
                    string to = gph.Header.Vertices[(int)j];
                    long lweight = weight.ToLower() == "inf" ? int.MaxValue : Convert.ToInt64(weight);
                    gp[i].Edges[j] = new Library.Edge() { To = to, Weight = lweight };
                }
            }
            return gp;
        }
        private void btnExecuteWarshall_Click(object sender, RoutedEventArgs e)
        {
            if (lst_matrix.Items.Count > 0)
                FloyedWarshalExe();
            else
                MessageBox.Show("Matrix is null, add a graph then use the algorithm.");
        }

        private void FloyedWarshalExe()
        {
            try
            {
                var graph = GetGraphFromGPH();
                Library.FloyedWarshallAlgorithm floyed = new Library.FloyedWarshallAlgorithm(graph);
                floyed.Start += new EventHandler(Floyed_Start);
                floyed.StepChanged += new EventHandler<Library.StepChangedEventArgs>(Floyed_StepChanged);
                floyed.NewShortPathFound += new EventHandler<Library.NewShortPathFoundEventArgs>(Floyed_NewShortPathFound);
                floyed.End += new EventHandler(Floyed_End);
                floyed.Execute();
                LogTheShortePaths(floyed);

            }
            catch (Exception ex)
            {
                txt_log.Dispatcher.Invoke(new Action(() =>
                {
                    txt_log.Text += $"\n{ex.Message}";
                }));
            }
        }

        private void LogTheShortePaths(FloyedWarshallAlgorithm floyed)
        {
            txt_log.Text += $"\n\n\n\nShortest paths are the below paths :";
            for (int i = 0; i < floyed.Graph.Size; i++)
            {
                txt_log.Text += $"\nshortest paths to for ({floyed.Graph[i].Name}) are :";
                for (int j = 0; j < floyed.Graph.Size; j++)
                    if (i != j)
                    {
                        txt_log.Text += $"\n{floyed.Graph[i].Name} To {floyed.Graph[j].Name} is[{floyed.Graph[i].Edges[j].Weight}]: ";
                        var source = floyed.Graph[i].Name;
                        var destination = floyed.Graph[j].Name;
                        var shortestPath = floyed.ShortestPath(i, j);
                        for (int k = 0; k < shortestPath.Count(); k++)
                            if (shortestPath.Count() - 1 == k)
                                txt_log.Text += $"{shortestPath[k]}";
                            else
                                txt_log.Text += $"{shortestPath[k]} => ";
                    }
            }
        }


        private void Floyed_End(object sender, EventArgs e)
        {
            watch.Stop();
            lbl_status.Text = $"Time of calculation: {watch.ElapsedMilliseconds}ms";
            txt_log.Text += $"\nShortest path algorithm finished.";
            txt_log.Text += $"\nGraph processed in {steps} steps, {newPaths} paths has been found.";
            txt_log.Text += $"\nTime of calcultion is {watch.ElapsedMilliseconds}ms.";
            watch.Reset();
        }

        long newPaths = 0;
        long steps = 0;
        private void Floyed_NewShortPathFound(object sender, Library.NewShortPathFoundEventArgs e)
        {
            txt_log.Text += $"\nNew short path has been found, ({e.newShortPath}).";
            lbl_status.Text = $"{watch.ElapsedMilliseconds}ms";
            newPaths++;
        }
        private void Floyed_StepChanged(object sender, Library.StepChangedEventArgs e)
        {
            txt_log.Text += $"\nStep {e.step}!";
            lbl_status.Text = $"{watch.ElapsedMilliseconds}ms";
            steps = e.step;
        }

        private void Floyed_Start(object sender, EventArgs e)
        {
            watch.Start();
            txt_log.Text = "";
            newPaths = steps = 0;
            txt_log.Text += $"\nShortest path algorithm started.";
            lbl_status.Text = $"{watch.ElapsedMilliseconds}ms";
        }
    }


    public static class WpfWin32Window
    {
        public static System.Windows.Forms.IWin32Window GetIWin32Window(this System.Windows.Media.Visual visual)
        {
            var source = System.Windows.PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
            System.Windows.Forms.IWin32Window win = new OldWindow(source.Handle);
            return win;
        }
        private class OldWindow : System.Windows.Forms.IWin32Window
        {
            private readonly System.IntPtr _handle;
            public OldWindow(System.IntPtr handle)
            {
                _handle = handle;
            }
            #region IWin32Window Members
            System.IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get
                {
                    return _handle;
                }
                #endregion
            }
        }
    }
}
