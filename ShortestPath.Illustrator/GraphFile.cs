using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath.Illustrator
{
    public struct GraphHeader
    {
        public string GraphName { get; set; }
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
    public struct GraphRecord
    {
        public string VertexName { get; set; }
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
    public struct GraphStructure
    {
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
        public GraphStructure GetGraph()
        {
            GraphStructure gp = new GraphStructure();
            try
            {
                using (reader = new StreamReader($"{Environment.CurrentDirectory}\\graph.gph"))
                {
                    //getting the header
                    var splitedLine = reader.ReadLine().Split(fileSplitter);
                    GraphHeader header = new GraphHeader();
                    header.ChangeGraphname(splitedLine[0]);
                    for (int i = 1; i < splitedLine.Length; i++)
                        header.AddVertex(splitedLine[i]);
                    gp.Header = header;
                    //getting the records
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var splitL = line.Split(fileSplitter);
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
                using (reader = new StreamReader($"{Environment.CurrentDirectory}\\graph.gph"))
                {
                    //getting the header
                    var splitedLine = (await reader.ReadLineAsync()).Split(fileSplitter);
                    GraphHeader header = new GraphHeader();
                    header.ChangeGraphname(splitedLine[0]);
                    for (int i = 1; i < splitedLine.Length; i++)
                        header.AddVertex(splitedLine[i]);
                    gp.Header = header;
                    //getting the records
                    string line = string.Empty;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var splitL = line.Split(fileSplitter);
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
                using (writer = new StreamWriter($"{Environment.CurrentDirectory}\\graph.gph"))
                {
                    //setting the header
                    StringBuilder firstLine = new StringBuilder(graph.Header.GraphName);
                    foreach (var item in graph.Header.Vertices)
                        firstLine.Append($"{fileSplitter}{item}");
                    writer.WriteLine(firstLine.ToString());
                    //setting the records
                    foreach (var record in graph.Records)
                    {
                        StringBuilder line = new StringBuilder(record.VertexName);
                        foreach (var edge in record.Edges)
                        {
                            line.Append($"{fileSplitter}{edge}");
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
                using (writer = new StreamWriter($"{Environment.CurrentDirectory}\\graph.gph"))
                {
                    //setting the header
                    StringBuilder firstLine = new StringBuilder(graph.Header.GraphName);
                    foreach (var item in graph.Header.Vertices)
                        firstLine.Append($"{fileSplitter}{item}");
                    await writer.WriteLineAsync(firstLine.ToString());
                    //setting the records
                    foreach (var record in graph.Records)
                    {
                        StringBuilder line = new StringBuilder(record.VertexName);
                        foreach (var edge in record.Edges)
                        {
                            line.Append($"{fileSplitter}{edge}");
                        }
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not save the graph [graph.gph]");
            }
        }

        public void Dispose()
        {
            reader.Dispose();
            writer.Dispose();
        }
    }
}
