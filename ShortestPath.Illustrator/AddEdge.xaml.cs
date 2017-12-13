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
using System.Text.RegularExpressions;

namespace ShortestPath.Illustrator
{
    /// <summary>
    /// Interaction logic for AddEdge.xaml
    /// </summary>
    public partial class AddEdge : Window
    {
        GraphStructure graph;
        GraphFile GPH;
        string from, to;
        public AddEdge()
        {
            InitializeComponent();
            GPH = new GraphFile();
            graph = GPH.GetGraph();
            from = to = string.Empty;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(string.IsNullOrEmpty(to)) && !(string.IsNullOrEmpty(from)) && !(string.IsNullOrEmpty(txtWeight.Text)))
                {
                    var regx = new Regex(@"^[0-9]+$");
                    if (regx.IsMatch(txtWeight.Text) || txtWeight.Text.Trim().ToLower().Equals("inf"))
                    {
                        int ifrom = findVetex(from);
                        int ito = findVetex(to);
                        if (ifrom != -1 && ito != -1)
                        {
                            graph.Records[ifrom].Edges[ito] = txtWeight.Text;
                            GPH.SetGraph(graph);
                            this.Close();
                        }
                        else
                            throw new Exception("Wrong input! from or destination vertex has a problem!");
                    }
                    else
                        throw new Exception("invalid input as weight.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmb_from_DropDownOpened(object sender, EventArgs e)
        {
            var list = graph.Header.Vertices.ToList();
            cmb_from.ItemsSource = list;
        }

        private void cmb_from_DropDownClosed(object sender, EventArgs e)
        {
            from = (string)cmb_from.SelectedValue;
            RefreshWeight();
        }

        private void cmb_to_DropDownClosed(object sender, EventArgs e)
        {
            to = (string)cmb_to.SelectedValue;
            RefreshWeight();
        }

        private void cmb_to_DropDownOpened(object sender, EventArgs e)
        {
            var list = graph.Header.Vertices.ToList();
            cmb_to.ItemsSource = list;
        }


        void RefreshWeight()
        {
            if (!(string.IsNullOrEmpty(to)) && !(string.IsNullOrEmpty(from)))
            {
                int ifrom = findVetex(from);
                int ito = findVetex(to);
                string weight = graph.Records[ifrom].Edges[ito];
                txtWeight.Text = weight;
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnOK_Click(null, null);
        }

        int findVetex(string vertex)
        {
            if (!string.IsNullOrEmpty(vertex))
                for (int i = 0; i < graph.Header.Vertices.Count; i++)
                    if (string.Equals(graph.Records[i].VertexName, vertex))
                        return i;
            return -1;
        }
    }
}
