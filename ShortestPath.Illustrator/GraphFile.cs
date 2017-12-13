using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath.Illustrator
{
    public class GraphHeader
    {
        private string graphname;
        public GraphHeader()
        {
            Vertices = new List<string>();
        }
        public GraphHeader(string graphname)
        {
            this.GraphName = graphname;
        }
        public string GraphName { get => graphname; set => graphname = value; }
        public List<string> Vertices { get; set; }

        public void AddVertex(string item)
        {
            if (Vertices == null)
                Vertices = new List<string>();
            Vertices.Add(item);
        }
        public void ChangeGraphname(string graphname)
        {
            GraphName = graphname;
        }
    }
    public class GraphRecord
    {
        private string vertexname;
        public GraphRecord()
        {
            Edges = new List<string>();
        }
        public GraphRecord(string VertexName)
        {
            this.VertexName = VertexName;
        }
        public string VertexName { get => vertexname; set => vertexname = value; }
        public List<string> Edges { get; set; }
        public void AddEdge(string item)
        {
            if (Edges == null)
                Edges = new List<string>();
            Edges.Add(item);
        }
        public void SetVertexName(string vertexName)
        {
            VertexName = vertexName;
        }
    }
    public class GraphStructure
    {
        public GraphStructure()
        {
            Header = new GraphHeader() { Vertices = new List<string>() };
            Records = new List<GraphRecord>();
        }
        public GraphHeader Header { get; set; }
        public List<GraphRecord> Records { get; set; }
        public void AddRecord(GraphRecord item)
        {
            if (Records == null)
                Records = new List<GraphRecord>();
            Records.Add(item);
        }
    }
    public class GraphFile : IDisposable
    {
        private StreamReader reader;
        private StreamWriter writer;
        public const char fileSplitter = ';';
        private string gphfilename;
        public GraphFile()
        {
            gphfilename = $"{Environment.CurrentDirectory}\\graph.gph";
        }
        public GraphStructure GetGraph()
        {
            GraphStructure gp = new GraphStructure();
            try
            {
                using (reader = new StreamReader(gphfilename))
                {
                    //getting the header
                    var splitedLine = reader.ReadLine().Split(fileSplitter).Where(x => !(x.Equals(string.Empty))).ToArray();
                    gp.Header.GraphName = splitedLine[0];
                    for (int i = 1; i < splitedLine.Length; i++)
                        gp.Header.AddVertex(splitedLine[i]);
                    //getting the records
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var splitL = line.Split(fileSplitter).Where(x => !(x.Equals(string.Empty))).ToArray();
                        GraphRecord record = new GraphRecord();
                        record.SetVertexName(splitL[0]);
                        for (int j = 1; j < splitL.Length; j++)
                            record.AddEdge(splitL[j]);
                        gp.AddRecord(record);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not read the graph, it might has a wrong format.");
            }
            return gp;
        }

        public async Task<GraphStructure> GetGraphAsync()
        {
            GraphStructure gp = new GraphStructure();
            try
            {
                using (reader = new StreamReader(gphfilename))
                {
                    //getting the header
                    var splitedLine = (await reader.ReadLineAsync())
                        .Split(fileSplitter).Where(x => !(x.Equals(string.Empty))).ToArray();
                    gp.Header.GraphName = splitedLine[0];
                    for (int i = 1; i < splitedLine.Length; i++)
                        gp.Header.AddVertex(splitedLine[i]);
                    //getting the records
                    string line = string.Empty;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var splitL = line.Split(fileSplitter).Where(x => !(x.Equals(string.Empty))).ToArray();
                        GraphRecord record = new GraphRecord();
                        record.SetVertexName(splitL[0]);
                        for (int j = 1; j < splitL.Length; j++)
                            record.AddEdge(splitL[j]);
                        gp.AddRecord(record);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not read the graph, it might has a wrong format.");
            }
            return gp;
        }

        public string[,] GetGraphEdgesMatrix()
        {
            var graph = GetGraph();
            var len = graph.Records.ElementAt(0).Edges.Count;
            string[,] arr = new string[len, len];
            try
            {
                for (int i = 0; i < len; i++)
                    for (int j = 0; j < len; j++)
                    {
                        var item = graph.Records.ElementAt(i).Edges.ElementAt(j);
                        arr[i, j] = item;
                    }
            }
            catch (Exception)
            {
                throw new Exception("Could not read the graph, it might has a wrong format.");
            }
            return arr;
        }

        public void SetGraph(GraphStructure graph)
        {
            try
            {
                using (writer = new StreamWriter(gphfilename))
                {
                    //setting the header
                    StringBuilder firstLine = new StringBuilder($"{graph.Header.GraphName}{fileSplitter}");

                    foreach (var item in graph.Header.Vertices)
                        firstLine.Append($"{item}{fileSplitter}");
                    writer.WriteLine(firstLine.ToString());
                    //setting the records

                    foreach (var record in graph.Records)
                    {
                        StringBuilder line = new StringBuilder($"{record.VertexName}{fileSplitter}");
                        foreach (var edge in record.Edges)
                        {
                            line.Append($"{edge}{fileSplitter}");
                        }
                        writer.WriteLine(line.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not save the graph [graph.gph]");
            }
        }

        public async Task SetGraphAsync(GraphStructure graph)
        {
            try
            {
                using (writer = new StreamWriter(gphfilename))
                {
                    //setting the header
                    StringBuilder firstLine = new StringBuilder($"{graph.Header.GraphName}{fileSplitter}");

                    foreach (var item in graph.Header.Vertices)
                        firstLine.Append($"{item}{fileSplitter}");
                    await writer.WriteLineAsync(firstLine.ToString());
                    //setting the records

                    foreach (var record in graph.Records)
                    {
                        StringBuilder line = new StringBuilder($"{record.VertexName}{fileSplitter}");
                        foreach (var edge in record.Edges)
                        {
                            line.Append($"{edge}{fileSplitter}");
                        }
                        await writer.WriteLineAsync(line.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not save the graph [graph.gph]");
            }
        }

        public void CleanGraph()
        {
            try
            {
                using (writer = new StreamWriter(gphfilename))
                    writer.WriteLine("dm");
            }
            catch (Exception)
            {
                throw new Exception("Could not save the graph [graph.gph]");
            }
        }

        public async Task CleanGraphAsync()
        {
            try
            {
                using (writer = new StreamWriter(gphfilename))
                    await writer.WriteLineAsync("dm");
            }
            catch (Exception)
            {
                throw new Exception("Could not save the graph [graph.gph]");
            }
        }


        public void OpenFile(string filename)
        {
            if (File.Exists(filename))
            {
                using (reader = new StreamReader(filename))
                using (writer = new StreamWriter(gphfilename))
                {
                    List<string> lines = new List<string>();
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                        if (!string.IsNullOrEmpty(line))
                            lines.Add(line);
                    foreach (var ln in lines)
                        writer.WriteLine(ln);
                }
            }
            else
                throw new FileNotFoundException($"{filename} does not exist!");
        }
        public async Task OpenFileAsync(string filename)
        {
            if (File.Exists(filename))
            {
                using (reader = new StreamReader(filename))
                using (writer = new StreamWriter(gphfilename))
                {
                    List<string> lines = new List<string>();
                    string line = string.Empty;
                    while ((line = await reader.ReadLineAsync()) != null)
                        if (!string.IsNullOrEmpty(line))
                            lines.Add(line);
                    foreach (var ln in lines)
                        await writer.WriteLineAsync(ln);
                }
            }
            else
                throw new FileNotFoundException($"{filename} does not exist!");
        }

        public string[] GetFile()
        {
            if (File.Exists(gphfilename))
            {
                using (var reader = new StreamReader(gphfilename))
                {
                    List<string> lines = new List<string>();
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                        if (!string.IsNullOrEmpty(line))
                            lines.Add(line);
                    return lines.ToArray();
                }
            }
            else
                throw new FileNotFoundException($"{gphfilename} does not exist!");
        }
        public async Task<string[]> GetFileAsync()
        {
            if (File.Exists(gphfilename))
            {
                using (var reader = new StreamReader(gphfilename))
                {
                    List<string> lines = new List<string>();
                    string line = string.Empty;
                    while ((line = await reader.ReadLineAsync()) != null)
                        if (!string.IsNullOrEmpty(line))
                            lines.Add(line);
                    return lines.ToArray();
                }
            }
            else
                throw new FileNotFoundException($"{gphfilename} does not exist!");
        }
        public void Dispose()
        {
            reader.Dispose();
            writer.Dispose();
        }
    }
}
