namespace BlApi
{
	public interface ICrud<T> where T : struct
	{
		public int Create(T entity);
		public T Read(int entityId);
		public T ReadWithFilter(Func<T?, bool>? filter);
		public IEnumerable<T?> ReadAllFiltered(Func<T?, bool>? filter = null);
		public void Update(T entity);
		public void Delete(int entityId);
	}
}
