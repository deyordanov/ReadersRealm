//58. Length of Last Word

string s = Console.ReadLine()!;
string[] strings = s
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .ToArray();

Console.WriteLine(s
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .ToArray()
    .Last()
    .Length);

//12. Integer to Roman
// int num = int.Parse(Console.ReadLine()!);
//
// List<string> romanNumber = new List<string>();
//
// Dictionary<int, string> mappingTable = new Dictionary<int, string>()
// {
//     { 1000, "M" },
//     { 900, "CM" },
//     { 500, "D" },
//     { 400, "CD" },
//     { 100, "C" },
//     { 90, "XC" },
//     { 50, "L" },
//     { 40, "XL" },
//     { 10, "X" },
//     { 9, "IX"},
//     { 5, "V" },
//     { 4, "IV" },
//     { 1, "I" },
// };
//
// foreach (int map in mappingTable.Keys)
// {
//     while (num >= map)
//     {
//         num -= map;
//         romanNumber.Add(mappingTable[map]);
//     }
// }
//
// Console.WriteLine(string.Join("", romanNumber));

//5. Longest Palindromic Substring

// string s = Console.ReadLine()!;
//
// int maxLength = 0;
// int maxLeftIndex = 0;
// int maxRightIndex = 0;
// for (int i = 0; i < s.Length; i++)
// {
//     int left = i;
//     int right = i;
//     while (left >= 0 &&
//            right < s.Length &&
//            s[left] == s[right])
//     {
//         if (right - left + 1 > maxLength)
//         {
//             maxLength = right - left + 1;
//             maxLeftIndex = left;
//             maxRightIndex = right;
//         }
//         left--;
//         right++;
//     }
//
//     left = i;
//     right = i + 1;
//     while (left >= 0 &&
//            right < s.Length &&
//            s[left] == s[right])
//     {
//         if (right - left + 1 > maxLength)
//         {
//             maxLength = right - left + 1;
//             maxLeftIndex = left;
//             maxRightIndex = right;
//         }
//         left--;
//         right++;
//     }
// }
//
// Console.WriteLine(maxLength);
// Console.WriteLine(s.Substring(maxLeftIndex, maxRightIndex - maxLeftIndex + 1));


//387. First Unique Character in a String
// string s = Console.ReadLine()!;
//
// Dictionary<char, int> occurrences = new Dictionary<char, int>();
//
// int firstNumber = 0;
// foreach (char c in s)
// {
//     occurrences.TryAdd(c, 0);
//     occurrences[c]++;
// }
//
// Console.WriteLine(GetIndex());
// int GetIndex()
// {
//     for (int i = 0; i < s.Length; i++)
//     {
//         char c = s[i];
//         if (occurrences[c] == 1)
//         {
//             return i;
//         }
//     }
//
//     return -1;
// }

// 2958. Length of Longest Subarray With at Most K Frequency
// int[] nums = Console
//     .ReadLine()!
//     .Split(",", StringSplitOptions.RemoveEmptyEntries)
//     .Select(int.Parse)
//     .ToArray();
//
// int k = int.Parse(Console.ReadLine()!);
// Dictionary<int ,int> occurrences = new Dictionary<int ,int>();
//
// int maxLength = 0;
// int left = 0;
// for (int right = 0; right < nums.Length; right++)
// {
//     int number = nums[right];
//     occurrences.TryAdd(number, 0);
//     occurrences[number]++;
//
//     while (occurrences[number] > k)
//     {
//         int leftNumber = nums[left];
//         occurrences[leftNumber]--;
//         left++;
//     }
//
//     maxLength = Math.Max(maxLength, right - left + 1);
// }
//
// Console.WriteLine(maxLength);

//3. Longest Substring Without Repeating Characters
// string s = Console.ReadLine()!;
//
// Dictionary<char, int> occurrences = new Dictionary<char, int>();
//
// int maxLength = 0;
// int left = 0;
// for (int right = 0; right < s.Length; right++)
// {
//     char c = s[right];
//     if (!occurrences.ContainsKey(s[right]))
//     {
//         occurrences.Add(c, 0);
//     }
//     occurrences[c]++;
//
//     while (occurrences[c] > 1)
//     {
//         char cLeft = s[left];
//         occurrences[cLeft]--;
//         left++;
//     }
//
//     maxLength = Math.Max(maxLength, right - left + 1);
// }
//
// Console.WriteLine(maxLength);

//Pow(x, n)
// double x = double.Parse(Console.ReadLine()!);
// int n = int.Parse(Console.ReadLine()!);
//
// bool isNegative = n < 0;
//
// double number = Pow(x, Math.Abs((long)n));
//
// Console.WriteLine(isNegative ? (1.0 / number) :
//     number);
// double Pow(double x, long n)
// {
//     if (n == 0)
//     {
//         return 1;
//     }
//
//     if (n == 1)
//     {
//         return x;
//     }
//
//     double result = Pow(x, n / 2);
//     double power = result * result;
//
//     if (n % 2 == 1)
//     {
//         power *= x;
//     }
//
//     return power;
// }
//

// //Longest Increasing Sequence
//
// // int[] nums = new int[] { 10, 9, 2, 5, 3, 7, 101, 18 };
// // int[] nums = new int[] { 0, 1, 0, 3, 2, 3 };
// // int[] nums = new int[] { 7, 7, 7, 7, 7, 7, 7, 7 };
// // int[] nums = new int[] { 1, 2, 5, 3, 4 };
// // int[] nums = new int[] { 4, 3, 2, 1 };
// int[] nums = new int[] { 4, 2, -1, 3, 5, 5 };
// int[] parents = new int[nums.Length];
// Array.Fill(parents, -1);
//
// //Initialize a dp array, which will keep track of the LIS
// //ending at the current index.
// int[] dp = new int[nums.Length];
// //Fill the array with 1s, since the LIS ending at any index is the number at the given index.
// Array.Fill(dp, 1);
//
// //Declare a variable to keep track of the LIS count.
// int maxLength = 1;
// //Compare all elements to the left of nums[i], if the number at nums[j] is less than the one at nums[i] we increment the count of LIS at dp[i] where we either extend the LIS at dp[j] if it is longer than the one we already have found dp[i].
// for (int i = 1; i < nums.Length; i++)
// {
//     for (int j = 0; j < i; j++)
//     {
//         if (nums[j] < nums[i])
//         {
//             dp[i] = Math.Max(dp[i], dp[j] + 1);
//             parents[i] = j;
//         }
//     }
//
//     maxLength = Math.Max(maxLength, dp[i]);
// }
//
// //We can use Binary Search instead as well
// int maxIndex = 0;
// while (dp[maxIndex] != maxLength)
// {
//     maxIndex++;
// }
//
// Stack<int> sequence = new Stack<int>();
// sequence.Push(nums[maxIndex]);
// while (parents[maxIndex] != -1)
// {
//     sequence.Push(nums[parents[maxIndex]]);
//     maxIndex = parents[maxIndex];
// }
//
// Console.WriteLine($"LIS: {string.Join(" ", sequence)}");
// Console.WriteLine($"LIS length: {maxLength}");

//DFS + BFS

// Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();
// HashSet<string> visited = new HashSet<string>();
// HashSet<string> cycles = new HashSet<string>();
// int nodesCount = int.Parse(Console.ReadLine()!);
//
// Stack<string> topologicalSorting = new Stack<string>();
//
// ReadGraph(nodesCount);
//
// foreach (string node in graph.Keys)
// {
//     DFS(node);
// }
//
// Console.WriteLine(string.Join(" ", topologicalSorting));
//
// void BFS(string startingNode)
// {
//     Queue<string> queue = new Queue<string>();
//     queue.Enqueue(startingNode);
//
//     while (queue.Count > 0)
//     {
//         string node = queue.Dequeue();
//
//         Console.WriteLine(node);
//
//         foreach (string child in graph[node])
//         {
//             queue.Enqueue(child);
//         }
//     }
// }
// void DFS(string node)
// {
//     if (cycles.Contains(node))
//     {
//         Console.WriteLine("Cycle detected!");
//         Environment.Exit(1);
//     }
//
//     if (visited.Contains(node))
//     {
//         return;
//     }
//
//     visited.Add(node);
//     cycles.Add(node);
//
//     foreach (string child in graph[node])
//     {
//         DFS(child);
//     }
//
//     cycles.Remove(node);
//     topologicalSorting.Push(node);
// }
// void ReadGraph(int n)
// {
//     for (int i = 0; i < n; i++)
//     {
//         string[] nodeArgs = Console
//             .ReadLine()!
//             .Split("->", StringSplitOptions.RemoveEmptyEntries);
//
//         string parent = nodeArgs[0].Trim();
//         List<string> children = nodeArgs.Length == 1 ? new List<string>()
//             : nodeArgs[1]
//                 .Trim()
//                 .Split(", ", StringSplitOptions.RemoveEmptyEntries)
//                 .ToList();
//
//         graph.Add(parent, children);
//     }
// }


//Binary Search
//Time Complexity: O(log(n))
//Space Complexity: O(1)
// int[] nums = new int[] { 10, 9, 4, 5, 3, 2, 4, 7, 9 };
// Array.Sort(nums);
// int number = int.Parse(Console.ReadLine()!);
//
// int startIndex = 0;
// int endIndex = nums.Length;
// int middleIndex = (endIndex + startIndex) / 2;
//
// while (startIndex <= endIndex)
// {
//     if (number > nums[middleIndex])
//     {
//         startIndex = middleIndex + 1;
//         middleIndex = (endIndex + startIndex) / 2;
//     }
//     else if(number < nums[middleIndex])
//     {
//         endIndex = middleIndex - 1;
//         middleIndex = (endIndex + startIndex) / 2;
//     }
//     else
//     {
//         Console.WriteLine($"Number found at {middleIndex} - {number}");
//         return;
//     }
// }
//
// Console.WriteLine("The number is not present in the given collection!");


//MergeSort
//Time Complexity: O(n*log(n))
//Space Complexity: O(n)
//Stable: true
// int[] nums = new int[] { 10, 9, 4, 5, 3, 2, 4, 7, 9 };
// int[] nums = new int[] { 5, 4, 3, 2, 1 };
// int[] nums = new int[] { 1, 4, 2, -1, 0 };
//
// Console.WriteLine(string.Join(" ", MergeSort(nums)));
// int[] MergeSort(int[] nums)
// {
//     if (nums.Length == 1)
//     {
//         return nums;
//     }
//
//     int[] leftHalf = nums.Take(nums.Length / 2).ToArray();
//     int[] rightHalf = nums.Skip(nums.Length / 2).ToArray();
//     return Merge(MergeSort(leftHalf), MergeSort(rightHalf));
// }
//
// int[] Merge(int[] leftArray, int[] rightArray)
// {
//     int leftIndex = 0;
//     int rightIndex = 0;
//     int[] merged = new int[leftArray.Length + rightArray.Length];
//     int mergedIndex = 0;
//
//     while (leftIndex < leftArray.Length &&
//            rightIndex < rightArray.Length)
//     {
//         if (leftArray[leftIndex] < rightArray[rightIndex])
//         {
//             merged[mergedIndex++] = leftArray[leftIndex++];
//         }
//         else
//         {
//             merged[mergedIndex++] = rightArray[rightIndex++];
//         }
//     }
//
//     if (leftIndex < leftArray.Length)
//     {
//         for (int i = leftIndex; i < leftArray.Length; i++)
//         {
//             merged[mergedIndex++] = leftArray[i];
//         }
//     }
//     else
//     {
//         for (int i = rightIndex; i < rightArray.Length; i++)
//         {
//             merged[mergedIndex++] = rightArray[i];
//         }
//     }
//
//     return merged;
// }

//QuickSort
//Time Complexity: O(n*log(n))
//Space Complexity: O(1)
//Stable: false
// int[] nums = new int[] { 10, 9, 4, 5, 3, 2, 4, 7, 9 };
//
// QuickSort(nums, 0, nums.Length - 1);
//
// Console.WriteLine(string.Join(" ", nums));
//
// void QuickSort(int[] nums, int startIndex, int endIndex)
// {
//     if (startIndex >= endIndex)
//     {
//         return;
//     }
//
//     int pivot = startIndex;
//     int leftIndex = startIndex + 1;
//     int rightIndex = endIndex;
//
//     while (leftIndex <= rightIndex)
//     {
//         if (nums[leftIndex] > nums[pivot] &&
//             nums[rightIndex] < nums[pivot])
//         {
//             Swap(nums, leftIndex, rightIndex);
//         }
//
//         if (nums[leftIndex] <= nums[pivot])
//         {
//             leftIndex++;
//         }
//
//         if (nums[rightIndex] >= nums[pivot])
//         {
//             rightIndex--;
//         }
//     }
//
//     Swap(nums, pivot, rightIndex);
//
//     int leftArraySize = rightIndex - 1;
//     int rightArraySize = endIndex - pivot + 1;
//     if (leftArraySize < rightArraySize)
//     {
//         QuickSort(nums, 0, rightIndex - 1);
//         QuickSort(nums, rightIndex + 1, endIndex);
//     }
//     else
//     {
//         QuickSort(nums, rightIndex + 1, endIndex);
//         QuickSort(nums, 0, rightIndex - 1);
//     }
// }
//
// void Swap(int[] nums, int first, int second)
// {
//     int temp = nums[first];
//     nums[first] = nums[second];
//     nums[second] = temp;
// }


//Insertion Sort
//Time Complexity: O(n^2)
//Space Complexity: O(1)
//Stable: true

// int[] nums = new int[] { 10, 9, 4, 5, 3, 2, 4, 7, 9 };
// for (int i = 1; i < nums.Length; i++)
// {
//     int j = i;
//     while (j - 1 >= 0 && nums[j] < nums[j - 1])
//     {
//         int temp = nums[j];
//         nums[j] = nums[j - 1];
//         nums[j - 1] = temp;
//         j--;
//     }
// }

// Console.WriteLine(string.Join(" ", nums));

//Selection Sort
//Time Complexity: O(n^2)
//Space Complexity: O(1)
//Stable: false
// int[] nums = new int[] { 10, 9, 4, 5, 3, 2, 4, 7, 9 };
//
// for (int i = 0; i < nums.Length; i++)
// {
//     int minIndex = i;
//     for (int j = i; j < nums.Length; j++)
//     {
//         if (nums[j] < nums[minIndex])
//         {
//             minIndex = j;
//         }
//     }
//
//     int temp = nums[minIndex];
//     nums[minIndex] = nums[i];
//     nums[i] = temp;
// }
//
// Console.WriteLine(string.Join(" ", nums));


//Bubble Sort
//Time Complexity = O(n^2)
//Space Complexity = O(1)
//Stable = true

// int[] nums = new int[] { 10, 9, 4, 5, 3, 2, 4, 7, 9 };
//
// int switchesMade = -1;
// while (switchesMade != 0)
// {
//     switchesMade = 0;
//     for (int j = 1; j < nums.Length; j++)
//     {
//         if (nums[j - 1] > nums[j])
//         {
//             int temp = nums[j];
//             nums[j] = nums[j - 1];
//             nums[j - 1] = temp;
//             switchesMade++;
//         }
//     }
// }
//
// Console.WriteLine(string.Join(" ", nums));

//8 Queens
// int size = int.Parse(Console.ReadLine()!);
//
// string[,] board = new string[size, size];
// int solutions = 0;
//
// GetQueens(board, 0);
// Console.WriteLine(solutions);
// void GetQueens(string[,] board, int row)
// {
//     if (row == board.GetLength(0))
//     {
//         solutions++;
//         Console.WriteLine("A solution has been found!");
//         return;
//     }
//
//     for (int col = 0; col < board.GetLength(1); col++)
//     {
//         if (IsSafe(board, row, col))
//         {
//             board[row, col] = "1";
//             GetQueens(board, row + 1);
//             board[row, col] = "0";
//         }
//     }
// }
//
// bool IsSafe(string[,] board, int row, int col)
// {
//     if (row < 0 ||
//         col < 0 ||
//         row >= board.GetLength(0) ||
//         col >= board.GetLength(1))
//     {
//         return false;
//     }
//
//     for (int i = 1; i < board.GetLength(0); i++)
//     {
//         //Up
//         if (row - i >= 0 &&
//             board[row - i, col] == "1")
//         {
//             return false;
//         }
//
//         //Right
//         if (col + i < board.GetLength(1) &&
//             board[row, col + 1] == "1")
//         {
//             return false;
//         }
//
//         //Down
//         if (row + i < board.GetLength(0) &&
//             board[row + i, col] == "1")
//         {
//             return false;
//         }
//
//         //Left
//         if (col - i >= 0 &&
//             board[row, col - i] == "1")
//         {
//             return false;
//         }
//
//         //Right Diagonal - Up
//         if (row - i >= 0 &&
//             col + i < board.GetLength(1) &&
//             board[row - i, col + i] == "1")
//         {
//             return false;
//         }
//
//         //Right Diagonal - Down
//         if (row + i < board.GetLength(0) &&
//             col + i < board.GetLength(1) &&
//             board[row + i, col + i] == "1")
//         {
//             return false;
//         }
//
//         //Left Diagonal - Up
//         if (row - i >= 0 &&
//             col - i >= 0 &&
//             board[row - i, col - i] == "1")
//         {
//             return false;
//         }
//
//         //Left Diagonal - Down
//         if (row + i < board.GetLength(0) &&
//             col - i >= 0 &&
//             board[row + i, col - i] == "1")
//         {
//             return false;
//         }
//     }
//
//     return true;
// }

//Maze
// string[][] matrix = new string[3][]
// {
//     new string[] { "0", "0", "0" },
//     new string[] { "0", "1", "0" },
//     new string[] { "0", "0", "E" },
// };

// string[] matrix = new string[]
// {
//     "010001",
//     "01010E",
//     "010100",
//     "000000",
// };
//
// List<string> paths = new List<string>();
// HashSet<string> visited = new HashSet<string>();
//
// Traverse(0, 0, "");
//
// Console.WriteLine(string.Join(Environment.NewLine, paths));
// void Traverse(int row, int col, string path)
// {
//     if (matrix[row][col] == 'E')
//     {
//         paths.Add(path);
//         return;
//     }
//
//     visited.Add($"{row},{col}");
//
//     //Up
//     if (IsSafe(row - 1, col))
//     {
//         Traverse(row - 1, col, path + "U");
//     }
//
//     //Right
//     if (IsSafe(row, col + 1))
//     {
//         Traverse(row, col + 1, path + "R");
//     }
//
//     //Down
//     if (IsSafe(row + 1, col))
//     {
//         Traverse(row + 1, col, path + "D");
//     }
//
//     //Left
//     if (IsSafe(row, col - 1))
//     {
//         Traverse(row, col - 1, path + "L");
//     }
//
//     visited.Remove($"{row},{col}");
// }
//
// bool IsSafe(int row, int col)
// {
//     return row >= 0 &&
//            col >= 0 &&
//            row < matrix.Length &&
//            col < matrix[row].Length &&
//            matrix[row][col] != '1' &&
//            !visited.Contains($"{row},{col}");
// }


//Longest Common Subsequence
// int[] nums1 = Console
//     .ReadLine()
//     .Split(" ", StringSplitOptions.RemoveEmptyEntries)
//     .Select(int.Parse)
//     .ToArray();
//
// int[] nums2 = Console
//     .ReadLine()
//     .Split(" ", StringSplitOptions.RemoveEmptyEntries)
//     .Select(int.Parse)
//     .ToArray();

// string string1 = Console.ReadLine()!;
// string string2 = Console.ReadLine()!;
//
// int[,] dp = new int[string1.Length + 1, string2.Length + 1];
// for (int row = 1; row < dp.GetLength(0); row++)
// {
//     for (int col = 1; col < dp.GetLength(1); col++)
//     {
//         if (string1[row - 1] == string2[col - 1])
//         {
//             dp[row, col] = dp[row - 1, col - 1] + 1;
//         }
//         else
//         {
//             dp[row, col] = Math.Max(dp[row - 1, col], dp[row, col - 1]);
//         }
//     }
// }
//
// for (int row = 1; row < dp.GetLength(0); row++)
// {
//     for (int col = 1; col < dp.GetLength(1); col++)
//     {
//         Console.Write(dp[row, col]);
//     }
//
//     Console.WriteLine("");
// }
// Stack<char> sequence = new Stack<char>();
//
// int r = string1.Length;
// int c = string2.Length;
//
// while (r > 0 && c > 0)
// {
//     if (string1[r - 1] == string2[c - 1] &&
//         dp[r, c] == dp[r - 1, c - 1] + 1)
//     {
//         sequence.Push(string1[r - 1]);
//         r--;
//         c--;
//     }
//     else if (dp[r - 1, c] > dp[r, c -1])
//     {
//         r--;
//     }
//     else
//     {
//         c--;
//     }
// }
//
// Console.WriteLine(string.Join("", sequence));
//
// Console.WriteLine(dp[string1.Length, string2.Length]);

//Showing Stack Size - must enable unsafe code from Properties -> Build
// Recursion();
//
// void Recursion()
// {
//     Overflow();
//     Recursion();
// }
//
// unsafe void Overflow()
// {
//     int x = 5;
//     Console.WriteLine((int)&x);
// }

//For Loop
// int n = int.Parse(Console.ReadLine()!);
//
// Loop(n);
// void Loop(int n)
// {
//     if (n == 0)
//     {
//         return;
//     }
//
//     Console.WriteLine(n);
//
//     Loop(n -1);
// }


//Factorial
// int factorial = int.Parse(Console.ReadLine()!);
//
// Console.WriteLine(CalculateFactorial(factorial));
// long CalculateFactorial(int factorial)
// {
//     if (factorial <= 1)
//     {
//         return 1;
//     }
//
//     return factorial * CalculateFactorial(factorial - 1);
// }


//Fibonacci Sequence
// int fib = int.Parse(Console.ReadLine()!);
//
// Dictionary<int, long> memo = new Dictionary<int, long>();
//
// Console.WriteLine(CalculateFibonacci(fib));
// long CalculateFibonacci(int fib)
// {
//     if (fib <= 1)
//     {
//         return fib;
//     }
//
//     long result;
//
//     if (!memo.ContainsKey(fib))
//     {
//         result = CalculateFibonacci(fib - 1) + CalculateFibonacci(fib - 2);
//         memo.Add(fib, result);
//     }
//     else
//     {
//         result = memo[fib];
//     }
//
//     return result;
// }