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

    [Serializable]
    public class dataLayerEntityNotFoundException : Exception
    {
        public dataLayerEntityNotFoundException() : base()
        {
            Console.WriteLine("That doesn't exist");
        }
        public dataLayerEntityNotFoundException(string entity) : base(entity)
        {
            Console.WriteLine($"This {entity} doesn't exist");
        }
        public dataLayerEntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        protected dataLayerEntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }

    [Serializable]
    public class dataLayerIdNotFoundException : Exception
    {
        public dataLayerIdNotFoundException() : base()
        {
            Console.WriteLine("This ID does not exist");
        }
        public dataLayerIdNotFoundException(string entity) : base(entity)
        {
            Console.WriteLine($"This ID does not match any existing {entity}s");
        }
        public dataLayerIdNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        protected dataLayerIdNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }

    [Serializable]
    public class dataLayerIdAlreadyExistsException : Exception
    {
        public dataLayerIdAlreadyExistsException() : base()
        {
            Console.WriteLine("This ID already exists");
        }
        public dataLayerIdAlreadyExistsException(string entity) : base(entity)
        {
            Console.WriteLine($"This ID already is assigned to an existing {entity}");
        }
        public dataLayerIdAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
        protected dataLayerIdAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class blGeneralException : Exception
    {
        public blGeneralException() : base()
        {
            Console.WriteLine("Something went wrong.");
        }
        public blGeneralException(string message, Exception innerException) : base(message, innerException) { }
        protected blGeneralException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
