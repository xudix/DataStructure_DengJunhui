// See https://aka.ms/new-console-template for more information

using DataStructure_DJ;


Vector<double> v = new Vector<double>(10,2,1,2,2,3,3,3,4,4,4,4,5,5,5,5,5);

Console.WriteLine(v);
v.Unsort();
Console.WriteLine(v);
v.Deduplicate();
Console.WriteLine(v);

