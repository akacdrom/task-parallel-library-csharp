using System.Diagnostics;
namespace Program
{
    class Program {         
        static void Main(string[] args)
        {
            int matrix1Row = 10000;
            int matrix1Colm= 10000;
            int matrix2Row = 10000;
            int matrix2Colm= 10000;

            Stopwatch sw = new Stopwatch();

            Random rnd = new Random();
            Console.WriteLine("First Matrix is being created...");
            int[,] matrix1 = new int[matrix1Row,matrix1Colm];
            for(int i=0;i<matrix1Row;i++)
            {
                for(int j=0;j<matrix1Colm;j++)        
                {
                    matrix1[i, j]= rnd.Next();
                    //Console.WriteLine("[{0}, {1}] = {2}", i, j, m1[i,j]);
                }
            }
            Console.WriteLine("First Matrix is created!\n");

            Console.WriteLine("Second Matrix is being created...");
            int[,] matrix2 = new int[matrix2Row,matrix2Colm];
            for(int i=0;i<matrix2Row;i++)
            {
                for(int j=0;j<matrix2Colm;j++)        
                {
                    matrix2[i,j]= rnd.Next();
                    //Console.WriteLine("[{0}, {1}] = {2}", i, j, m2[i,j]);
                }
            }
            Console.WriteLine("Second Matrix is created!");

            int s = 0;
            int[,] newSequentialMatrix = new int[10000,10000];
            Console.WriteLine();
            Console.WriteLine("Sequential Matrix multiplication started...");

            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    for (int k = 0; k < 10000; k++)
                    {
                        s = s + matrix1[i,k]*matrix2[k,j];
                    }
                    newSequentialMatrix[i,j] = s;
                    s = 0;
                }
            }
            sw.Stop();
            Console.WriteLine("Sequential Matrix multiplication is finished -- "+ sw.ElapsedMilliseconds + " ms.");
            Console.WriteLine("Last index of matrix is: "+newSequentialMatrix[9999,9999]);
            
            //  for (int i = 0; i < m1Row; i++){
            //      for (int j = 0; j < m2Colm; j++)
            //      {
            //          Console.Write(newMatrix[i,j]+ " ");
            //      }
            //      Console.WriteLine();
            //  }
            //  Console.WriteLine("Final matrix is printed");
        

        ParallelOptions parallelOptions = new ParallelOptions{
            MaxDegreeOfParallelism = 4
        };

        Console.WriteLine();
        Console.WriteLine("Parallel Matrix multiplication started...");
        int[,] newParallelMatrix = new int[10000,10000];
        int[] subtotalls = new int[8];

        sw.Reset();
        sw.Start();
        Parallel.Invoke(
            parallelOptions,
            () => subtotalls[0] = CalculatePart(1, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[1] = CalculatePart(2, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[2] = CalculatePart(3, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[3] = CalculatePart(4, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[4] = CalculatePart(5, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[5] = CalculatePart(6, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[6] = CalculatePart(7, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[7] = CalculatePart(8, matrix1Row, matrix1, matrix2, newParallelMatrix)
        );
        sw.Stop();
        Console.WriteLine("Parallel Matrix multiplication is finished! -- " + sw.ElapsedMilliseconds + " ms.");

        int lastIndex = 0;
         foreach (var subtotall in subtotalls)
         {
            lastIndex = subtotall;
         }
         Console.WriteLine("Last index of matrix is: "+ lastIndex);
    }
        static int CalculatePart(int n, int sizeOfMatrix, int[,] matrix1, int[,] matrix2, int[,]newParallelMatrix){
            //string value = "";
            int s = 0;

            Console.WriteLine("Thread Number: " + Thread.CurrentThread.ManagedThreadId);
            for (int i = (n - 1) * (sizeOfMatrix / 8); i < n * (sizeOfMatrix / 8); i++){
                
                    for (int j = 0; j < 10000; j++)
                    {
                        for (int k = 0; k < 10000; k++)
                        {
                            s = s + matrix1[i,k]*matrix2[k,j];
                        }
                        newParallelMatrix[i,j] = s;
                        //value += s.ToString() + " ";
                        s = 0;
                    }
            }
            return newParallelMatrix[9999,9999];
        }
    }
}
