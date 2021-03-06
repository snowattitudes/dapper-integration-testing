﻿using System;
using System.Collections.Generic;
using System.Linq;

using Dapper;

using DapperTesting.Core.Model;

namespace DapperTesting.Core.Data
{
    public class DapperPostRepository : DapperRepositoryBase, IPostRepository
    {
        public DapperPostRepository(IConnectionFactory connectionFactory, string connectionStringName)
            : base(connectionFactory, connectionStringName)
        {
        }

        public void Create(Post post)
        {
            var date = DateTime.Now;
            const string sql = "INSERT INTO [Posts] ([OwnerId], [Title], [Slug], [PostedDate], [EditedDate], [Deleted]) OUTPUT inserted.[Id] VALUES(@ownerId, @title, @slug, @postedDate, @editedDate, @deleted)";

            var id = Fetch(c => c.Query<int>(sql, new
            {
                ownerId = post.OwnerId,
                title = post.Title,
                slug = post.Slug,
                postedDate = date,
                editedDate = date,
                deleted = post.Deleted
            })).Single();

            post.Id = id;
            post.PostedDate = date;
            post.EditedDate = date;
        }

        public void AddDetail(PostDetail detail)
        {
            const string sql = "INSERT INTO [PostDetails] ([PostId], [SequenceNumber], [Text]) OUTPUT inserted.[Id] VALUES(@postId ,@sequenceNumber, @text)";

            var id = Fetch(c => c.Query<int>(sql, new
            {
                postId = detail.PostId,
                sequenceNumber = detail.SequenceNumber,
                text = detail.Text
            })).Single();

            detail.Id = id;
        }

        public bool Delete(int postId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDetail(int detailId)
        {
            throw new NotImplementedException();
        }

        public List<Post> GetPostsForUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Post Get(int id)
        {
            const string sql = "SELECT * FROM [Posts] WHERE [Id] = @postId";

            var post = Fetch(c => c.Query<Post>(sql, new { postId = id })).SingleOrDefault();

            return post;
        }

        public PostDetail GetDetail(int postId, int sequenceNumber)
        {
            const string sql = "SELECT * FROM [PostDetails] WHERE [PostId] = @postId AND [SequenceNumber] = @sequenceNumber";

            var details = Fetch(c => c.Query<PostDetail>(sql, new { postId, sequenceNumber })).SingleOrDefault();

            return details;
        }

        public bool Update(Post post)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDetail(PostDetail detail)
        {
            throw new NotImplementedException();
        }
    }
}
