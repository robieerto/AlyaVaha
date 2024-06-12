namespace AlyaVaha
{
    public class WindowCommand
    {
        public string Command { get; set; }
        public string Value { get; set; }

        public WindowCommand(string command, string value)
        {
            Command = command;
            Value = value;
        }
    }
}
