using NUnit.Framework;
using System;
using Galilei;
using Galilei.Core;
using System.Net;

namespace Galilei.Test
{
	[TestFixture()]
	public class UPut
	{
		private Server srv;
		
		[TestFixtureSetUp]
		public void SetUp()
		{
			srv = new Server();
			srv.Port = 3003;
			Node node_1 = new Node("node_1");
			Node node_2 = new Node("node_2");
			
			node_1.Parent = srv;
			node_2.Parent = node_1;
	
			srv.Start();
		}
		
		[TestFixtureTearDown]
		public void TearDown()
		{
			srv.Stop();
		}
		
		
		[Test()]
		public void RenameNode()
		{
			HttpWebResponse response = Helper.Put("http://127.0.0.1:3003/node_1/node_2", "name=new_name");
			
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
					
			response = Helper.Get("http://127.0.0.1:3003/node_1/new_name");
			
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			Assert.AreEqual("application/json", response.ContentType);
			Assert.AreEqual("{\"type\":\"Node\"," +
				"\"name\":\"new_name\"," +
				"\"parent\":\"xpca://node_1\"," +
				"\"children\":[]}", 
				Helper.ReadFromStream(response.GetResponseStream(), response.ContentLength)
			);
		}
		
		[Test()]
		public void ReplaceNode()
		{
			HttpWebResponse response = Helper.Put("http://127.0.0.1:3003/node_1/new_name", "parent=xpca://");
			
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			
			response = Helper.Get("http://127.0.0.1:3003/new_name");
			
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			Assert.AreEqual("application/json", response.ContentType);
			Assert.AreEqual("{\"type\":\"Node\"," +
				"\"name\":\"new_name\"," +
				"\"parent\":\"xpca://\"," +
				"\"children\":[]}", 
				Helper.ReadFromStream(response.GetResponseStream(), response.ContentLength)
			);
		}
		
		[Test()]
		public void PutToUnexpectedPath()
		{
			HttpWebResponse response = Helper.Put("http://127.0.0.1:3003/node_xxx/new_node", "");
			
			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}
		
		[Test()]
		public void PutToUnexpectedNode()
		{
			HttpWebResponse response = Helper.Put("http://127.0.0.1:3003/node_1/new_node", "");
			
			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}

