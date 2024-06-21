namespace CyanBlog.DbAccess.Interface
{
    /// <summary>
    /// 数据库表访问接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IDao<T>
    {
        /// <summary>
        /// 添加实体到表中
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>是否添加成功</returns>
        public bool Add(T entity);

        /// <summary>
        /// 根据目标实体，在表中删除记录
        /// </summary>
        /// <param name="entity">待删除的实体</param>
        /// <returns>是否删除成功</returns>
        public bool Delet(T entity);

        /// <summary>
        /// 根据目标实体，更新表中的实体
        /// </summary>
        /// <param name="entity">待更新的实体</param>
        /// <returns>是否更新成功</returns>
        public bool Update(T entity);

        /// <summary>
        /// 根据索引删除表中的实体
        /// </summary>
        /// <param name="index">待删除实体的索引</param>
        /// <returns>是否被删除</returns>
        public bool RemoveAt(int index);

        /// <summary>
        /// 根据索引查找对应实体
        /// </summary>
        /// <param name="index">待查找实体的索引</param>
        /// <returns>查找到的实体，如果没找到就返回空</returns>
        public T? Select(int index);

        /// <summary>
        /// 返回表中全部实体
        /// </summary>
        /// <returns>表中的全部实体</returns>
        public List<T> GetAll();

        /// <summary>
        /// 表的记录条数
        /// </summary>
        /// <returns></returns>
        public int Count();
    }
}
