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
        protected abstract void BFS(int i, ref int n);
        protected abstract void DFS(int i, ref int n);
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

        public abstract int NextNbf(int index1, int index2); 

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

        public abstract void BFS(int index);

        public abstract void DFS(int index);


        public abstract void BCC(int index);

        public abstract Stack_<Tvertex> TSort(int index);

        public abstract void Prim(int index);

        public abstract void Dijkstra(int index);

        public abstract void PFS(int index, Type type);


    }
}
