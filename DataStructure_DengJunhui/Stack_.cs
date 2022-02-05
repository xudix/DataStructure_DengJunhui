using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class Stack_<T> where T : IEquatable<T>
    {
        private const int defaul_Capacity = 10;

        protected int _size, _capacity;
        protected T[] _elem;

        public Stack_(int capacity = defaul_Capacity, int size = 0, T initial_Val = default)
        {
            _elem = new T[_capacity = capacity > size ? capacity : size];
            for (_size = 0; _size < size; _size++)
                _elem[_size] = initial_Val;
        }


        public Stack_(int capacity = defaul_Capacity, int size = 0, params T[] initial_Vals)
        {
            size = initial_Vals.Length;
            _elem = new T[_capacity = capacity > size ? capacity : size];
            for (_size = 0; _size < size; _size++)
                _elem[_size] = initial_Vals[_size];
        }

        /// <summary>
        /// Use before inserting any element to the vector
        /// </summary>
        protected void Expand()
        {
            if (_size < _capacity) return;
            if (_capacity < defaul_Capacity) _capacity = defaul_Capacity;
            T[] oldElem = _elem;
            _elem = new T[_capacity <<= 1];
            for (int i = 0; i < _size; i++)
                _elem[i] = oldElem[i];
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Shrink()
        {
            if (_size << 2 > _capacity || _capacity < defaul_Capacity << 1) return;
            T[] oldElem = _elem;
            _elem = new T[_capacity >>= 1];
            for (int i = 0; i < _size; i++)
                _elem[i] = oldElem[i];
        }
        public void Push(T newElement)
        {
            Expand();
            _elem[_size++] = newElement;
        }

        public T Pop()
        {
            T pop = _elem[--_size];
            Shrink();
            return pop;
        }

        /// <summary>
        /// Find the target in disordered vector
        /// </summary>
        /// <param name="target"></param>
        /// <returns>The index of target in the vector. Returns -1 if the target is not found.</returns>
        public int Find(in T target) =>
            Find(target, 0, _size);

        /// <summary>
        /// Find the target in the range [lo, hi) of disordered vector
        /// </summary>
        /// <param name="target"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns>The index of target in the vector. Returns lo-1 if the target is not found.</returns>
        public int Find(in T target, int lo, int hi)
        {
            while (lo < hi-- && !_elem[hi].Equals(target)) ;
            return hi;
        }

        public T Top =>
            _elem[_size - 1];

        public bool Empty => _size == 0;

        public int Size => _size;

        /// <summary>
        /// Read only indexer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index] => _elem[index];
    }

    public class StackExercices
    {
        
        public static void Main()
        {
            N_Queens(10);
        }

        public static string ConvertToNewBase(Int64 input, int newBase)
        {
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
            Stack_<char> resultStack = new Stack_<char>();
            do
                resultStack.Push(digits[input%newBase]);
            while ((input/=newBase)>0);
            StringBuilder sb = new StringBuilder();
            while(!resultStack.Empty)
                sb.Append(resultStack.Pop());
            return sb.ToString();
        }

        #region Related to formular calculation
        public static bool ParenthesisCheck(string input)
        {
            char[] inputChars = input.ToCharArray();
            Stack_<char> stack_ = new Stack_<char>();
            foreach(char c in inputChars)
                switch (c)
                {
                    case '(': case '{': case '[':
                        stack_.Push(c);
                        break;
                    case ')':
                        if(stack_.Empty || stack_.Pop() != '(')
                            return false;
                        break;
                    case ']':
                        if (stack_.Empty || stack_.Pop() != '[')
                            return false;
                        break;
                    case '}':
                        if (stack_.Empty || stack_.Pop() != '{')
                            return false;
                        break;
                }
            return stack_.Empty;
        }

        /// <summary>
        /// Evaluate the string Formula and return the result.
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static double Evaluate(string formula)=>
            Evaluate(formula, new Dictionary<string, double>());

        /// <summary>
        /// Evaluate the string Formula and return the result.
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="variableValues">If the formula contains named variables, use this Dictionary to identify their values.</param>
        /// <returns>Result of the formula.</returns>
        /// <exception cref="InvalidDataException"></exception>
        public static double Evaluate(string formula, Dictionary<string, double> variableValues)
        {
            char[] formulaChars = (String.Concat(formula.Where(c => !Char.IsWhiteSpace(c)))).ToArray();
            Stack_<double> operands = new();
            Stack_<char> operators = new();
            operators.Push('\0');
            int i = 0;
            bool readingOperand = false;
            char current;
            int n_operators = 0;
            while (!operators.Empty)
            {
                switch (current = i < formulaChars.Length ? formulaChars[i] : '\0') // This section is to support some special features.
                    // The part regarding '(' allows writing expression that omit some * signs before '('
                    // The part regarding '+' and '-' allows using + and - as a signal of positive or negative number.
                {
                    case '(':
                        //if (i > 0 && (readingOperand || formulaChars[i - 1] == '!' || formulaChars[i - 1] == ')'))
                        if (operators.Top == '!')
                            break;
                        if (operands.Size - n_operators == 1)
                        {
                            if (operands.Size - n_operators != 1)
                                throw new InvalidDataException($"Invalid Formula {formula} at {i}: {current}.");
                            operators.Push('*');
                            n_operators++;
                        }
                        operators.Push(current);
                        readingOperand = false;
                        i++;
                        continue;
                    //case ')':
                    //    if (i == 0 || !(readingOperand || formulaChars[i - 1] == ')' || formulaChars[i - 1] == '!'))
                    //        throw new InvalidDataException($"Invalid Formula at {i}: {current}.");
                    //    if (operators.Top == '(')
                    //    {
                    //        operators.Pop();
                    //        continue;
                    //    }
                    //    break;
                    case '+': case '-':
                        if(operators.Top != '!' && operands.Size == n_operators)// In this case, + or - is unary, meaning positive or negative.
                        //if (i == 0 || !(readingOperand || formulaChars[i - 1] == ')' || formulaChars[i - 1] == '!')) 
                            if (current == '-') // '+' will be ignored.
                            {
                                operands.Push(-1);
                                operators.Push('*');
                                n_operators++;
                                i++;
                                continue;
                            }
                        break;
                    case '*': case '/': case '\\': case '%': case '^': case '!': case '\0': case ')':
                        break;
                    default:
                        // This will be reading operands
                        try
                        {
                            operands.Push(GetNumber(formulaChars, ref i, variableValues));
                        }
                        catch (InvalidDataException e)
                        {
                            throw new InvalidDataException($"Invalid name: {e.Message}");
                        }
                        
                        readingOperand = true;
                        continue;
                }
                readingOperand = false;
                if (operatorPrcd[current] <= operatorPrcd[operators.Top]) // Current operator's precedence is lower
                {
                    if(operatorPrcd[operators.Top] < 0) // ')' or '\0'
                        operators.Pop();
                    else
                    {
                        try
                        {
                            Calculate(operands, operators);
                        }
                        catch (Exception)
                        {
                            throw new InvalidDataException($"Invalid Formula {formula} at {i}: {current}.");
                        }
                        n_operators--;
                        i--; // Since we are operating with operators in the stack, current operator is not processed yet. Move index back.
                    }
                }
                else // Current operator's precedence is higher. Push into stack
                {
                    if (operands.Size - n_operators != 1)
                        throw new InvalidDataException($"Invalid Formula {formula} at {i}: {current}.");
                    operators.Push(current);
                    n_operators++;
                }
                i++;
            }
            
            return operands.Pop();
        }

        private static void Calculate(Stack_<double> operands, Stack_<char> operators)
        {
            if (operands.Size < 1 || (operands.Size < 2 && operators.Top != '!'))
                throw new InvalidDataException();
            switch (operators.Pop())
            {
                case '+':
                    operands.Push(operands.Pop() + operands.Pop());
                    break;
                case '-':
                    operands.Push(- operands.Pop() + operands.Pop());
                    break;
                case '*':
                    operands.Push(operands.Pop() * operands.Pop());
                    break;
                case '/':
                    double a = operands.Pop(), b = operands.Pop();
                    operands.Push(b/a);
                    break;
                case '\\':
                    operands.Push(operands.Pop() / operands.Pop());
                    break;
                case '%':
                    a = operands.Pop(); b = operands.Pop();
                    operands.Push(b % a);
                    break;
                case '^':
                    a = operands.Pop(); b = operands.Pop();
                    operands.Push(Math.Pow(b,a));
                    break;
                case '!':
                    int n = (int) operands.Pop();
                    Int64 result = (Int64)n;
                    while (n > 1)
                        result *= (--n);
                    operands.Push((double)result);
                    break;
                case ')':
                default :
                    throw new InvalidDataException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formulaChars"></param>
        /// <param name="index">Index of starting char of the operand. After executing getNumber, index will be moved to the first non-operand char.</param>
        /// <param name="variableValues"></param>
        /// <returns></returns>
        private static double GetNumber(char[] formulaChars, ref int index, Dictionary<string, double> variableValues)
        {
            StringBuilder buffer = new();
            char c = formulaChars[index];
            double result = IsNumeric(c) ? 0 : 1;
            double fraction = 1;
            string variableName = "";
            while ((c = formulaChars[index]) >= '0' && c <= '9') // Integer part
            {
                result = result * 10 + c - '0';
                if(++index == formulaChars.Length) // End of formula
                    return result;
            }
            if (c == '.') // Decimal part
                if (++index == formulaChars.Length) // End of formula. In this case, the formula ends with a '.'
                    return result;
                else
                    while ((c = formulaChars[index]) >= '0' && c <= '9')
                    {
                        result += ((c - '0') * (fraction /= 10));
                        if (++index == formulaChars.Length) // End of formula
                            return result;
                    }
            while (!operatorPrcd.ContainsKey(c)) // This is not an operator. Variable name part
            {
                buffer.Append(c);
                if (++index == formulaChars.Length) // End of formula
                    break;
                c = formulaChars[index];
            }
            if (buffer.Length > 0)
                try
                {
                    variableName = buffer.ToString();
                    result *= variableValues[variableName];
                }
                catch (Exception)
                {
                    throw new InvalidDataException(variableName);
                }
                
            // After the program c should be the first non-operand index, including end of line (formulaChars.Length).
            return result;
        }

        //enum oprPrcd { add = 12, sub = 12, mul = 13, div = 13, pow = 14, fra = 15, l_p = 100, r_p = -1 };
        protected static Dictionary<char, int> operatorPrcd = new Dictionary<char, int>
        {
            {'+',12 },
            {'-',12 },
            {'*',13 },
            {'/',13 },
            {'%',13 },
            {'\\',13 },
            {'^',14 },
            {'!',15 },
            {'(',-1 },
            {')',-1 },
            {'\0',-2 },
        };

        public static bool IsNumeric(char c) =>
            (c >= '0' && c <= '9') || c == '.';

        #endregion

        public static void N_Queens(int n)
        {
            Stack_<Queen> queenStack = new();
            queenStack.Push(new Queen(0, 0));
            Queen newQueen = new Queen(queenStack.Size, 0);
            StringBuilder sb = new();
            int solutionCount = 0, searchCount = 0;
            var display = () =>
            {
                sb.Clear();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < queenStack[i].y; j++)
                        sb.Append('O');
                    sb.Append('X');
                    for (int j = 0; j < n - queenStack[i].y - 1; j++)
                        sb.Append('O');
                    sb.AppendLine();
                }
                sb.AppendLine();
                Console.WriteLine(sb.ToString());
            };
            while (newQueen.x > 0 || newQueen.y < n) // End after all posibilities for Queen(0,y) are tried out.
            {
                while (queenStack.Find(newQueen) >= 0 && newQueen.y < n)
                {
                    newQueen.y++; searchCount++;
                }
                if (newQueen.y < n)
                {
                    queenStack.Push(newQueen);
                    if (queenStack.Size == n) //found one solution. Move to next.
                    {
                        //display();
                        solutionCount++;
                        newQueen = queenStack.Pop();
                        newQueen.y++;
                    }
                    else
                        newQueen = new Queen(queenStack.Size, 0);
                }
                else
                {
                    newQueen = queenStack.Pop();
                    newQueen.y++;
                }
            }
            Console.WriteLine(solutionCount);
            Console.WriteLine(searchCount);
        }
    }

    public struct Queen : IEquatable<Queen> // Used to solve the N-Queens problem.
    {
        public int x, y; // Position of Queen

        public Queen(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Determines if two Queens are attacking each other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Queen other)
        {
            return (x == other.x) || (y == other.y) || (x + y == other.x + other.y) || (x - y == other.x - other.y);
        }
    }
}
