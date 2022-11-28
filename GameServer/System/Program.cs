// See https://aka.ms/new-console-template for more information

using GameServer;

Server gameServer = Server.CreateServer(SystemConfig.maxConnections, SystemConfig.bufferSize);
gameServer.Start(SystemConfig.host, SystemConfig.port, SystemConfig.backlog);

Console.WriteLine("GameServer Started.");

string? str;
while (true)
{
	str = Console.ReadLine();

	if (str == "exit")
		break;
}

gameServer.Stop();