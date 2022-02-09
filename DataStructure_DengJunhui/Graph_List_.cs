using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    /// <summary>
    /// Represents a digraph, implemented with a adjacency list.
    /// </summary>
    /// <typeparam name="Tvertex"></typeparam>
    /// <typeparam name="Tedge"></typeparam>
    public class Graph_List_<Tvertex, Tedge> : Graph_<Tvertex, Tedge>
    {
        protected Vector_<Vertex<Tvertex>> Vertices;
        protected Vector_<List_<Edge_for_List<Tvertex, Tedge>>> Edges_Lists;

        public Graph_List_()
        {
            n_vertex = 0;
            n_edge = 0;
            Vertices = new();
            Edges_Lists = new();
        }


        public override void BCC(int index)
        {
            throw new NotImplementedException();
        }

        public override void BFS(int index)
        {
            throw new NotImplementedException();
        }

        public override void DFS(int index)
        {
            throw new NotImplementedException();
        }

        public override void Dijkstra(int index)
        {
            throw new NotImplementedException();
        }

        public override ref int DTime(int index) =>
            ref Vertices[index].dTime;

        /// <summary>
        /// Check if the edge Exists before you call this.
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public override ref Tedge? Edge(int from_index, int to_index)
        {
            ListNode<Edge_for_List<Tvertex, Tedge>> currentEdge = Edges_Lists[from_index].First;
            for (int i = 0; i < Edges_Lists[from_index].Size; i++)
                if (currentEdge.Data.toVertex == Vertices[to_index])
                    return ref currentEdge.Data.data;
                else
                    currentEdge = currentEdge.Succ;
            throw new InvalidOperationException("The requested edge does not exist!");
        }



        public override bool Exists(int from_index, int to_index) {
            if (from_index < 0 || from_index >= n_vertex || to_index < 0 || to_index >= n_vertex)
                return false;
            ListNode<Edge_for_List<Tvertex, Tedge>> currentEdge = Edges_Lists[from_index].First;
            for (int i = 0; i < Edges_Lists[from_index].Size; i++)
                    if (currentEdge.Data.toVertex == Vertices[to_index])
                        return true;
                    else
                        currentEdge = currentEdge.Succ;
            return false;
        }
            

        public override int FirstNbr(int index)
        {
            throw new NotImplementedException();
        }

        public override ref int FTime(int index) =>
            ref Vertices[index].fTime;

        public override int InDegree(int index) =>
            Vertices[index].inDegree;

        /// <summary>
        /// Insert a vertex to the graph.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override int Insert(in Tvertex? data)
        {
            Edges_Lists.Insert(new List_<Edge_for_List<Tvertex, Tedge>>());
            n_vertex++;
            return Vertices.Insert(new Vertex<Tvertex>(data)); ;
        }

        /// <summary>
        /// Insert an edge from one vertex to another. If the edge existed, it will be overridden.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="from_index"></param>
        /// <param name="to_index"></param>
        /// <param name="weight"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Insert(Tedge? data, int from_index, int to_index, double weight = 1)
        {
            Remove(from_index, to_index);
            Edges_Lists[from_index].InsertAsLast(new Edge_for_List<Tvertex, Tedge>(Vertices[to_index], data, weight));
            n_edge++;
            Vertices[to_index].inDegree++;
            Vertices[from_index].outDegree++;
        }

        public override int NextNbr(int index1, int index2)
        {
            throw new NotImplementedException();
        }

        public override int OutDegree(int index) =>
            Vertices[index].outDegree;

        public override ref int Parent(int index)=>
            ref Vertices[index].parent;

        public override void PFS(int index, Type type)
        {
            throw new NotImplementedException();
        }

        public override void Prim(int index)
        {
            throw new NotImplementedException();
        }

        public override ref int Priority(int index) =>
            ref Vertices[index].priority;

        public override Tvertex? Remove(int index)
        {
            if (index >= n_vertex)
                throw new IndexOutOfRangeException();
            ListNode<Edge_for_List<Tvertex, Tedge>> currentEdge = Edges_Lists[index].First;
            for (int j = 0; j < Edges_Lists[index].Size; j++)
            {
                currentEdge.Data.toVertex.inDegree--;
                currentEdge = currentEdge.Succ;
            }
            n_edge -= Edges_Lists[index].Size;
            Edges_Lists.Remove(index); // Edges from "index"
            n_vertex--;
            for (int from_index = 0; from_index < n_vertex; from_index++) //all edges to "index"
            {
                currentEdge = Edges_Lists[from_index].First;
                for (int j = 0; j < Edges_Lists[from_index].Size; j++)
                    if (currentEdge.Data.toVertex == Vertices[index])
                    {
                        Edges_Lists[from_index].Remove(currentEdge);
                        n_edge--;
                        Vertices[from_index].outDegree--;
                    }
                    else
                        currentEdge = currentEdge.Succ;
            }
            return Vertices.Remove(index).data;
        }

        public override Tedge? Remove(int from_index, int to_index)
        {
            ListNode<Edge_for_List<Tvertex, Tedge>> currentEdge = Edges_Lists[from_index].First;
            for (int i = 0; i < Edges_Lists[from_index].Size; i++)
                if (currentEdge.Data.toVertex == Vertices[to_index])
                {
                    return Edges_Lists[from_index].Remove(currentEdge).data;
                }
                else
                    currentEdge = currentEdge.Succ;
            return default;
        }

        public override ref Vertex_Status Status(int index) =>
            ref Vertices[index].status;

        public override ref Edge_Status Status(int from_index, int to_index)
        {
            ListNode<Edge_for_List<Tvertex, Tedge>> currentEdge = Edges_Lists[from_index].First;
            for (int i = 0; i < Edges_Lists[from_index].Size; i++)
                if (currentEdge.Data.toVertex == Vertices[to_index])
                {
                    return ref currentEdge.Data.status;
                }
                else
                    currentEdge = currentEdge.Succ;
            throw new InvalidOperationException("The requested edge does not exist!");
        }

        public override Stack_<Tvertex> TSort(int index)
        {
            throw new NotImplementedException();
        }

        public override ref Tvertex? Vertex(int index) =>
            ref Vertices[index].data;

        public override ref double Weight(int from_index, int to_index)
        {
            ListNode<Edge_for_List<Tvertex, Tedge>> currentEdge = Edges_Lists[from_index].First;
            for (int i = 0; i < Edges_Lists[from_index].Size; i++)
                if (currentEdge.Data.toVertex == Vertices[to_index])
                {
                    return ref currentEdge.Data.weight;
                }
                else
                    currentEdge = currentEdge.Succ;
            throw new InvalidOperationException("The requested edge does not exist!");
        }

        protected override void BCC(int i, ref int n)
        {
            throw new NotImplementedException();
        }

        protected override void BFS(int i, ref int n)
        {
            throw new NotImplementedException();
        }

        protected override void DFS(int i, ref int n)
        {
            throw new NotImplementedException();
        }

        protected override bool TSort(int i, ref int n, Stack_<Tvertex> vertexStack)
        {
            throw new NotImplementedException();
        }
    }

    public class Edge_for_List<Tvertex, Tedge>
    {
        public Tedge? data;
        public double weight;
        public Edge_Status status;
        public Vertex<Tvertex> toVertex;

        public Edge_for_List(Vertex<Tvertex> toVertex, Tedge? data = default, double weight = 1)
        {
            this.toVertex = toVertex;
            this.data = data;
            this.weight = weight;
            status = Edge_Status.UNDETERMINED;
        }
    }
}
