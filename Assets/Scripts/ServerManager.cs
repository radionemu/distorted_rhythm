using UnityEngine;

public static class ServerManager
{
	public static string server1 = "localhost:11345";
	public static string server2 = "106.246.242.58:11345";
	public static bool isChangeServer = false;

	public static string GetServer() => isChangeServer ? server1 : server1;
	public static bool ChangeServer()
	{
		isChangeServer = !isChangeServer;
		return isChangeServer;
	}
}