﻿using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;
using System.Collections.Generic;

namespace SelfHost
{
	[HubName("SayAnything")]
	public class SayAnything : Hub
	{
		//public void SendMessage(string username, string message)
		//{
		//	UserData usr = new UserData(username);
		//	Debug.WriteLine("Stock update for {0} new price {1}", usr.Username, usr.Message);
		//	//Clients.All.MessageReceived(username, message);
		//	Clients.All.MessageReceived(usr);
		//}

		[HubMethodName("GetPlayerList")]
		public List<UserData> GetPlayers()
		{
			Debug.WriteLine("[Invoke] GetPlayerList");
			return Program.gameData.UserList;
		}

		// Client listen to UserAdded
		[HubMethodName("NewPlayer")]
		public void NewPlayer(string username)
		{
			Debug.WriteLine("[Invoke] NewPlayer");
			Program.gameData.AddUser(new UserData(username));

			Clients.All.UserAdded(Program.gameData.UserList);
			Debug.WriteLine("[Broadcast] UserAdded - User " + username + " added. Total User: " + Program.gameData.UserList.Count);
		}

		[HubMethodName("DeletePlayer")]
		public void DeletePlayer(string username)
		{
			Debug.WriteLine("[Invoke] DeletePlayer");
			Program.gameData.RemoveUserByName(username);
			Clients.All.UserDeleted(Program.gameData.UserList);
			Debug.WriteLine("[Broadcast] UserDeleted - User " + username + " deleted. Total User: " + Program.gameData.UserList.Count);
		}

		[HubMethodName("StartGame")]
		public void StartGame()
		{
			Debug.WriteLine("[Invoke] StartGame");
			Program.gameData.SetHost();
			Clients.All.GameStart(Program.gameData.UserList, Program.gameData.QuestionNum);
			Debug.WriteLine("[Broadcast] GameStart");

		}

		[HubMethodName("SubmitAnswer")]
		public void SubmitAnswer(string username, string answer)
		{
			Debug.WriteLine("[Invoke] SubmitAnswer");
			Program.gameData.SetAnswerByName(username, answer); // set answer
			Clients.All.AnswerSubmitted(Program.gameData.UserList, Program.gameData.IsAllUserSubmit()); // send updated userlist and is all user submit answer
			Debug.WriteLine("[Broadcast] AnswerSubmitted");

		}
	}
}