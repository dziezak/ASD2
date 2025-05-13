namespace Lab10;

public class Program2
{
   static void Permute(string s, string answer)
   {
      if (s.Length == 0)
      {
         Console.WriteLine("answer: " + answer);
         //Console.WriteLine("s: " + s);
         return;
      }

      for (int i = 0; i < s.Length; i++)
      {
         char ch = s[i];
         string left_substr = s.Substring(0, i);
         string right_substr = s.Substring(i + 1);
         string rest = left_substr + right_substr;
         Permute(rest, answer + ch);
      }
   } 
   
   static void Main1(string[] args)
   {
      string str = "ABC";
      Permute(str, "");
   }
}