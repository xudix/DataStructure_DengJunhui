// For code testing.
// See https://aka.ms/new-console-template for more information


using ThisProj = DataStructure_DJ;


//ThisProj.StackExercices.Main();

var queue= new ThisProj.Queue_<double>(5);
for (int i=0; i<11;i++)
    queue.Enqueue(i);

for (int i = 0; i < 8; i++)
    Console.WriteLine(queue.Dequeue());

for (int i = 0; i < 9; i++)
{
    queue.Enqueue(i);
    Console.WriteLine(queue.Dequeue());
}
    
