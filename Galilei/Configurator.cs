using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using Galilei.Core;

using Newtonsoft.Json;

namespace Galilei
{
	public class Configurator
	{
		private string configPath;
		private Server srv;
		
		public Configurator (string configPath, Server srv)
		{
			this.configPath = configPath;
			this.srv = srv;
		}
		
		public void Save()
		{
			StreamWriter file = File.CreateText(configPath);
			using (JsonTextWriter jsonWriter = new JsonTextWriter(file))
			{
				SaveNode(jsonWriter, srv);
			}
			file.Close();
		}
		
		public void Load()
		{
			StreamReader file = File.OpenText(configPath);
			
			using (JsonTextReader jsonReader = new JsonTextReader(file))
			{
				
			}
		}
		
		private void SaveNode(JsonTextWriter jsonWriter, Node node)
		{			
			jsonWriter.Formatting = Formatting.Indented;
			
			jsonWriter.WriteStartObject();
			Accessor xpcaAccessor = new Accessor(node.GetType());
			
			foreach (KeyValuePair<string, PropertyInfo> property in xpcaAccessor.Properties) {
//				if ((int)(xpcaAccessor.GetXpcaAttribute(property.Value).Options & XpcaPropertyOptions.Config) == 0)
//						continue;
				
				object val = property.Value.GetValue(node, null);
					
				if (val != null) {
					jsonWriter.WritePropertyName(property.Key);
					if(val is Node) {
						jsonWriter.WriteValue(((Node)val).FullName);
					}
					else {							
						jsonWriter.WriteValue(val.ToString());
					}
				}
			}
			
			jsonWriter.WritePropertyName("children");
			jsonWriter.WriteStartArray();
			foreach (Node child in node.Children) {
				SaveNode(jsonWriter, child);
			}
			jsonWriter.WriteEndArray();
			
			jsonWriter.WriteEndObject();
		}
		
		private void LoadNode(JsonTextReader jsonReader, Node node)
		{
			if (jsonReader.Read()
				|| jsonReader.TokenType == JsonToken.StartObject) {
//				Dictionary<string, object> properties = new Dictionary<string, object>();
//				
//				xmlWriter.WriteStartElement("root");
//				while(jsonReader.Read()) {
//					switch (jsonReader.TokenType) {
//					case JsonToken.PropertyName:
//					case JsonToken.String:
//					case JsonToken.Boolean:
//					case JsonToken.Date:
//					case JsonToken.Integer:
//						if (name == "parent")
//						{
//								
//						} 
//						else {
//							properties.Add(name, jsonReader.Value);
//						}
//						break;
//					case JsonToken.StartArray:
//						List<object> objs = new List<object>();
//						while(jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray) {
//							objs.Add(jsonReader.Value);
//						}
//						properties.Add(name, objs.ToArray());
//						break;
//					}
//				}
			}
		}
	}
}

