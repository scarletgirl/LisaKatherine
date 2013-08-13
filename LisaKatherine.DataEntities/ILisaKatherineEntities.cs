namespace LisaKatherine.DataEntitiesRepository
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq.Expressions;

    public interface ILisaKatherineEntities<TEntity>
    {
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<ArticleEntity> Articles1 { get; }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<Photos> tblPhotos { get; }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<UserEntity> Users1 { get; }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<ArticleTypeEntity> ArticleTypes1 { get; }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<PublishedArticleEntity> PublishedArticles { get; }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<FlickrSets> FlickrSets { get; }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<Section> Sections { get; }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<FacebookPost> FacebookPosts { get; }

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        ObjectSet<FacebookUser> FacebookUsers { get; }

        DbConnection Connection { get; }

        string DefaultContainerName { get; set; }

        MetadataWorkspace MetadataWorkspace { get; }

        ObjectStateManager ObjectStateManager { get; }

        int? CommandTimeout { get; set; }

        ObjectContextOptions ContextOptions { get; }

        /// <summary>
        /// Deprecated Method for adding a new object to the Articles1 EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        void AddToArticles1(ArticleEntity articles);

        /// <summary>
        /// Deprecated Method for adding a new object to the tblPhotos EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        void AddTotblPhotos(Photos photos);

        /// <summary>
        /// Deprecated Method for adding a new object to the Users1 EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        void AddToUsers1(UserEntity users);

        /// <summary>
        /// Deprecated Method for adding a new object to the ArticleTypes1 EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        void AddToArticleTypes1(ArticleTypeEntity articleTypes);

        /// <summary>
        /// Deprecated Method for adding a new object to the PublishedArticles EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        void AddToPublishedArticles(PublishedArticleEntity publishedArticles);

        /// <summary>
        /// Deprecated Method for adding a new object to the FlickrSets EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        void AddToFlickrSets(FlickrSets flickrSets);

        /// <summary>
        /// Deprecated Method for adding a new object to the Sections EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        void AddToSections(Section section);

        /// <summary>
        /// Deprecated Method for adding a new object to the FacebookPosts EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        void AddToFacebookPosts(FacebookPost facebookPost);

        /// <summary>
        /// Deprecated Method for adding a new object to the FacebookUsers EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
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

        ObjectResult ExecuteStoreQuery(
            string commandText, string entitySetName, MergeOption mergeOption, params object[] parameters);

        ObjectResult Translate(DbDataReader reader);

        ObjectResult Translate(DbDataReader reader, string entitySetName, MergeOption mergeOption);

        void CreateDatabase();

        void DeleteDatabase();

        bool DatabaseExists();

        string CreateDatabaseScript();

        event EventHandler SavingChanges;

        event ObjectMaterializedEventHandler ObjectMaterialized;
    }
}