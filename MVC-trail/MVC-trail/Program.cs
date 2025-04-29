using MVC_trail;

class Program {
    static async Task Main(string[] args) {
        if (args.Length > 0 && args[0] == "--server") {
            int port = args.Length > 1 ? int.Parse(args[1]) : 5555;
            await new Server().Start(port);
        } else if (args.Length > 0 && args[0] == "--client") {
            var host = "127.0.0.1";
            int port = 5555;

            if (args.Length > 1) {
                var parts = args[1].Split(':');
                host = parts[0];
                if (parts.Length > 1) port = int.Parse(parts[1]);
            }

            await new Client().Start(host, port);
        } else {
            Console.Write("Start as (S)erver or (C)lient? ");
            var choice = Console.ReadLine()?.ToLower();
            if (choice == "s") await new Server().Start();
            else await new Client().Start("127.0.0.1", 5555);
        }
    }
}