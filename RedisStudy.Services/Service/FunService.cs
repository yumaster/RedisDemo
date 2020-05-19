using Autofac;
using RedisCommon;
using RedisStudy.Common;
using RedisStudy.DAL.Abstraction.Models;
using RedisStudy.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisStudy.Services.Service
{
    public class FunService
    {
        private readonly IEFRepository<Function> _funRepository = DIContainer.Container.Resolve<IEFRepository<Function>>();
        private RedisHelper redis = new RedisHelper();
        #region 获取数据
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Function GetEntity(string id)
        {
            var fun = redis.HashGet<Function>("FunCache", "FunCache" + id);
            if (fun == null)//如果Redis中不存在，则从数据库中读取
            {
                fun = _funRepository.GetById(id);
                bool ret = redis.HashSet<Function>("FunCache", "FunCache" + fun.Id, fun);
                return fun;
            }
            else
            {
                return fun;
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Function> GetList()
        {
            var cacheDict = redis.HashKeys<Function>("FunCache");
            if (cacheDict == null || cacheDict.Count == 0)//如果Redis中不存在，则从数据库中读取
            {
                var list = _funRepository.Table.ToList();
                foreach (var item in list)
                {
                    bool ret = redis.HashSet<Function>("FunCache", "FunCache" + item.Id, item);
                }
                return list;
            }
            else
            {
                return cacheDict;
            }
        }
        #endregion

        #region 提交数据
        public void SaveForm(Function entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                _funRepository.Add(entity);
                int ret = _funRepository.Context.Commit();//首先变更数据库  增加一条记录
                bool bret = redis.HashSet("FunCache", "FunCache" + entity.Id, entity);

            }
            else
            {
                //更新
                _funRepository.Update(entity);
                int ret = _funRepository.Context.Commit();
                bool bret = redis.HashSet("FunCache", "FunCache" + entity.Id, entity);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldList"></param>
        /// <returns></returns>
        public void DeleteForm(string id)
        {
            _funRepository.Remove(_funRepository.GetById(id));
            int ret = _funRepository.Context.Commit();
            if (ret == 1)
            {
                redis.HashDelete("FunCache", "FunCache" + id);
            }
        }
        #endregion
    }
}
