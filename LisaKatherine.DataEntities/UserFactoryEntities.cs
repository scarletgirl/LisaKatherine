namespace LisaKatherine.DataEntitiesRepository
{
    using System;
    using System.Collections.Generic;
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
            Users user = (from u in this.dataModel.Users1 where u.userId == userId select u).First();

            return new User(userId, user.username, user.password, user.firstname, user.lastname);
        }

        public IEnumerable<IUser> GetList()
        {
            return
                Enumerable.Cast<IUser>(
                    this.dataModel.Users1.Select(
                        user => new User(user.userId, user.username, user.password, user.firstname, user.lastname)))
                          .ToList();
        }

        public void Update(IUser user)
        {
            Users originalUser = (from u in this.dataModel.Users1 where u.userId == user.UserId select u).First();

            this.dataModel.ApplyCurrentValues(originalUser.EntityKey.EntitySetName, user);
            this.dataModel.SaveChanges();
        }

        public void Add(IUser user)
        {
            user.UserId = Guid.NewGuid();
            this.dataModel.AddToUsers1(
                new Users
                    {
                        firstname = user.FirstName,
                        lastname = user.LastName,
                        password = user.Password,
                        userId = user.UserId,
                        username = user.Username
                    });

            this.dataModel.SaveChanges();
        }

        public void Delete(Guid userId)
        {
            this.dataModel.DeleteObject((from u in this.dataModel.Users1 where u.userId == userId select u).First());
            this.dataModel.SaveChanges();
        }
    }
}