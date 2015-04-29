using System;
using System.Collections.Generic;
using System.Linq;
using LisaKatherine.Interface;

namespace LisaKatherine.DataEntitiesRepository
{
    public class UserFactoryEntities : IUserFactory
    {
        private readonly LisaKatherineEntities dataModel;

        public UserFactoryEntities(LisaKatherineEntities dataModel)
        {
            this.dataModel = dataModel;
        }

        public UserFactoryEntities()
        {
            dataModel = new LisaKatherineEntities();
        }

        public IUser Get(Guid userId)
        {
            try
            {
                var user = (from u in dataModel.UserEntity where u.userId == userId select u).First();
                return new User(userId, user.username, user.password, user.firstname, user.lastname);
            }
            catch (Exception)
            {
                //throw new Exception("User not found for userID " + userId);
                return new User();
            }
        }

        public IEnumerable<IUser> GetList()
        {
            var users = dataModel.UserEntity;
            return Enumerable.Cast<IUser>(users.Select(u => new User(u.userId, u.username, u.password, u.firstname, u.lastname))).ToList();
        }

        public void Update(IUser user)
        {
            var originalUser = (from u in dataModel.UserEntity where u.userId == user.UserId select u).First();

            dataModel.ApplyCurrentValues(originalUser.EntityKey.EntitySetName, ConvertToUserEntity(user));
            dataModel.SaveChanges();
        }

        public void Add(IUser user)
        {
            user.UserId = Guid.NewGuid();
            dataModel.AddToUserEntity(
                new UserEntity { firstname = user.FirstName, lastname = user.LastName, password = user.Password, userId = user.UserId, username = user.Username });

            dataModel.SaveChanges();
        }

        public void Delete(Guid userId)
        {
            dataModel.DeleteObject((from u in dataModel.UserEntity where u.userId == userId select u).First());
            dataModel.SaveChanges();
        }

        private UserEntity ConvertToUserEntity(IUser user)
        {
            return new UserEntity { firstname = user.FirstName, lastname = user.LastName, password = user.Password, username = user.Username, userId = user.UserId };
        }
    }
}