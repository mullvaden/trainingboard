using System.Linq;
using Chello.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chello.Tests
{
	[TestClass]
	public class CommentsTests : TestBase
	{
		[TestMethod]
		public void ForBoard()
		{
			var chello = new ChelloClient();
            var comments = chello.Comments.ForBoard(TestData.BoardName);
            Assert.IsNotNull(comments, "comments is null.");
            Assert.IsTrue(comments.Count() > 0, "No comments found.");

		    var comment = comments.Where(c => c.Id == TestData.CommentName).FirstOrDefault();
            Assert.IsNotNull(comment, "Comment with Id = {0} not found.", TestData.CommentName);
            Assert.IsNotNull(comment.Data, "Comment.Data is null.");
            Assert.AreEqual("Test Comment", comment.Data.Text);
            Assert.AreEqual(TestData.CommentDate, comment.Date);
		}
	}
}