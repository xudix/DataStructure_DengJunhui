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


        public override object BCC(int index)
        {
            Vector_<Stack_<Tvertex>> result = new();
            int i = index, clock = 0;
            do
            {
                if(Vertices[i].status == Vertex_Status.UNDISCOVERED)
                    BCC_Itr(i, ref clock, result);
            }while((i = (i+1) % Vertices.Size)!=index);
            result.Remove(result.Size - 1);
            return result;
        }


        public override void Dijkstra(int index) =>
            PFS(index, Dijkstra_PU);


        /// <summary>
        /// Priority updater for Dijkstra algorithm. Priority of a vertex is set to its distance from tree root.
        /// </summary>
        /// <param name="vertex">Current vertex. Verticed with edge from the current vertex will be updated.</param>
        public void Dijkstra_PU(Vertex_for_List<Tvertex, Tedge> vertex)
        {
            for(var edge = vertex.edgesList.First; edge.Data != null; edge = edge.Succ)
                if(edge.Data.toVertex.status == Vertex_Status.UNDISCOVERED && edge.Data.toVertex.priority > vertex.priority + edge.Data.weight)
                {
                    edge.Data.toVertex.priority = vertex.priority+edge.Data.weight;
                    edge.Data.toVertex.parent = vertex;
                }
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

        public Vertex_for_List<Tvertex, Tedge>? Parent(Vertex_for_List<Tvertex, Tedge> child) =>
            child.parent;

        protected override void PFS(int index, ref int clock, Action<Graph_<Tvertex, Tedge>, int, int> priorityUpdater)
        {
            throw new NotImplementedException();
        }

        public void PFS(int index, Action<Vertex_for_List<Tvertex, Tedge>> priorityUpdater)
        {
            Reset();
            int clock = 0;
            int i = index;
            do
            {
                if (Status(i) == Vertex_Status.UNDISCOVERED)
                    PFS(i, ref clock, priorityUpdater);
            } while ((i = (++i % n_vertex)) != index);
        }

        /// <summary>
        /// Priority first search.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="clock"></param>
        /// <param name="priorityUpdater">Delegate of method or action used to update the priority of a vertex. 
        /// The parameter is the current vertex. 
        /// This method should update the priority of the children of the current vertex, and update their "parent" as well.</param>
        protected void PFS(int index, ref int clock, Action<Vertex_for_List<Tvertex, Tedge>> priorityUpdater)
        {
            Vertex_for_List<Tvertex, Tedge> vertex = Vertices[index];
            vertex.priority = 0;
            while(true)
            {
                vertex.status = Vertex_Status.VISITED;
                priorityUpdater(vertex);
                double minPrior = double.MaxValue;
                for (int current = 0; current < n_vertex; current++) // find the undiscovered vertices with smallest priority
                {
                    if (Status(current) == Vertex_Status.UNDISCOVERED && Priority(current) < minPrior)
                    {
                        vertex = Vertices[current];
                        minPrior = vertex.priority;
                    }
                }
                if (vertex.status != Vertex_Status.VISITED) // found next vertex. update edge status to "TREE"
                {
                    for (var edge = vertex.parent.edgesList.First; edge.Data != null; edge = edge.Succ)
                        if (edge.Data.toVertex == vertex)
                        {
                            edge.Data.status = Edge_Status.TREE;
                            break;
                        }
                }
                else
                    break;// The loop ends when there's no next vertex
            }
        }


        public override void Prim(int index) =>
            PFS(index, Prim_PU);
        

        public void Prim_PU(Vertex_for_List<Tvertex, Tedge> vertex)
        {
            for(var edge = vertex.edgesList.First; edge.Data != null; edge = edge.Succ)
            {
                if(edge.Data.toVertex.status == Vertex_Status.UNDISCOVERED && edge.Data.toVertex.priority > edge.Data.weight)
                {
                    edge.Data.toVertex.priority = edge.Data.weight;
                    edge.Data.toVertex.parent = vertex;
                }
                    
            }
        }

        public override ref double Priority(int index) =>
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


        protected override void BCC(int index, ref int clock, Vector_<Stack_<Tvertex>> result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bi-connected component decomposition.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="clock"></param>
        protected void BCC_Itr(int index, ref int clock, Vector_<Stack_<Tvertex>> result)
        {
            // The idea is to find any loop in the graph by backward edges.
            // Class Vertex field "fTime" will be used to record the "highest connected ancestor"

            Vertex_for_List<Tvertex, Tedge>? vertex = Vertices[index];
            vertex.fTime = (vertex.dTime = ++clock);
            vertex.status = Vertex_Status.DISCOVERED;
            result.Insert(new Stack_<Tvertex>());
            int currentBCC = result.Size-1;
            Stack_<Vertex_for_List<Tvertex, Tedge>> vertexItrStack = new(), tempResultStack = new();
            // vertexItrStack tracks where the current and next vertices to visit; tempResultStack tracks all vertices that is not put into result yet.
            Stack_<Edge_for_List<Tvertex,Tedge>> edgeStack = new();
            vertexItrStack.Push(vertex);
            tempResultStack.Push(vertex);

            while (!vertexItrStack.Empty)
            {
                vertex = vertexItrStack.Top;
                ListNode<Edge_for_List<Tvertex, Tedge>>? edge = vertex.edgesList.First;
                while (edge.Data != null)
                {
                    if (edge.Data.status == Edge_Status.UNDETERMINED)
                        switch (edge.Data.toVertex.status)
                        {
                            case Vertex_Status.UNDISCOVERED: // new node discovered
                                edge.Data.toVertex.parent = vertex;
                                vertex = edge.Data.toVertex;
                                vertexItrStack.Push(vertex);
                                tempResultStack.Push(vertex);
                                edgeStack.Push(edge.Data);
                                vertex.status = Vertex_Status.DISCOVERED;
                                vertex.fTime = (vertex.dTime = ++clock);
                                edge.Data.status = Edge_Status.TREE;
                                edge = vertex.edgesList.First;
                                continue;
                            case Vertex_Status.DISCOVERED:
                                edge.Data.status = Edge_Status.BACKWARD;
                                if (edge.Data.toVertex != vertex.parent) // Backward edge
                                {
                                    if (vertex.fTime > edge.Data.toVertex.dTime)
                                        vertex.fTime = edge.Data.toVertex.dTime;
                                }
                                break;
                            default: // Vertex_Satus.Visited. For digraph.
                                edge.Data.status = (edge.Data.toVertex.dTime > vertex.dTime) ? Edge_Status.FORWARD : Edge_Status.CROSS;
                                break;
                        }
                    edge = edge.Succ;
                }
                vertex.status = Vertex_Status.VISITED;
                vertexItrStack.Pop();
                if(!vertexItrStack.Empty)
                    if (vertex.fTime < vertexItrStack.Top.fTime) // The current vertex is connected to a higher ancestor
                    {
                        vertexItrStack.Top.fTime = vertex.fTime;
                    }
                    else // The current vertex is not connected to a higher ancestor. The bbc ends here. Stack top is a cut vertex
                    {
                        while(tempResultStack.Top.parent!=vertexItrStack.Top) // put all vertices before the cut vertex into the 
                            result[currentBCC].Push(tempResultStack.Pop().data);
                        result[currentBCC].Push(tempResultStack.Pop().data);
                        result[currentBCC].Push(vertexItrStack.Top.data);
                        currentBCC++;
                        result.Insert(new());
                    }
            }
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

        /// <summary>
        /// Priority updater for Priority-First Search PFS() to use Breath-First Search
        /// </summary>
        /// <param name="vertex"></param>
        public void BFS_PU(Vertex_for_List<Tvertex, Tedge> vertex)
        {
            for(var edge = vertex.edgesList.First; edge.Data !=null; edge = edge.Succ )
                if (edge.Data.toVertex.status == Vertex_Status.UNDISCOVERED)
                    if(edge.Data.toVertex.priority > vertex.priority + 1)
                    {
                        edge.Data.toVertex.priority = vertex.priority + 1;
                        edge.Data.toVertex.parent = vertex;
                    }
        }

        /// <summary>
        /// Priority updater for Priority-First Search PFS() to use Depth-First Search
        /// </summary>
        /// <param name="vertex"></param>
        public void DFS_PU(Vertex_for_List<Tvertex, Tedge> vertex)
        {
            for (var edge = vertex.edgesList.First; edge.Data != null; edge = edge.Succ)
                if (edge.Data.toVertex.status == Vertex_Status.UNDISCOVERED)
                    if (edge.Data.toVertex.priority > vertex.priority - 1)
                    {
                        edge.Data.toVertex.priority = vertex.priority - 1;
                        edge.Data.toVertex.parent = vertex;
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

        public override string ToString()
        {
            return data.ToString();
        }
    }

    public class Vertex_for_List<Tvertex, Tedge>
    {
        public Tvertex? data;
        public int inDegree, outDegree;
        public Vertex_Status status;
        public int dTime, fTime;
        public Vertex_for_List<Tvertex, Tedge>? parent;
        public double priority;
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

        public override string ToString()
        {
            return data.ToString();
        }
    }
}
