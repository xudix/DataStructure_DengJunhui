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
                for(int j = FirstNbr(i); j >= 0; j = NextNbr(i, j)) // FirstNbr and NextNbr should guarentee that edge (i,j) exists.
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
                    default:
                        Status(index, j) = DTime(index) < DTime(j) ? Edge_Status.FORWARD : Edge_Status.CROSS;
                        break;
                }
            }
            Status(index) = Vertex_Status.VISITED;
            FTime(index) = ++clock;
        }

        protected virtual void DFS_Itr(int index, ref int clock)
        {
            Stack_<int> vertices = new();
            vertices.Push(index);
            Status(index) = Vertex_Status.DISCOVERED;
            while (!vertices.Empty)
            {
                //int i = vertices.Top;
                //int j = FirstNbr(i);
                //do
                //{
                //    if (Status(j) == Vertex_Status.UNDISCOVERED)
                //    {
                //        vertices.Push(j);
                //        Status(j) = Vertex_Status.DISCOVERED;
                //        DTime(j) = ++clock;
                //        Status(i, j) = Edge_Status.TREE;
                //        Parent(j) = i;
                //        break;
                //    }
                //    else if (Status(j) == Vertex_Status.DISCOVERED) //previously discovered but not done with. It is still in the stack but not all child examined.
                //    { Status(i, j) = Edge_Status.BACKWARD; }
                //    else
                //    { Status(i, j) = DTime(i) < DTime(j) ? Edge_Status.FORWARD : Edge_Status.CROSS; }
                //} while ((j = NextNbr(i, j)) >=0);
                //if (j < 0)
                //{
                //    Status(i) = Vertex_Status.VISITED;
                //    vertices.Pop();
                //    FTime(i) = ++clock;
                //}
                int i = vertices.Top;
                int j = FirstNbr(i);
                while (j >= 0) // FirstNbr and NextNbr should guarentee that edge (i,j) exists.
                {
                    if (Status(i, j) == Edge_Status.UNDETERMINED) // this edge has not been checked
                        if (Status(j) == Vertex_Status.UNDISCOVERED)
                        {
                            vertices.Push(j);
                            Status(j) = Vertex_Status.DISCOVERED;
                            DTime(j) = ++clock;
                            Status(i, j) = Edge_Status.TREE;
                            Parent(j) = i;
                            i = vertices.Top;
                            j = FirstNbr(i);
                            continue;
                        }
                        else if (Status(j) == Vertex_Status.DISCOVERED) //previously discovered but not done with. It is still in the stack but not all child examined.
                            Status(i, j) = Edge_Status.BACKWARD;
                        else
                            Status(i, j) = DTime(i) < DTime(j) ? Edge_Status.FORWARD : Edge_Status.CROSS;
                    j = NextNbr(i, j);
                }
                Status(i) = Vertex_Status.VISITED;
                vertices.Pop();
                FTime(i) = ++clock;
            }
        }

        protected abstract void BCC(int i, ref int clock, Vector_<Stack_<Tvertex>> result);
        protected abstract bool TSort(int i, ref int n, Stack_<Tvertex> vertexStack);

        protected abstract void PFS(int index, ref int clock, Action<Graph_<Tvertex, Tedge>, int, int> priorityUpdater);




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
        public virtual void DFS(int index)
        {
            Reset();
            int clock = 0;
            int i = index;
            do
            {
                if (Status(i) == Vertex_Status.UNDISCOVERED)
                    DFS_Itr(i, ref clock);
            } while ((i = (++i % n_vertex)) != index);
        }


        public abstract object BCC(int index);

        public abstract Stack_<Tvertex> TSort(int index);

        public abstract void Prim(int index);

        public abstract void Dijkstra(int index);

        public virtual void PFS(int index, Action<Graph_<Tvertex, Tedge>, int, int> priorityUpdater)
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

        


    }

    public class Graph_Exercises
    {
        public static void Main()
        {
            var graph = new Graph_List_<string, int>();
            //var graph = new ThisProj.Graph_List_<string, int>();
            //// Graph in fig 6.7
            //graph.Insert("A0");
            //graph.Insert("B1");
            //graph.Insert("C2");
            //graph.Insert("D3");
            //graph.Insert("E4");
            //graph.Insert("F5");
            //graph.Insert("G6");
            //graph.Insert("S7");
            //graph.Insert(1, 0, 2, 3);
            //graph.Insert(2, 0, 4, 4);
            //graph.Insert(3, 2, 1, 7);
            //graph.Insert(4, 3, 1, 9);
            //graph.Insert(5, 4, 5, 2);
            //graph.Insert(6, 4, 6, 5);
            //graph.Insert(0, 6, 1, 1);
            //graph.Insert(0, 6, 5, 1);
            //graph.Insert(0, 7, 0, 1);
            //graph.Insert(0, 7, 2, 1);
            //graph.Insert(0, 7, 3, 1);
            //graph.BFS(0);
            //graph.PFS(7, graph.BFS_PU);


            // Graph in fig 6.8
            //graph.Insert("A0");
            //graph.Insert("B1");
            //graph.Insert("C2");
            //graph.Insert("D3");
            //graph.Insert("E4");
            //graph.Insert("F5");
            //graph.Insert("G6");
            //graph.Insert(1, 0, 1, 3);
            //graph.Insert(2, 0, 2, 4);
            //graph.Insert(3, 1, 2, 7);
            //graph.Insert(4, 3, 0, 9);
            //graph.Insert(5, 3, 4, 2);
            //graph.Insert(6, 0, 5, 5);
            //graph.Insert(0, 4, 5, 1);
            //graph.Insert(0, 5, 6, 1);
            //graph.Insert(0, 6, 0, 1);
            //graph.Insert(0, 6, 2, 1);
            //graph.DFS(0);
            //graph.PFS(0, graph.DFS_PU);

            //// Graph in fig 6.12
            //graph.Insert("A0");
            //graph.Insert("B1");
            //graph.Insert("C2");
            //graph.Insert("D3");
            //graph.Insert("E4");
            //graph.Insert("F5");
            //graph.Insert(1, 0, 2);
            //graph.Insert(1, 0, 3);
            //graph.Insert(1, 1, 2);
            //graph.Insert(1, 2, 3);
            //graph.Insert(1, 2, 4);
            //graph.Insert(1, 2, 5);
            //graph.Insert(1, 4, 5);
            //Stack_<string> sorted = graph.TSort(2);

            //// Graph in Figure 6.17
            //graph.Insert("A0");
            //graph.Insert("B1");
            //graph.Insert("C2");
            //graph.Insert("D3");
            //graph.Insert("E4");
            //graph.Insert("F5");
            //graph.Insert("G6");
            //graph.Insert("H7");
            //graph.Insert("I8");
            //graph.Insert("J9");
            //graph.Insert(1, 0, 1);
            //graph.Insert(1, 0, 7);
            //graph.Insert(1, 0, 8);
            //graph.Insert(1, 0, 9);
            //graph.Insert(1, 1, 0);
            //graph.Insert(1, 1, 2);
            //graph.Insert(1, 2,1);
            //graph.Insert(1, 2,3);
            //graph.Insert(1, 2,7);
            //graph.Insert(1, 3,2);
            //graph.Insert(1, 3,4);
            //graph.Insert(1, 3,6);
            //graph.Insert(1, 4,3);
            //graph.Insert(1, 4,6);
            //graph.Insert(1, 5,6);
            //graph.Insert(1, 6,5);
            //graph.Insert(1, 6,3);
            //graph.Insert(1, 6,4);
            //graph.Insert(1, 7,0);
            //graph.Insert(1, 7,2);
            //graph.Insert(1, 8,0);
            //graph.Insert(1, 8,9);
            //graph.Insert(1, 9,0);
            //graph.Insert(1, 9,8);
            //Vector_<Stack_<string>> stack = (Vector_<Stack_<string>>)graph.BCC(3);

        }
    }
}
