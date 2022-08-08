using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace GymRecorderNETversion
{
	public class CustomUserConverter : JsonConverter<User>
	{

		public override void Write(Utf8JsonWriter writer, User person, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			Console.Write("Here!");
			foreach (var prop in person.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				
				writer.WriteString(prop.Name, prop.GetValue(person)?.ToString());
			}
			writer.WriteEndObject();
		}
		public override User Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			//Intentionally not implemented
			throw new NotImplementedException();
		}
	}
}

	