﻿using MediatR;
using System;
using System.Threading.Tasks;
using Jambo.Producer.Application.Commands;
using Jambo.Producer.Application.Commands.Blogs;
using Jambo.Producer.Application.Commands.Posts;
using Jambo.Domain.Model.Blogs;
using Jambo.ServiceBus;

namespace Jambo.Producer.Application.CommandHandlers.Blogs
{
    public class EnableBlogCommandHandler : IAsyncRequestHandler<EnableBlogCommand>
    {
        private readonly IBusWriter _serviceBus;
        private readonly IBlogReadOnlyRepository _blogReadOnlyRepository;

        public EnableBlogCommandHandler(
            IBusWriter serviceBus,
            IBlogReadOnlyRepository blogReadOnlyRepository)
        {
            _serviceBus = serviceBus ??
                throw new ArgumentNullException(nameof(serviceBus));
            _blogReadOnlyRepository = blogReadOnlyRepository ??
                throw new ArgumentNullException(nameof(blogReadOnlyRepository));
        }

        public async Task Handle(EnableBlogCommand message)
        {
            Blog blog = await _blogReadOnlyRepository.GetBlog(message.Id);
            blog.Enable();

            await _serviceBus.Publish(blog.GetEvents(), message.CorrelationId);
        }
    }
}