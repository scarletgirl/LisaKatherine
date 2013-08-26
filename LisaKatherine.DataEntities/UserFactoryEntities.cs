namespace LisaKatherine.DataEntitiesRepository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Objects;
    using System.Linq;

    using LisaKatherine.Interface;

    public class UserFactoryEntities : IUserFactory
    {
        private readonly LisaKatherineEntities dataModel;

        public UserFactoryEntities(LisaKatherineEntities dataModel)
        {
            this.dataModel = dataModel;
        }

        public UserFactoryEntities()
        {
            this.dataModel = new LisaKatherineEntities();
        }

        public IUser Get(Guid userId)
        {
            try
            {
                UserEntity user = (from u in this.dataModel.UserEntity where u.userId == userId select u).First();
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
            ObjectSet<UserEntity> users = this.dataModel.UserEntity;
            var list = new List<IUser>();
            foreach (UserEntity u in users)
            {
                list.Add(new User(u.userId, u.username, u.password, u.firstname, u.lastname));
            }
            return list;
        }

        public void Update(IUser user)
        {
            UserEntity originalUser = (from u in this.dataModel.UserEntity where u.userId == user.UserId select u).First();

            this.dataModel.ApplyCurrentValues(originalUser.EntityKey.EntitySetName, this.ConvertToUserEntity(user));
            this.dataModel.SaveChanges();
        }

        public void Add(IUser user)
        {
            user.UserId = Guid.NewGuid();
            this.dataModel.AddToUserEntity(
                new UserEntity { firstname = user.FirstName, lastname = user.LastName, password = user.Password, userId = user.UserId, username = user.Username });

            this.dataModel.SaveChanges();
        }

        public void Delete(Guid userId)
        {
            this.dataModel.DeleteObject((from u in this.dataModel.UserEntity where u.userId == userId select u).First());
            this.dataModel.SaveChanges();
        }

        private UserEntity ConvertToUserEntity(IUser user)
        {
            return new UserEntity { firstname = user.FirstName, lastname = user.LastName, password = user.Password, username = user.Username, userId = user.UserId };
        }
    }
}