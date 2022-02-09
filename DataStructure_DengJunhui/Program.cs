// For code testing.
// See https://aka.ms/new-console-template for more information


using ThisProj = DataStructure_DJ;


//ThisProj.StackExercices.Main();

var graph = new ThisProj.Graph_Matrix_<string, int>();
graph.Insert("A"); 
graph.Insert("B");
graph.Insert("C");
graph.Insert("D");
graph.Insert("E");
graph.Insert(1, 0, 2, 3);
graph.Insert(2, 2, 0,4);
graph.Insert(3, 1, 0, 7);
graph.Insert(4, 1, 2, 9);
graph.Insert(5, 1, 3, 2);
graph.Insert(6, 3, 1, 5);


