namespace SFA.DAS.Encoder
{
    public class Console : IConsole
    {
        public void Write(string s)
        {
            System.Console.WriteLine(s);
        }

        public void Write(object o)
        {
            Write(o.ToString());
        }

        public void Write(string name, string value)
        {
            Write($"{name,20} : {value}");
        }

        public void Write(string name, object value)
        {
            Write(name, value.ToString());
        }
    }
}