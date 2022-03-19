using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Domain;
using System;
using System.Collections.Generic;

namespace Minitwit_BE.Test
{
    internal class TestHelper
    {
        internal static IEnumerable<Message> GetMessages()
        {
            return  new List<Message>
            {
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    MessageId = 1,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = 2,
                    Flagged = false,
                    MessageId = 2,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = 3,
                    Flagged = false,
                    MessageId = 3,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = 4,
                    Flagged = false,
                    MessageId = 4,
                    PublishDate = DateTime.Now,
                    Text = "text"
                }
            };
        }

        internal static IEnumerable<Message> GetMessagesWithUserId(int id)
        {
            return new List<Message>
            {
                new Message
                {
                    AuthorId = id,
                    Flagged = false,
                    MessageId = 1,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = id,
                    Flagged = false,
                    MessageId = 2,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = id,
                    Flagged = false,
                    MessageId = 3,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = id,
                    Flagged = false,
                    MessageId = 4,
                    PublishDate = DateTime.Now,
                    Text = "text"
                }
            };
        }

        internal static IEnumerable<Follower> GetFollowersWithWhoUserId(int id)
        {
            return new List<Follower>
            {
                new Follower
                {
                    WhoId = id,
                    WhomId = id+1,
                },
                new Follower
                {
                    WhoId = id,
                    WhomId = id+2,
                },
                new Follower
                {
                    WhoId = id,
                    WhomId = id+3,
                }
            };
        }

        internal static ControllerContext CreateHttpContext()
        {
            var httpcontext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            httpcontext.HttpContext.Request.Headers.Add("Authorization", "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh");

            return httpcontext;
        }
    }
}
