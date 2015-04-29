using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;

namespace LisaKatherine.DataEntitiesRepository
{
    public interface ILisaKatherineEntities<TEntity>
    {
        event EventHandler SavingChanges;
        event ObjectMaterializedEventHandler ObjectMaterialized;
        ObjectSet<ArticleEntity> Articles1 { get; }
        ObjectSet<Photos> TblPhotos { get; }
        ObjectSet<UserEntity> Users1 { get; }
        ObjectSet<ArticleTypeEntity> ArticleTypes1 { get; }
        ObjectSet<PublishedArticleEntity> PublishedArticles { get; }
        ObjectSet<FlickrSets> FlickrSets { get; }
        ObjectSet<Section> Sections { get; }
        ObjectSet<FacebookPost> FacebookPosts { get; }
        ObjectSet<FacebookUser> FacebookUsers { get; }
        DbConnection Connection { get; }
        string DefaultContainerName { get; set; }
        MetadataWorkspace MetadataWorkspace { get; }
        ObjectStateManager ObjectStateManager { get; }
        int? CommandTimeout { get; set; }
        ObjectContextOptions ContextOptions { get; }
        void AddToArticles1(ArticleEntity articles);
        void AddTotblPhotos(Photos photos);
        void AddToUsers1(UserEntity users);
        void AddToArticleTypes1(ArticleTypeEntity articleTypes);
        void AddToPublishedArticles(PublishedArticleEntity publishedArticles);
        void AddToFlickrSets(FlickrSets flickrSets);
        void AddToSections(Section section);
        void AddToFacebookPosts(FacebookPost facebookPost);
        void AddToFacebookUsers(FacebookUser facebookUser);
        void ApplyChanges(string entityName, object currentEntity);
        void AcceptAllChanges();
        void AddObject(string entitySetName, object entity);
        void LoadProperty(object entity, string navigationProperty);
        void LoadProperty(object entity, string navigationProperty, MergeOption mergeOption);
        void LoadProperty(TEntity entity, Expression selector);
        void LoadProperty(TEntity entity, Expression selector, MergeOption mergeOption);
        void ApplyPropertyChanges(string entitySetName, object changed);
        TEntity ApplyCurrentValues(string entitySetName, TEntity currentEntity);
        TEntity ApplyOriginalValues(string entitySetName, TEntity originalEntity);
        void AttachTo(string entitySetName, object entity);
        void Attach(IEntityWithKey entity);
        EntityKey CreateEntityKey(string entitySetName, object entity);
        ObjectQuery CreateQuery(string queryString, params ObjectParameter[] parameters);
        void DeleteObject(object entity);
        void Detach(object entity);
        void Dispose();
        object GetObjectByKey(EntityKey key);
        void Refresh(RefreshMode refreshMode, IEnumerable collection);
        void Refresh(RefreshMode refreshMode, object entity);
        int SaveChanges();
        int SaveChanges(bool acceptChangesDuringSave);
        int SaveChanges(SaveOptions options);
        void DetectChanges();
        bool TryGetObjectByKey(EntityKey key, out object value);
        ObjectResult ExecuteFunction(string functionName, params ObjectParameter[] parameters);
        ObjectResult ExecuteFunction(string functionName, MergeOption mergeOption, params ObjectParameter[] parameters);
        void CreateProxyTypes(IEnumerable<Type> types);
        int ExecuteStoreCommand(string commandText, params object[] parameters);
        ObjectResult ExecuteStoreQuery(string commandText, params object[] parameters);
        ObjectResult ExecuteStoreQuery(string commandText, string entitySetName, MergeOption mergeOption, params object[] parameters);
        ObjectResult Translate(DbDataReader reader);
        ObjectResult Translate(DbDataReader reader, string entitySetName, MergeOption mergeOption);
        void CreateDatabase();
        void DeleteDatabase();
        bool DatabaseExists();
        string CreateDatabaseScript();
    }
}