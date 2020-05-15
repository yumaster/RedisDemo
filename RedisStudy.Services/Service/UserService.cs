using Autofac;
using RedisCommon;
using RedisStudy.Common;
using RedisStudy.DAL.Abstraction.Models;
using RedisStudy.DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Threading;

namespace RedisStudy.Services
{
    public class UserService
    {
        private readonly IEFRepository<User> _userRepository = DIContainer.Container.Resolve<IEFRepository<User>>();
        private RedisHelper redis = new RedisHelper();
        #region 获取数据
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetEntity(string id)
        {
            //var user = _userRepository.Table.SingleOrDefault(x => x.Id == id);
            //var user = _userRepository.GetById(id);
            //return user;

            var user = redis.HashGet<User>("UserCache", "UserCahce" + id);
            if (user == null)//如果Redis中不存在，则从数据库中读取
            {
                user = _userRepository.GetById(id);
                bool ret = redis.HashSet<User>("UserCahce", "UserCahce" + user.Id,user);
                return user;
            }
            else
            {
                return user;
            }

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<User> GetList()
        {
            //List<User> userList = _userRepository.Table.ToList();
            //return userList;
            var cacheDict = redis.HashKeys<User>("UserCache");
            if (cacheDict == null || cacheDict.Count == 0)//如果Redis中不存在，则从数据库中读取
            {
                var list = _userRepository.Table.ToList();
                foreach (var item in list)
                {
                    //int ret = CacheFactory.Cache.SetHashFieldCache<User>(CacheKey, CacheKey + item.Id, item);
                    bool ret = redis.HashSet<User>("UserCache", "UserCache" + item.Id, item);
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
        public void SaveForm(User entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                _userRepository.Add(entity);
                int ret = _userRepository.Context.Commit();//首先变更数据库  增加一条记录
                bool bret = redis.HashSet("UserCache", "UserCache" + entity.Id, entity);
                
            }
            else
            {
                //更新
                _userRepository.Update(entity);
                int ret = _userRepository.Context.Commit();
                bool bret = redis.HashSet("UserCache", "UserCache" + entity.Id, entity);
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
            _userRepository.Remove(_userRepository.GetById(id));
            int ret = _userRepository.Context.Commit();
            if(ret==1)
            {
                //new UserCache().Remove();
                redis.HashDelete("UserCache", "UserCache" + id);
            }
        }
        #endregion
    }
}
