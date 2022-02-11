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
        protected Vector_<Vertex_for_List<Tvertex, Tedge>> Vertices;
        //protected Vector_<List_<Edge_for_List<Tvertex, Tedge>>> Edges_Lists;

        public Graph_List_()
        {
            n_vertex = 0;
            n_edge = 0;
            Vertices = new();
            //Edges_Lists = new();
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
        /// Find the edge data between two vertices. Check if the edge Exists before you call this.
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>The data associated with the edge.</returns>
        public override ref Tedge? Edge(int from_index, int to_index)
        {
            Edge_for_List<Tvertex, Tedge>? edge = Edge_Ref(from_index, to_index);
            if (edge != null)
                return ref edge.data;
            else
                throw new InvalidOperationException("The requested edge does not exist!");
        }

        /// <summary>
        /// Find the edge between two vertices. Check if the edge Exists before you call this.
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>The reference to the edge.</returns>
        public Edge_for_List<Tvertex, Tedge>? Edge_Ref(int from_index, int to_index)
        {
            ListNode<Edge_for_List<Tvertex, Tedge>> edge;
            for (edge = Vertices[from_index].edgesList.First; edge.Data != null; edge = edge.Succ)
                if (edge.Data.toVertex == Vertices[to_index])
                    break;
            return edge.Data;
        }





        public override bool Exists(int from_index, int to_index) {
            if (from_index < 0 || from_index >= n_vertex || to_index < 0 || to_index >= n_vertex)
                return false;
            ListNode<Edge_for_List<Tvertex, Tedge>> edge;
            for (edge = Vertices[from_index].edgesList.First; edge.Data != null; edge = edge.Succ)
                if (edge.Data.toVertex == Vertices[to_index])
                    return true;
            return false;
        }

        /// <summary>
        /// Find the index of the given vertex in the vector of vertices.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns>The index. -1 if the vertex is not found.</returns>
        public int FindIndex(Vertex_for_List<Tvertex, Tedge> vertex)
        {
            int index;
            for (index = Vertices.Size - 1; index >= 0; index--)
                if (Vertices[index] == vertex)
                    break;
            return index;
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
            //Edges_Lists.Insert(new List_<Edge_for_List<Tvertex, Tedge>>());
            n_vertex++;
            return Vertices.Insert(new Vertex_for_List<Tvertex, Tedge>(data)); ;
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
            Vertices[from_index].edgesList.InsertAsLast(new Edge_for_List<Tvertex, Tedge>(Vertices[to_index], data, weight));
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

        public override ref int Parent(int index) 
        { 
            throw new NotImplementedException();
        }

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
            ListNode<Edge_for_List<Tvertex, Tedge>> edge;
            for (edge = Vertices[index].edgesList.First; edge.Data != null; edge = edge.Succ)
                edge.Data.toVertex.inDegree--;
            n_edge -= Vertices[index].edgesList.Size;
            n_vertex--;
            for (int from_index = 0; from_index < n_vertex; from_index++) //all edges to "index"
            {
                for (edge = Vertices[from_index].edgesList.First; edge.Data != null; edge = edge.Succ)
                    if (edge.Data.toVertex == Vertices[index])
                    {
                        Vertices[from_index].edgesList.Remove(edge);
                        n_edge--;
                        Vertices[from_index].outDegree--;
                    }
            }
            return Vertices.Remove(index).data;
        }

        public override Tedge? Remove(int from_index, int to_index)
        {
            ListNode<Edge_for_List<Tvertex, Tedge>> edge;
            for (edge = Vertices[from_index].edgesList.First; edge.Data != null; edge = edge.Succ)
                if (edge.Data.toVertex == Vertices[to_index])
                {
                    Vertices[from_index].outDegree--;
                    Vertices[to_index].inDegree--;
                    n_edge--;
                    return Vertices[from_index].edgesList.Remove(edge).data;
                }
            return default;
        }

        public override ref Vertex_Status Status(int index) =>
            ref Vertices[index].status;

        public override ref Edge_Status Status(int from_index, int to_index)=>
            ref Edge_Ref(from_index, to_index).status;

        public override Stack_<Tvertex> TSort(int index)
        {
            Reset();
            int clock = 0, i = index;
            Stack_<Tvertex> sorted = new();
            do
            {

                if (Vertices[i].status == Vertex_Status.UNDISCOVERED)
                    if (!TSort(i, ref clock, sorted))
                        throw new InvalidOperationException("This graph cannot be torpologicalli sorted.");
            }while((i=(i+1)%n_vertex)!=index);
            return sorted;
        }

        public override ref Tvertex? Vertex(int index) =>
            ref Vertices[index].data;

        public override ref double Weight(int from_index, int to_index) =>
            ref Edge_Ref(from_index, to_index).weight;

        protected override void BCC(int i, ref int n)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Breath-First Search starting from index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="clock"></param>
        protected override void BFS(int index, ref int clock)
        {
            Queue_<Vertex_for_List<Tvertex, Tedge>> VertexQueue = new();
            VertexQueue.Enqueue(Vertices[index]);
            Status(index) = Vertex_Status.DISCOVERED;
            while (!VertexQueue.Empty)
            {
                Vertex_for_List<Tvertex, Tedge> currentVertex = VertexQueue.Dequeue();
                currentVertex.dTime = ++clock;
                ListNode<Edge_for_List<Tvertex, Tedge>>? currentEdge = currentVertex.edgesList.First;
                while (currentEdge.Data != null) // scan through all edges
                {
                    if (currentEdge.Data.toVertex.status == Vertex_Status.UNDISCOVERED)
                    {
                        VertexQueue.Enqueue(currentEdge.Data.toVertex);
                        currentEdge.Data.toVertex.status = Vertex_Status.DISCOVERED;
                        currentEdge.Data.toVertex.parent = currentVertex;
                        currentEdge.Data.status = Edge_Status.TREE;
                    }
                    else
                        currentEdge.Data.status = Edge_Status.CROSS;
                    currentEdge = currentEdge.Succ;
                }
                currentVertex.status = Vertex_Status.VISITED;
            }
        }

        /// <summary>
        /// Depth-First Search
        /// </summary>
        /// <param name="index"></param>
        public override void DFS(int index)
        {
            Reset();
            int clock = 0;
            int i = index;
            do
            {
                if (Status(i) == Vertex_Status.UNDISCOVERED)
                    DFS_Itr(Vertices[i], ref clock);
            } while ((i = (++i % n_vertex)) != index);
        }


        /// <summary>
        /// Depth-First Search implemented with recursion.
        /// </summary>
        /// <param name="fromVertex"></param>
        /// <param name="clock"></param>
        protected void DFS(Vertex_for_List<Tvertex, Tedge> fromVertex, ref int clock)
        {
            fromVertex.status = Vertex_Status.DISCOVERED;
            fromVertex.dTime = ++clock;
            for (ListNode<Edge_for_List<Tvertex, Tedge>>? currentEdge = fromVertex.edgesList.First; currentEdge.Data != null; currentEdge = currentEdge.Succ)
            {
                switch (currentEdge.Data.toVertex.status)
                {
                    case Vertex_Status.UNDISCOVERED:
                        currentEdge.Data.toVertex.status = Vertex_Status.DISCOVERED;
                        currentEdge.Data.status= Edge_Status.TREE;
                        currentEdge.Data.toVertex.parent = fromVertex;
                        DFS(currentEdge.Data.toVertex, ref clock);
                        break;
                    case Vertex_Status.DISCOVERED:
                        currentEdge.Data.status = Edge_Status.BACKWARD;
                        break;
                    default: // Status is VISITED.
                        currentEdge.Data.status = fromVertex.dTime < currentEdge.Data.toVertex.dTime ? Edge_Status.FORWARD : Edge_Status.CROSS;
                        break;
                }
            }
            fromVertex.status = Vertex_Status.VISITED;
            fromVertex.fTime = ++clock;
        }

        protected override void DFS_Itr(int index, ref int clock)
        {
            DFS_Itr(Vertices[index], ref clock);
        }

        /// <summary>
        /// Depth-First Search implemented using iteration.
        /// </summary>
        /// <param name="fromVertex"></param>
        /// <param name="clock"></param>
        protected void DFS_Itr(Vertex_for_List<Tvertex, Tedge> fromVertex, ref int clock)
        {
            Stack_<Vertex_for_List<Tvertex, Tedge>> vertexStack = new();
            vertexStack.Push(fromVertex);
            fromVertex.status = Vertex_Status.DISCOVERED;
            fromVertex.dTime = ++clock;
            while (!vertexStack.Empty)
            {
                Vertex_for_List<Tvertex, Tedge>? vertex = vertexStack.Top;
                ListNode<Edge_for_List<Tvertex, Tedge>> edge = vertex.edgesList.First;
                while (edge.Data != null)
                {
                    if(edge.Data.status == Edge_Status.UNDETERMINED)
                        switch (edge.Data.toVertex.status)
                        {
                            case Vertex_Status.UNDISCOVERED:
                                edge.Data.toVertex.parent = vertex;
                                vertex = edge.Data.toVertex;
                                vertexStack.Push(vertex);
                                vertex.status = Vertex_Status.DISCOVERED;
                                vertex.dTime = ++clock;
                                edge.Data.status = Edge_Status.TREE;
                                edge = vertex.edgesList.First;
                                continue;
                            case Vertex_Status.DISCOVERED:
                                edge.Data.status = Edge_Status.BACKWARD;
                                break;
                            default: // Visited
                                edge.Data.status = vertex.dTime < edge.Data.toVertex.dTime? Edge_Status.FORWARD: Edge_Status.CROSS;
                                break;
                        }
                    edge = edge.Succ;
                }
                vertex.fTime = ++clock;
                vertex.status = Vertex_Status.VISITED;
                vertexStack.Pop();
            }
        }

        /// <summary>
        /// Topological sort from "fromIndex".
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="clock"></param>
        /// <param name="vertexStack"></param>
        /// <returns>True if sorted correctly. False if a loop is found in the graph.</returns>
        protected override bool TSort(int fromIndex, ref int clock, Stack_<Tvertex> vertexStack)
        {
            Vertex_for_List<Tvertex, Tedge> current = Vertices[fromIndex];
            current.status = Vertex_Status.DISCOVERED;
            current.dTime = ++clock;
            Stack_<Vertex_for_List<Tvertex, Tedge>> tempStack = new();
            tempStack.Push(current);
            while (!tempStack.Empty)
            {
                current = tempStack.Top;
                var edge = current.edgesList.First;
                while(edge.Data != null)
                {
                    if(edge.Data.status == Edge_Status.UNDETERMINED)
                        switch (edge.Data.toVertex.status)
                        {
                            case Vertex_Status.UNDISCOVERED:
                                current = edge.Data.toVertex;
                                tempStack.Push(current);
                                current.status = Vertex_Status.DISCOVERED;
                                current.dTime = ++clock;
                                edge.Data.status = Edge_Status.TREE;
                                edge = current.edgesList.First;
                                continue;
                            case Vertex_Status.DISCOVERED: // backward edge found
                                edge.Data.status |= Edge_Status.BACKWARD;
                                return false;
                            default:
                                edge.Data.status = current.dTime < edge.Data.toVertex.dTime ? Edge_Status.FORWARD : Edge_Status.CROSS;
                                break;
                        }
                    edge = edge.Succ;
                }
                current.status = Vertex_Status.VISITED;
                current.fTime = ++clock;
                vertexStack.Push(current.data);
                tempStack.Pop();
            }
            return true;
        }

        protected override void Reset()
        {
            for (int i = 0; i < n_vertex; i++)
            {
                Vertices[i].status = Vertex_Status.UNDISCOVERED;
                DTime(i) = (FTime(i) = -1);
                Vertices[i].parent = null;
                Priority(i) = int.MaxValue;
                for (var edge = Vertices[i].edgesList.First; edge.Data != null; edge = edge.Succ )
                    edge.Data.status = Edge_Status.UNDETERMINED;
            }
        }
    }

    public class Edge_for_List<Tvertex, Tedge>
    {
        public Tedge? data;
        public double weight;
        public Edge_Status status;
        public Vertex_for_List<Tvertex, Tedge> toVertex;
        

        public Edge_for_List(Vertex_for_List<Tvertex, Tedge> toVertex, Tedge? data = default, double weight = 1)
        {
            this.toVertex = toVertex;
            this.data = data;
            this.weight = weight;
            status = Edge_Status.UNDETERMINED;
        }
    }

    public class Vertex_for_List<Tvertex, Tedge>
    {
        public Tvertex? data;
        public int inDegree, outDegree;
        public Vertex_Status status;
        public int dTime, fTime;
        public Vertex_for_List<Tvertex, Tedge>? parent;
        public int priority;
        public List_<Edge_for_List<Tvertex, Tedge>> edgesList;
        public Vertex_for_List(in Tvertex? data = default)
        {
            this.data = data;
            inDegree = 0;
            outDegree = 0;
            status = Vertex_Status.UNDISCOVERED;
            dTime = -1;
            fTime = -1;
            parent = default;
            priority = int.MaxValue;
            edgesList = new();
        }
    }
}
