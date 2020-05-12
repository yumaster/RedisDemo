using Autofac;
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

        #region 获取数据
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetEntity(string id)
        {
            //var user = _userRepository.Table.SingleOrDefault(x => x.Id == id);
            var user = _userRepository.GetById(id);
            return user;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<User> GetList()
        {
            List<User> userList = _userRepository.Table.ToList();
            return userList;
        }
        #endregion

        #region 提交数据
        public void SaveForm(User entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                //new UserCache().Remove();
                entity.Id = Guid.NewGuid().ToString();
                _userRepository.Add(entity);
                int ret = _userRepository.Context.Commit();//首先变更数据库  增加一条记录
                //Thread.Sleep(500);
                //new UserCache().Remove();
                new UserCache().SetAddEntityToList(entity);
            }
            else
            {
                //new UserCache().Remove();
                //更新
                _userRepository.Update(entity);
                int ret = _userRepository.Context.Commit();
                //Thread.Sleep(500);
                //new UserCache().Remove();
                new UserCache().SetAddEntityToList(entity);
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
                new UserCache().Remove();
            }
        }
        #endregion
    }
}
