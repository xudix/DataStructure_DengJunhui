using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public enum Vertex_Status { UNDISCOVERED, DISCOVERED, VISITED};
    public enum Edge_Status { UNDETERMINED, TREE, CROSS, FORWARD, BACKWARD};
    public abstract class Graph_ <Tvertex, Tedge>
    {
        protected virtual void Reset()
        {
            for(int i = 0; i < n_vertex; i++)
            {
                Status(i) = Vertex_Status.UNDISCOVERED;
                DTime(i) = (FTime(i) = -1);
                Parent(i) = -1;
                Priority(i) = int.MaxValue;
                for (int j = 0; j < n_vertex; j++)
                    if (Exists(i, j))
                        Status(i, j) = Edge_Status.UNDETERMINED;
            }
        }
        

        protected int n_vertex, n_edge;

        /// <summary>
        /// Breath-First Search starting from index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="clock"></param>
        protected virtual void BFS(int index, ref int clock)
        {
            Queue_<int> vertices = new();
            vertices.Enqueue(index);
            Status(index) = Vertex_Status.DISCOVERED;
            while (!vertices.Empty)
            {
                int i = vertices.Dequeue();
                DTime(i) = ++clock;
                for(int j = this.FirstNbr(i); j >= 0; j = NextNbr(i, j)) // FirstNbr and NextNbr should guarentee that edge (i,j) exists.
                {
                    if(Status(j) == Vertex_Status.UNDISCOVERED)
                    {
                        vertices.Enqueue(j);
                        Status(j) = Vertex_Status.DISCOVERED;
                        Status(i, j) = Edge_Status.TREE;
                        Parent(j) = i;
                    }
                    else
                        Status(i, j) = Edge_Status.CROSS;
                }
                Status(i) = Vertex_Status.VISITED;
            }
        }
        protected virtual void DFS(int index, ref int clock)
        {
            Status(index) = Vertex_Status.DISCOVERED;
            DTime(index) = ++clock;
            for (int j = this.FirstNbr(index); j >= 0; j = NextNbr(index, j))
            {
                switch (Status(j))
                {
                    case Vertex_Status.UNDISCOVERED:
                        Status(j) = Vertex_Status.DISCOVERED;
                        Status(index, j) = Edge_Status.TREE;
                        Parent(j) = index;
                        DFS(j, ref clock);
                        break;
                    case Vertex_Status.DISCOVERED:
                        Status(index, j) = Edge_Status.BACKWARD;
                        break;
                    

                }
                
            }
            Status(index) = Vertex_Status.VISITED;
        }

        protected virtual void DFS_Itr(int index, ref int clock)
        {
            Stack_<int> vertices = new();
            vertices.Push(index);
            Status(index) = Vertex_Status.DISCOVERED;
            while (!vertices.Empty)
            {
                int i = vertices.Pop();
                DTime(i) = ++clock;
                for (int j = this.FirstNbr(i); j >= 0; j = NextNbr(i, j)) // FirstNbr and NextNbr should guarentee that edge (i,j) exists.
                {
                    switch (Status(j))
                    {
                        case Vertex_Status.UNDISCOVERED:
                            vertices.Push(j);
                            Status(j) = Vertex_Status.DISCOVERED;
                            Status(i, j) = Edge_Status.TREE;
                            Parent(j) = i;
                            break;
                        case Vertex_Status.DISCOVERED: //previously discovered but not done with. It is still in the stack but not all child examined.
                            Status(i, j) = Edge_Status.BACKWARD;
                            break;
                        default:
                            Status(i, j) = DTime(i) < DTime(j) ? Edge_Status.FORWARD : Edge_Status.CROSS;
                            break;
                    }
                }
                Status(i) = Vertex_Status.VISITED;
                FTime(i) = ++clock;
            }
        }

        protected abstract void BCC(int i, ref int n);
        protected abstract bool TSort(int i, ref int n, Stack_<Tvertex> vertexStack);
        




        public int N_vertex => n_vertex;
        public int N_edge => n_edge;

        public abstract int Insert(in Tvertex data);
        public abstract Tvertex Remove(int index);

        public abstract ref Tvertex? Vertex(int index);

        public abstract int InDegree(int index);

        public abstract int OutDegree(int index);

        public abstract int FirstNbr(int index);

        public abstract int NextNbr(int index1, int index2); 

        public abstract ref int DTime(int index);
        public abstract ref int FTime(int index);
        public abstract ref int Parent(int index);
        public abstract ref int Priority(int index);

        
        public abstract ref Vertex_Status Status (int index);

        public abstract bool Exists(int from_inded, int to_index);

        public abstract void Insert(Tedge data, int from_index, int to_index, double weight);

        public abstract Tedge Remove(int from_inded, int to_index);

        public abstract ref Edge_Status Status(int from_inded, int to_index);

        public abstract ref Tedge? Edge(int from_index, int to_index);

        public abstract ref double Weight(int from_index, int to_index);

        /// <summary>
        /// Breath-First Search
        /// </summary>
        /// <param name="index"></param>
        public virtual void BFS(int index)
        {
            Reset();
            int clock = 0;
            int i = index;
            do
            {
                if (Status(i) == Vertex_Status.UNDISCOVERED)
                    BFS(i, ref clock);
            } while ((i = (++i % n_vertex)) != index);
        }

        /// <summary>
        /// Depth-First Search
        /// </summary>
        /// <param name="index"></param>
        public abstract void DFS(int index);


        public abstract void BCC(int index);

        public abstract Stack_<Tvertex> TSort(int index);

        public abstract void Prim(int index);

        public abstract void Dijkstra(int index);

        public abstract void PFS(int index, Type type);


    }
}
