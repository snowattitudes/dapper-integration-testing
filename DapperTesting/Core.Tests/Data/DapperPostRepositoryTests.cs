﻿using System;
using System.Collections.Generic;
using System.Linq;
using DapperTesting.Core.Data;
using DapperTesting.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DapperTesting.Core.Tests.Data
{
    [TestClass]
    public class DapperPostRepositoryTests : TestBase
    {
        private DapperPostRepositoryTestContext _c;

        [TestMethod]
        public void When_a_new_post_is_created_the_values_are_stored_in_the_database()
        {
            var testUser = _c.CreateTestUser();

            var repository = _c.GetRepository();
            var post = new Post
            {
                OwnerId = testUser.Id,
                Title = "This is my post title",
                Slug = "1-this-is-my-post-title"
            };

            repository.Create(post);

            var createdPost = repository.Get(post.Id);

            Assert.AreEqual(post.Title, createdPost.Title);
            Assert.AreEqual(post.Slug, createdPost.Slug);
            Assert.AreEqual(post.OwnerId, createdPost.OwnerId);
            Assert.AreEqual(post.Deleted, createdPost.Deleted);
        }

        [TestMethod]
        public void When_a_new_post_is_created_as_deleted_the_deleted_value_is_stored_in_the_database()
        {
            var testUser = _c.CreateTestUser();

            var repository = _c.GetRepository();
            var post = new Post
            {
                OwnerId = testUser.Id,
                Title = "This is my post title",
                Slug = "1-this-is-my-post-title",
                Deleted = true
            };

            repository.Create(post);

            var createdPost = repository.Get(post.Id);

            Assert.AreEqual(post.Deleted, createdPost.Deleted);
        }

        [TestMethod]
        public void When_a_new_post_is_created_the_posted_date_and_edited_dates_are_updated()
        {
            var testUser = _c.CreateTestUser();

            var repository = _c.GetRepository();
            var post = new Post
            {
                OwnerId = testUser.Id,
                Title = "This is my post title",
                Slug = "1-this-is-my-post-title"
            };

            var before = _c.RoundToSecond(DateTime.Now);
            repository.Create(post);
            var after = _c.RoundToSecond(DateTime.Now);

            var posted = _c.RoundToSecond(post.PostedDate);
            var edited = _c.RoundToSecond(post.EditedDate);

            Assert.IsTrue(before <= posted && posted <= after);
            Assert.IsTrue(before <= edited && edited <= after);
        }

        [TestInitialize]
        public void Init()
        {
            Start();

            _c = new DapperPostRepositoryTestContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_c != null)
            {
                _c.Dispose();
            }

            End();
        }

        private class DapperPostRepositoryTestContext : TestContextBase
        {
            private const string UserConnectionStringName = "UserConnectionString";
            private const string PostConnectionStringName = "PostConnectionString";

            private IUserRepository GetUserRepository()
            {
                var connectionFactory = CreateConnectionFactory(UserConnectionStringName);

                return new DapperUserRepository(connectionFactory, UserConnectionStringName);
            }

            public IPostRepository GetRepository()
            {
                var connectionFactory = CreateConnectionFactory(PostConnectionStringName);

                return new DapperPostRepository(connectionFactory, PostConnectionStringName);
            }

            public User CreateTestUser()
            {
                var repository = GetUserRepository();

                var user = CreateStandardUser(1000);

                repository.Create(user);

                return user;
            }
        }
    }
}
