namespace DalApi
{
	public interface ICrud<T>
	{
		public int Create(T entity);
		public T Read(int entityId);
		public IEnumerable<T> ReadAll();
		public void Update(T entity);
		public void Delete(int entityId);
	}
}
