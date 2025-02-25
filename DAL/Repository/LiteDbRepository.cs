using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    using DAL.Interfaces;
    using LiteDB;

    public abstract class LiteDbRepository<T> : IRepository<T>
    {
        private readonly LiteDatabase _database;
        protected readonly ILiteCollection<T> _collection;

     

        public LiteDbRepository(string databasePath, string collectionName)
        {
            
             _database = new LiteDatabase(new ConnectionString($"Filename={databasePath}; Connection=Shared"));
             

            _collection = _database.GetCollection<T>(collectionName);
        }

        public void Insert(T entity) => _collection.Insert(entity);

        public void Update(T entity) => _collection.Update(entity);
        /// <summary>
        /// Delete by primary key id in LiteDb
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id) => _collection.Delete(id);
        /// <summary>
        /// Get by primary key id in LiteDb
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id) => _collection.FindById(id);

        public IEnumerable<T> GetAll() => _collection.FindAll();
    }

}
