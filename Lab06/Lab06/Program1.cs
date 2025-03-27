using ASD.Graphs;

namespace ASD;

public class Program1
{
    void main(string[] args)
    {
    TestSet Stage2 = new TestSet(prototypeObject: new Lab06(), description: "Etap II", settings: true);
        
        DiGraph<int> G = new DiGraph<int>(10);
        Graph<int> C = new Graph<int>(10); /// Oczywiście ten jest nieskierowany a G jest skierowany
        int[] waitTime = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int s = 0, t = 9;
        for (int i = 0; i < 10; ++i)
        {
            G.AddEdge(i, (i + 1) % 10, 10 - i);
            C.AddEdge(i, (i + 1) % 10, 1);
        }
        Lab06 test = new Lab06();
        
        //test.Stage2(G, waitTime, s, 98, 1,);


    }
}