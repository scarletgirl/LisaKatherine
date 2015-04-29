namespace LisaKatherine.Interface
{
    using System;
    using System.Collections.Generic;

    public interface IUserFactory
    {
        IUser Get(Guid userId);

        IEnumerable<IUser> GetList();

        void Update(IUser user);

        void Add(IUser user);

        void Delete(Guid userId);
    }
}