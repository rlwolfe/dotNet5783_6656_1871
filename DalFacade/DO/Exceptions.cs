﻿using System.Runtime.Serialization;

namespace DO
{
	[Serializable]
	public class EntityNotFoundException : Exception
	{
		public EntityNotFoundException() : base() {
			Console.WriteLine("That doesn't exist");
		}
		public EntityNotFoundException(string entity) : base(entity) {
			Console.WriteLine($"This {entity} doesn't exist");
		}
		public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
		protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}

	[Serializable]
	public class idNotFoundException : Exception
	{
		public idNotFoundException() : base() {
			Console.WriteLine("This ID does not exist");
		}
		public idNotFoundException(string entity) : base(entity) {
			Console.WriteLine($"This ID does not match any existing {entity}s");
		}
		public idNotFoundException(string message, Exception innerException) : base(message, innerException) { }
		protected idNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}
	
	[Serializable]
	public class idAlreadyExistsException : Exception
	{
		public idAlreadyExistsException() : base() {
			Console.WriteLine("This ID already exists");
		}
		public idAlreadyExistsException(string entity) : base(entity) {
			Console.WriteLine($"This ID already is assigned to an existing {entity}");
		}
		public idAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
		protected idAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class DalConfigException : Exception
	{
		public DalConfigException(string msg) : base(msg) { }
		public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
	}
}
