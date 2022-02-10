// For code testing.
// See https://aka.ms/new-console-template for more information


using ThisProj = DataStructure_DJ;


//ThisProj.StackExercices.Main();

var graph = new ThisProj.Graph_Matrix_<string, int>();
graph.Insert("A0"); 
graph.Insert("B1");
graph.Insert("C2");
graph.Insert("D3");
graph.Insert("E4");
graph.Insert("F5");
graph.Insert("G6");
graph.Insert("S7");
graph.Insert(1, 0, 2, 3);
graph.Insert(2, 0, 4,4);
graph.Insert(3, 2, 1, 7);
graph.Insert(4, 3, 1, 9);
graph.Insert(5, 4, 5, 2);
graph.Insert(6, 4, 6, 5);
graph.Insert(0, 4, 6, 1);
graph.Insert(0, 6, 5, 1);
graph.Insert(0, 6, 1, 1);
graph.Insert(0, 7, 0, 1);
graph.Insert(0, 7, 2, 1);
graph.Insert(0, 7, 3, 1);
graph.BFS(7);


