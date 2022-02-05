using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class Stack_<T>: Vector_<T> where T : IComparable<T>, IEquatable<T>, IComparable
    {
        public void Push(T newElement) => 
            Insert(newElement);

        public T Pop()
        {
            T pop = _elem[--_size];
            Shrink();
            return pop;
        }

        public T Top =>
            this[_size - 1];
    }

    public class StackExercices
    {
        
        public static void Main()
        {
            try
            {
                Dictionary<string, double> dic = new Dictionary<string, double>
                {
                    {"a", 5.5},
                    {"b", 10},
                    {"c", -1},
                    {"d", -.3},
                };
                dic["d"] = -.5;
                Console.WriteLine(Evaluate("3!*(a+c)", out _, dic));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
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
        /// <param name="RPN"></param>
        /// <returns></returns>
        public static double Evaluate(string formula)=>
            Evaluate(formula, new Dictionary<string, double>());


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
        
    }


}
