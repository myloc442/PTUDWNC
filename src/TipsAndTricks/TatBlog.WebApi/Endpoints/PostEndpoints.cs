﻿using Mapster;
using MapsterMapper;
using System.Collections.Generic;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Post;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostsEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");
            routeGroupBuilder.MapGet("/", GetPosts)
               .WithName("GetPosts")
               .Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapGet("/featured/{limit:int}", GetPopularArticle)
                .WithName("GetPopularArticle")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
                .WithName("GetRandomPosts")
                .Produces<ApiResponse<IList<PostDto>>>();

            return app;
        }
        /// <summary>
        /// Lấy danh sách bài viết. Hỗ trợ tìm theo từ khóa, chuyên mục, tác giả, ngày đăng, … và phân trang kết quả.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryRepository"></param>
        /// <returns></returns>
        private static async Task<IResult> GetPosts(
            [AsParameters] PostFilterModel model,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            // Tạo điều kiện truy vấn
            // Vì để field Published là optional nên cần check trường hợp là null để gán phù hợp
            if (model.Published == null) { model.Published = true; }
            var postQuery = mapper.Map<PostQuery>(model);

            var posts = await blogRepository.GetPagedPostsByQueryAsync<PostDto>(
                posts => posts.ProjectToType<PostDto>(),
                postQuery,
                pagingModel);

            var paginationResult = new PaginationResult<PostDto>(posts);
            return Results.Ok(paginationResult);
        }

        /// <summary>
        /// Lấy danh sách N (limit) bài viết nhiều người đọc nhất.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="blogRepository"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        private static async Task<IResult> GetPopularArticle(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetPopularArticleAsync(limit);

            return Results.Ok(value: mapper.Map<IList<PostDto>>(posts));
        }

        /// <summary>
        /// Lấy ngẫu nhiên một danh sách N (limit) bài viết.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="blogRepository"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        private static async Task<IResult> GetRandomPosts(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetRandomsPostsAsync(limit);

            return Results.Ok(value: mapper.Map<IList<PostDto>>(posts));
        }

        //private static async Task<IResult> GetPostsInMonthly(
        //    int limit,
        //    IBlogRepository blogRepository)
        //{

        //}

    }
}
