using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ASD
{
    public class Lab08 : MarshalByRefObject
    {
        /// <summary>Etap I</summary>
        /// <param name="P">Tablica która dla każdego pola zawiera informacje, ile maszyn moze lacznie wyjechac z tego pola</param>
        /// <param name="MachinePos">Tablica zawierajaca informacje o poczatkowym polozeniu maszyn</param>
        /// <returns>Pierwszy element kroki to liczba uratowanych maszyn, drugi to tablica indeksów tych maszyn</returns>
            
        public (int savedNum, int[] Saved) Stage1(int[,] P, (int row, int col)[] MachinePos)
        {
			int h = P.GetLength(0);
            int w = P.GetLength(1);

            int numMachines = MachinePos.Length;

            int totalVerticles = numMachines + 2 * h * w + 2;
            int source = totalVerticles - 2;
            int sink = totalVerticles - 1;
            DiGraph<int> grap = new DiGraph<int>(totalVerticles);
            
            for(int i=0; i<numMachines; i++)
            {
                var (row, col) = MachinePos[i];
                int machineNode = i;
                int cellNodeIn = numMachines + (row * w + col)*2;
                int cellNodeOut = cellNodeIn + 1;

                grap.AddEdge(source, machineNode, 1);
                grap.AddEdge(machineNode, cellNodeIn, 1);
            }

            int[] directionsX = { -1, 1, 0, 0 };
            int[] directionsY = { 0, 0, -1, 1 };

            for(int row = 0; row < h; row++)
            {
                for(int col = 0; col < w; col++)
                {
                    int cellNodeIn = numMachines + (row * w + col) * 2;
                    int cellNodeOut = cellNodeIn + 1;
                    grap.AddEdge(cellNodeIn, cellNodeOut, P[row, col]);
                    
                    for (int i=0; i<4; i++)
                    {
                        int newRow = row + directionsX[i];
                        int newCol = col + directionsY[i];
                        if (newRow >= 0 && newRow < h && newCol >= 0 && newCol < w){
                            int neighborNodeIN = numMachines + (newRow * w + newCol)*2;
                            int neighborNodeOut = neighborNodeIN + 1;
                            grap.AddEdge(cellNodeOut, neighborNodeIN, P[row, col]);
                        }
                    }
                    if(row == 0)
                    {
                        grap.AddEdge(cellNodeOut, sink, P[row, col]);
                    }
                }
            }

            var (maxflow, flowGraph) = Flows.FordFulkerson(grap, source, sink);

            List<int> savedMachines = new List<int>();
            for(int i=0; i<numMachines; i++)
            {
                if (flowGraph.HasEdge(source, i))
                {
                    if(flowGraph.GetEdgeWeight(source, i) > 0)
                        savedMachines.Add(i);
                }
            }
            
            return (maxflow, savedMachines.ToArray());
        }

        /// <summary>Etap II</summary>
        /// <param name="P">Tablica która dla każdego pola zawiera informacje, ile maszyn moze lacznie wyjechac z tego pola</param>
        /// <param name="MachinePos">Tablica zawierajaca informacje o poczatkowym polozeniu maszyn</param>
        /// <param name="MachineValue">Tablica zawierajaca informacje o wartosci maszyn</param>
        /// <param name="moveCost">Koszt jednego ruchu</param>
        /// <returns>Pierwszy element kroki to najwiekszy mozliwy zysk, drugi to tablica indeksow maszyn, ktorych wyprowadzenie maksymalizuje zysk</returns>
        public (int bestProfit, int[] Saved) Stage2(int[,] P, (int row, int col)[] MachinePos, int[] MachineValue, int moveCost)
        {
            ///TUTAJ JEST JAKIS NETWORK A nie graph
			int h = P.GetLength(0);
            int w = P.GetLength(1);

            int numMachines = MachinePos.Length;

            int totalVerticles = numMachines + 2 * h * w + 2;
            int source = totalVerticles - 2;
            int sink = totalVerticles - 1;
            //DiGraph<int> grap = new DiGraph<int>(totalVerticles);
            NetworkWithCosts<int, int> network = new NetworkWithCosts<int, int>(totalVerticles);

            for (int i = 0; i < numMachines; i++)
            {
                var (row, col) = MachinePos[i];
                int machineNode = i;
                int cellNodeIn = numMachines + (row * w + col) * 2;
                int cellNodeOut = cellNodeIn + 1;

               
                //int cost = P[row, col] + moveCost; // dodane
                network.AddEdge(source, machineNode,(1, 0)); // moze ujemne
                network.AddEdge(machineNode, cellNodeIn, (1, 0)); // mozje ujemne?
                //network.AddEdge(cellNodeIn, cellNodeOut, (cost, 0)); // dodajen
            }

            int[] directionsX = { -1, 1, 0, 0 };
            int[] directionsY = { 0, 0, -1, 1 };

            for (int row = 0; row < h; row++)
            {
                for (int col = 0; col < w; col++)
                {
                    int cellNodeIn = numMachines + (row * w + col) * 2;
                    int cellNodeOut = cellNodeIn + 1;
                    network.AddEdge(cellNodeIn, cellNodeOut, (P[row, col], moveCost));

                    for (int i = 0; i < 4; i++)
                    {
                        int newRow = row + directionsX[i];
                        int newCol = col + directionsY[i];
                        if (newRow >= 0 && newRow < h && newCol >= 0 && newCol < w)
                        {
                            int neighborNodeIN = numMachines + (newRow * w + newCol) * 2;
                            int neighborNodeOut = neighborNodeIN + 1;
                            network.AddEdge(cellNodeOut, neighborNodeIN, (P[row, col], moveCost));
                        }
                    }
                    if (row == 0)
                    {
                        network.AddEdge(cellNodeOut, sink, (P[row, col], 0));
                    }
                }
            }


            var (maxFlow, bestcost, flowGraph) = Flows.MinCostMaxFlow(network, source, sink);
           
            List<int> savedMachines = new List<int>();
            for (int i = 0; i < numMachines; i++)
            {
                if (flowGraph.HasEdge(source, i))
                {
                    if (flowGraph.GetEdgeWeight(source, i) > 0)
                        savedMachines.Add(i);
                }
            }

            int bestProfit = 0;
            foreach(int machineIndex in savedMachines)
            {
                bestProfit += MachineValue[machineIndex];
            }
            

            return (bestProfit, savedMachines.ToArray());
        }
    }
}