using System.Diagnostics;
    class Program {
        int [,] firstArray;
        int [,] secondArray;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.getArrays();
        }

    private void getArrays(){
        Program program = new Program();
        program.createFirstArray(firstArray);
        program.createSecondArray(secondArray);

        serialMatrix(program.getFirstArray(),program.getSecondArray());
        parallelMatrix(program.getFirstArray(),program.getSecondArray());
    }

    private  void createFirstArray(int[,] firstArray){
            Random random = new Random();
            firstArray = new int[1000,1000];
            for(int i=0;i<1000;i++)
            {
                for(int j=0;j<1000;j++)        
                {
                    firstArray[i, j]= random.Next(1,10);
                }
            }
            this.firstArray = firstArray;
    }
    private int[,] getFirstArray(){
        return this.firstArray;
    }
    private void createSecondArray(int[,] secondArray){
            Random random = new Random();
            secondArray = new int[1000,1000];
            for(int i=0;i<1000;i++)
            {
                for(int j=0;j<1000;j++)        
                {
                    secondArray[i,j]= random.Next(1,10);
                }
            }
            this.secondArray = secondArray;
    }
    private int[,] getSecondArray(){
        return this.secondArray;
    }
    private static int[,] serialMatrix(int[,] firstArray, int[,] secondArray)
    {
            Stopwatch stopwatch = new Stopwatch();

            int temp = 0;
            int[,] serialArray = new int[1000,1000];
            Console.WriteLine("Serial calculations happening");

            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    for (int k = 0; k < 1000; k++)
                    {
                        temp += firstArray[k,i]*secondArray[k,j];
                    }
                    serialArray[i,j] = temp;
                    temp = 0;
                }
            }
            stopwatch.Stop();
            Console.WriteLine("serial matrix: "+ stopwatch.ElapsedMilliseconds + " ms.");
        return serialArray;
    }

    private static int[,] parallelMatrix(int[,] firstArray, int[,] secondArray)
    {
        Stopwatch stopwatch = new Stopwatch();

        Console.WriteLine();
        Console.WriteLine("Parallel calculations happening");

        int[,] parallelArray = new int[1000,1000];

        var options = new ParallelOptions(){
            MaxDegreeOfParallelism = 4
        };
        stopwatch.Start();

        //I think using lock would be unnecessary so I didn't use.
        object lck = new object();
        Parallel.For(0, 1000, options, count => 
        {   
            int temp = 0;
            int i = count;
                    for (int j = 0; j < 1000; j++)
                    {
                        for (int k = 0; k < 1000; k++)
                        {
                           temp += firstArray[k,i]*secondArray[k,j];
                        }
                        parallelArray[i,j] = temp;
                        temp = 0;
                    }            
        });
        stopwatch.Stop();
        Console.WriteLine("parallel matrix: " + stopwatch.ElapsedMilliseconds + " ms.");
        return parallelArray;
    }
}
