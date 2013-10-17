using System;
using System.Collections.Generic;
using System.Linq;
using Chello.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chello.Tests
{
	[TestClass]
	public class BoardsTests : TestBase
	{
		[TestMethod]
		public void ForUser()
		{
			var chello = new ChelloClient();
			var boards = chello.Boards.ForUser(TestData.UserName);
		    foreach (var board in boards)
		    {
		        Console.WriteLine(board.Id  + " " + board.Name);
		    }
			Assert.IsTrue(boards.Count() > 0);
		}

		[TestMethod]
		public void Single()
		{
			var chello = new ChelloClient();
            var board = chello.Boards.Single("525bcdf08e8ccd036e00762d");
			Assert.IsNotNull(board);
            
            Assert.AreEqual("Träning", board.Name);

		}
	}
}
