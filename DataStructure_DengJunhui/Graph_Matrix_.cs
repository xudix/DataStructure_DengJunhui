using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ

{
    /// <summary>
    /// Represents a digraph, implemented with a adjacency matrix.
    /// </summary>
    public class Graph_Matrix_<Tvertex, Tedge> : Graph_<Tvertex, Tedge>
    {

        protected Vector_<Vertex<Tvertex>> Vertices;
        protected Vector_<Vector_<Edge<Tedge>>> Edges_Matrix;

        public Graph_Matrix_()
        {
            n_vertex = 0;
            n_edge = 0;
            Vertices = new();
            Edges_Matrix = new();
        }


        public override void BCC(int index)
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
        public override ref Tedge? Edge(int index1, int index2)=>
            ref Edges_Matrix[index1][index2].data;


        public override bool Exists(int from_index, int index2) =>
            from_index >=0 && from_index < n_vertex && index2 >=0 && index2 < n_vertex &&!(Edges_Matrix[from_index][index2] == null);

        public override int FirstNbr(int from_index) =>
            NextNbr(from_index, Edges_Matrix[from_index].Size);

        public override ref int FTime(int index)=>
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
            for (int i = 0;i<n_vertex;i++)
                Edges_Matrix[i].Insert(null);
            n_vertex++;
            Edges_Matrix.Insert(new Vector_<Edge<Tedge>>(Edges_Matrix.Capacity, n_vertex));
            return Vertices.Insert(new Vertex<Tvertex>(data)); ;
        }

        /// <summary>
        /// Insert an edge from index1 to index2. If the edge existed, it will be overridden.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="from_index"></param>
        /// <param name="to_index"></param>
        /// <param name="weight"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Insert(Tedge? data, int from_index, int to_index, double weight = 1)
        {
            if(!Exists(from_index, to_index))
            {
                n_edge++;
                Vertices[to_index].inDegree++;
                Vertices[from_index].outDegree++;
            }
            Edges_Matrix[from_index][to_index] = new Edge<Tedge>(data, weight);
        }

        public override int NextNbr(int from_index, int to_index)
        {
            int nextNbr = to_index;
            while (--nextNbr >= 0 && !Exists(from_index, nextNbr)) ;
            return nextNbr;
        }

        public override int OutDegree(int index) =>
            Vertices[index].outDegree;

        public override ref int Parent(int index) =>
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
            for (int i = 0; i < n_vertex; i++)
                if (Exists(index, i))//edges from "index" to others
                {
                    n_edge--;
                    Vertices[i].inDegree--;
                }
            Edges_Matrix.Remove(index);
            n_vertex--;
            for (int i=0; i<n_vertex; i++) //all edges to "index"
            {
                if (Exists(i, index))
                {
                    n_edge--;
                    Vertices[i].outDegree--;
                }
                Edges_Matrix[i].Remove(index);
            }
            return Vertices.Remove(index).data;
        }

        public override Tedge? Remove(int from_index, int to_index)
        {
            if (Exists(from_index, to_index))
            {
                Tedge? data = Edge(from_index,to_index);
                Edges_Matrix[from_index][to_index] = default;
                n_edge--;
                Vertices[to_index].inDegree--;
                Vertices[from_index].outDegree--;
                return data;
            }
            else
                return default(Tedge);
        }

        public override ref Vertex_Status Status(int index)=>
            ref Vertices[index].status;

        public override ref Edge_Status Status(int from_index, int to_index)=>
            ref Edges_Matrix[from_index][to_index].status;

        public override Stack_<Tvertex> TSort(int index)
        {
            Stack_<Tvertex>? sorted = new();
            int clock = 0, i = index;
            Reset();
            do
            {
                if(Status(i) == Vertex_Status.UNDISCOVERED)
                    if (!TSort(i, ref clock, sorted))
                        throw new InvalidOperationException("The graph cannot be torpologically sorted.");
            } while ((i = (i + 1) % n_vertex) != index);
            return sorted;
        }

        public override ref Tvertex? Vertex(int index) =>
            ref Vertices[index].data;

        public override ref double Weight(int from_index, int to_index) =>
            ref Edges_Matrix[from_index][to_index].weight;

        protected override void BCC(int i, ref int n)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Perform a torpological sort of the graph starting from vertex i.
        /// </summary>
        /// <param name="i">Starting vertex</param>
        /// <param name="clock"></param>
        /// <param name="vertexStack">The stack to show the result of the torpological sort</param>
        /// <returns>True if the sort is completed sucessfully. False if the graph has a loop.</returns>
        protected override bool TSort(int i, ref int clock, Stack_<Tvertex> vertexStack)
        {
            Status(i) = Vertex_Status.DISCOVERED;
            DTime(i) = ++clock;
            Stack_<int> tempStack = new();
            tempStack.Push(i);
            while (!tempStack.Empty)
            {
                i = tempStack.Pop();
                int j = FirstNbr(i);
                while (j >= 0)
                {
                    if (Status(i, j) == Edge_Status.UNDETERMINED)
                        switch (Status(j))
                        {
                            case Vertex_Status.UNDISCOVERED:
                                Status(j) = Vertex_Status.DISCOVERED;
                                DTime(j) = ++clock;
                                tempStack.Push(i);
                                Status(i, j) = Edge_Status.TREE;
                                i = j;
                                j = FirstNbr(i);
                                continue;
                            case Vertex_Status.DISCOVERED: // Backward edge.
                                Status(i, j) = Edge_Status.BACKWARD;
                                return false;
                            default:
                                Status(i, j) = DTime(i) < DTime(j) ? Edge_Status.FORWARD : Edge_Status.CROSS;
                                break;
                        }
                    j = NextNbr(i, j);
                }
                Status(i) = Vertex_Status.VISITED;
                vertexStack.Push(Vertex(i));
                FTime(i) = ++clock;
//                tempStack.Pop();
            }
            return true;
        }
    }

    public class Vertex<T>
    {
        public T? data;
        public int inDegree, outDegree;
        public Vertex_Status status;
        public int dTime, fTime;
        public int parent;
        public int priority;
        public Vertex(in T? data = default)
        {
            this.data = data;
            inDegree = 0;
            outDegree = 0;
            status = Vertex_Status.UNDISCOVERED;
            dTime = -1;
            fTime = -1;
            parent = -1;
            priority = int.MaxValue;
        }
    }

    public class Edge<T>
    {
        public T? data;
        public double weight;
        public Edge_Status status;

        public Edge(T? data = default, double weight = 1)
        {
            this.data = data;
            this.weight = weight;
            status = Edge_Status.UNDETERMINED;
        }
    }
}
