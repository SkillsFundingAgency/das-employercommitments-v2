namespace SFA.DAS.Encoder
{
    public interface IConsole
    {
        void Write(string s);
        void Write(object o);
        void Write(string name, string value);
        void Write(string name, object value);
    }
}