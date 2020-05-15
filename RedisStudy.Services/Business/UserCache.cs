using RedisStudy.DAL.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RedisStudy.Services
{
    public class UserCache:BaseBusinessCache<User>
    {
        private UserService demoService = new UserService();

        public override string CacheKey => this.GetType().Name;
        public override  List<User> GetList()
        {
            var cacheDict = CacheFactory.Cache.GetHashToListCache<User>(CacheKey);
            if (cacheDict == null || cacheDict.Count == 0)//如果Redis中不存在，则从数据库中读取
            {
                var list = demoService.GetList();
                foreach(var item in list)
                {
                    int ret=CacheFactory.Cache.SetHashFieldCache<User>(CacheKey, CacheKey + item.Id, item);
                }
                return list;
            }else
            {
                return cacheDict;
            }



            //从Redis中读取数据

            //var cacheList = CacheFactory.Cache.GetCache<List<User>>(CacheKey);
            //if (cacheList == null||cacheList.Count==0)//如果Redis中不存在，则从数据库中读取
            //{
            //    var list = demoService.GetList();
            //    CacheFactory.Cache.SetCache(CacheKey, list);
            //    return list;
            //}
            //else//否则从Redis中读
            //{
            //    return cacheList;
            //}
        }


        public bool SetAddEntityToList(User user)
        {
            //var cacheList = CacheFactory.Cache.GetCache<List<User>>(CacheKey);
            //cacheList.Add(user);
            //return CacheFactory.Cache.SetCache(CacheKey, cacheList);



            int ret=CacheFactory.Cache.SetHashFieldCache<User>(CacheKey, CacheKey + user.Id, user);
            if (ret == 1)
                return true;
            else
                return false;

        }
    }
}
