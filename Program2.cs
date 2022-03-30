using System.Diagnostics;
namespace Program
{
    class Program {         
        static void Main(string[] args)
        {
            //I have variables for rows and colm
            int matrix1Row = 3000;
            int matrix1Colm= 3000;
            int matrix2Row = 3000;
            int matrix2Colm= 3000;

            Random rnd = new Random();

            //Create first matrix with random integers
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

            //Create second matrix with random integers
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

            //Send two matrix to the both method
            Console.WriteLine("Sequantial Matrix: "+ serialAlgorithm(matrix1,matrix2));
            Console.WriteLine("Parallel Matrix: "+ parallelAlgorithm(matrix1Row,matrix1,matrix2));

        }
    static int[,] serialAlgorithm(int[,] matrix1, int[,] matrix2)
    {
            Stopwatch sw = new Stopwatch();

            int s = 0;
            int[,] newSequentialMatrix = new int[3000,3000];
            Console.WriteLine();
            Console.WriteLine("Sequential Matrix multiplication started...");

            //I have standart algorithm for create the serial algorith.
            sw.Start();
            for (int i = 0; i < 3000; i++)
            {
                for (int j = 0; j < 3000; j++)
                {
                    for (int k = 0; k < 3000; k++)
                    {
                        s = s + matrix1[i,k]*matrix2[k,j];
                    }
                    newSequentialMatrix[i,j] = s;
                    s = 0;
                }
            }
            sw.Stop();
            Console.WriteLine("Sequential Matrix multiplication is finished -- "+ sw.ElapsedMilliseconds + " ms.");
            Console.WriteLine("Last index of matrix is: "+newSequentialMatrix[2999,2999]);
            
            //  for (int i = 0; i < m1Row; i++){
            //      for (int j = 0; j < m2Colm; j++)
            //      {
            //          Console.Write(newMatrix[i,j]+ " ");
            //      }
            //      Console.WriteLine();
            //  }
            //  Console.WriteLine("Final matrix is printed");
        return newSequentialMatrix;
    }

    static int parallelAlgorithm(int matrix1Row, int[,] matrix1, int[,] matrix2)
    {
        Stopwatch sw = new Stopwatch();

        ParallelOptions parallelOptions = new ParallelOptions{
            MaxDegreeOfParallelism = 4
        };

        Console.WriteLine();
        Console.WriteLine("Parallel Matrix multiplication started...");

        //Create new matrix for multiplication.
        int[,] newParallelMatrix = new int[3000,3000];
        int[] subtotalls = new int[4];

        //I used Invoke method for parallelisation. It's looks better for me.
        sw.Start();
        Parallel.Invoke(
            parallelOptions,
            () => subtotalls[0] = CalculatePart(1, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[1] = CalculatePart(2, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[2] = CalculatePart(3, matrix1Row, matrix1, matrix2, newParallelMatrix),
            () => subtotalls[3] = CalculatePart(4, matrix1Row, matrix1, matrix2, newParallelMatrix)
        );
        sw.Stop();
        Console.WriteLine("Parallel Matrix multiplication is finished! -- " + sw.ElapsedMilliseconds + " ms.");

        //Print the last index of matrix
        int lastIndex = 0;
         foreach (var subtotall in subtotalls)
         {
            lastIndex = subtotall;
         }
         Console.WriteLine("Last index of matrix is: "+ lastIndex);
         return lastIndex;
    }
    static int CalculatePart(int n, int sizeOfMatrix, int[,] matrix1, int[,] matrix2, int[,]newParallelMatrix)
        {
            //string value = "";
            int s = 0;

            //I used same algorithm which you used during class.
            Console.WriteLine("Thread Number: " + Thread.CurrentThread.ManagedThreadId);
            for (int i = (n - 1) * (sizeOfMatrix / 4); i < n * (sizeOfMatrix / 4); i++){
                
                //I have standart algorithm for multiplication of matrices.
                    for (int j = 0; j < 3000; j++)
                    {
                        for (int k = 0; k < 3000; k++)
                        {
                            s = s + matrix1[i,k]*matrix2[k,j];
                        }
                        newParallelMatrix[i,j] = s;
                        //value += s.ToString() + " ";
                        s = 0;
                    }
            }
            return newParallelMatrix[2999,2999];
        }
    }
}