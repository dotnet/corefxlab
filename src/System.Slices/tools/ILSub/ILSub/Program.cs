using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSub
{
    class Program
    {
        class Match
        {
            public int Index { get; set; }
            public int EndIndex { get; set; }
        }

        static Match FindContainingAllTokensInOrder(string s, int startIndex, int endIndex, string[] tokens)
        {
            int firstTokenStart;
            int lastTokenEnd;

            if (tokens.Length == 0)
            {
                return null;
            }
            firstTokenStart = s.IndexOf(tokens[0], startIndex, endIndex - startIndex);
            if (firstTokenStart == -1)
            {
                return null;
            }

            lastTokenEnd = firstTokenStart + tokens[0].Length;
            for (int i = 1; i < tokens.Length; i++)
            {
                lastTokenEnd = s.IndexOf(tokens[i], lastTokenEnd, endIndex - lastTokenEnd);
                if (lastTokenEnd == -1)
                {
                    return null;
                }
                lastTokenEnd += tokens[i].Length;
            }

            Match ret = new Match();
            ret.Index = firstTokenStart;
            ret.EndIndex = lastTokenEnd;
            return ret;
        }

        static Match FindILSubClass(string s)
        {
            string[] tokens = new string[]
            {
                @".class private auto ansi beforefieldinit System.ILSub",
                "}",
                "}",
                "\n"
            };

            return FindContainingAllTokensInOrder(s, 0, s.Length, tokens);
        }

        static Match FindMethod(string s, int startIndex, int endIndex)
        {
            string[] tokens = new string[]
            {
                @".method ",
                "{",
                "}",
                "\n"
            };

            return FindContainingAllTokensInOrder(s, startIndex, endIndex, tokens);
        }

        static string StringFromMatch(string s, Match match)
        {
            return s.Substring(match.Index, match.EndIndex - match.Index);
        }

        static Match GetMethodBody(string s, Match methodDef)
        {
            if (methodDef == null)
            {
                return null;
            }

            Match ret = new Match();
            ret.Index = s.IndexOf("{", methodDef.Index, methodDef.EndIndex - methodDef.Index);
            ret.EndIndex = s.LastIndexOf("}", methodDef.EndIndex - 1, methodDef.EndIndex - methodDef.Index);
            //string x = StringFromMatch(methodDef);

            if (ret.Index == -1 || ret.EndIndex == -1)
            {
                return null;
            }
            else
            {
                ret.Index = ret.Index + 1;

                return ret;
            }
        }

        static Match FindILSubInstance(string s, int startIndex, int endIndex)
        {
            string[] tokens = new string[]
            {
                @".custom instance void System.ILSub::.ctor(string)",
                "( ",
                " )",
                "\n"
            };

            return FindContainingAllTokensInOrder(s, startIndex, endIndex, tokens);
        }

        static Match FindILSubBytes(string s, int startIndex, int endIndex)
        {
            string[] tokens = new string[]
            {
                "( ",
                " )"
            };

            Match bytes = FindContainingAllTokensInOrder(s, startIndex, endIndex, tokens);
            if (bytes != null)
            {
                bytes.Index = bytes.Index + tokens[0].Length;
                bytes.EndIndex = bytes.EndIndex - tokens[tokens.Length - 1].Length;
            }
            return bytes;
        }

        static bool FindMethodWithILSubInstance(string s, int startIndex, int endIndex, out Match methodBody, out Match constructorBytes)
        {
            while (true)
            {
                Match method = FindMethod(s, startIndex, endIndex);
                if (method == null)
                {
                    methodBody = null;
                    constructorBytes = null;
                    return false;
                }
                startIndex = method.EndIndex;

                string dbg1 = StringFromMatch(s, method);

                methodBody = GetMethodBody(s, method);
                if (methodBody == null)
                {
                    constructorBytes = null;
                    continue;
                }

                string dbg2 = StringFromMatch(s, methodBody);

                Match ilSub = FindILSubInstance(s, methodBody.Index, methodBody.EndIndex);
                if (ilSub == null)
                {
                    continue;
                }
                string dbg3 = StringFromMatch(s, ilSub);

                constructorBytes = FindILSubBytes(s, ilSub.Index, ilSub.EndIndex);
                if (constructorBytes == null)
                {
                    throw new Exception("internal error");
                }

                string dbg4 = StringFromMatch(s, constructorBytes);

                return true;
            }
        }

        static bool IsHex(char c)
        {
            return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
        }

        static byte HexToByte(char c)
        {
            if (c >= '0' && c <= '9')
                return (byte)(c - '0');
            if (c >= 'A' && c <= 'F')
                return (byte)(c - 'A' + 10);
            if (c >= 'a' && c <= 'f')
                return (byte)(c - 'a' + 10);
            throw new Exception("internal error");
        }

        static byte[] GetBytes(string s, Match constructorBytes)
        {
            string dbg0 = StringFromMatch(s, constructorBytes);
            List<byte> bytes = new List<byte>();
            bool possibleCommentStart = false;
            for (int i = constructorBytes.Index; i < constructorBytes.EndIndex;)
            {
                while (i < constructorBytes.EndIndex && !IsHex(s[i]))
                {
                    if (!possibleCommentStart && s[i] == '/')
                    {
                        possibleCommentStart = true;
                        i++;
                        continue;
                    }
                    if (possibleCommentStart)
                    {
                        if (s[i] == '/')
                        {
                            do
                            {
                                i++;
                            } while (i < constructorBytes.EndIndex && s[i] != '\n');
                            i++;
                            continue;
                        }
                        else
                        {
                            possibleCommentStart = false;
                        }
                    }
                    i++;
                }

                if (i < constructorBytes.EndIndex)
                {
                    byte hi = HexToByte(s[i]);
                    i++;
                    if (i < constructorBytes.EndIndex)
                    {
                        if (IsHex(s[i]))
                        {
                            bytes.Add((byte)((hi << 4) | HexToByte(s[i])));
                            i++;
                            continue;
                        }
                    }
                    throw new Exception("internal error");
                }
            }

            if (bytes.Count < 6)
            {
                throw new Exception("internal error");
            }

            if (bytes[bytes.Count - 2] != 0)
            {
                throw new Exception("internal error");
            }

            if (bytes[bytes.Count - 1] != 0)
            {
                throw new Exception("internal error");
            }

            return bytes.ToArray();
        }

        static string RewriteIL(string input)
        {
            Match ilSubClass = FindILSubClass(input);
            if (ilSubClass == null)
            {
                throw new Exception("ILSub class not found");
            }

            input = input.Substring(0, ilSubClass.Index) + input.Substring(ilSubClass.EndIndex);

            StringBuilder sb = new StringBuilder();
            int i = 0;
            while (i < input.Length)
            {
                Match methodBody;
                Match constructorBytes;
                if (!FindMethodWithILSubInstance(input, i, input.Length, out methodBody, out constructorBytes))
                {
                    sb.Append(input, i, input.Length - i);
                    return sb.ToString();
                }

                sb.Append(input, i, methodBody.Index - i);
                byte[] bytes = GetBytes(input, constructorBytes);
                sb.Append(Encoding.UTF8.GetString(bytes, 4, bytes.Length - 6));
                sb.AppendLine();
                i = methodBody.EndIndex;
            }
            return sb.ToString();
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage ILSub.exe <input.il> <output.il>");
            }

            string input = File.ReadAllText(args[0]);
            Match ilsubclass = FindILSubClass(input);
            if (ilsubclass == null)
            {
                throw new Exception("class System.ILSub not found!");
            }

            File.WriteAllText(args[1], RewriteIL(input));
        }
    }
}
