using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
		[Serializable]
	public class InputIsInvalidException : Exception
	{
		public InputIsInvalidException() : base()
		{
			Console.WriteLine("That is invalid");
		}
		public InputIsInvalidException(string entity) : base(entity)
		{
			Console.WriteLine($"The {entity} entered is invalid");
		}
		public InputIsInvalidException(string message, Exception innerException) : base(message, innerException) { }
		protected InputIsInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
