using System.Net;
using System.Net.Http;
using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Http;
using DevexpApiSdk.Messages;
using DevexpApiSdk.Messages.ApiResponseDtos;
using DevexpApiSdk.Messages.Models;
using DevexpApiSdk.Metrics;
using Moq;
using NUnit.Framework;

namespace MyApiSdk.Tests.Messages
{
    [TestFixture]
    public class MessagesClientTests
    {
        private Mock<IDevexpApiHttpClient> _httpMock;
        private DevexpApiOptions _options;
        private MessagesClient _client;
        private IOperationExecutor _executionWrapper = new NoMetricsOperationExecutor();

        [SetUp]
        public void SetUp()
        {
            _httpMock = new Mock<IDevexpApiHttpClient>();
            _options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("there-is-no-key")
                .Build();
            _client = new MessagesClient(_httpMock.Object, _options, _executionWrapper);
        }

        [Test]
        public async Task GetMessagesAsync_ShouldReturnPagedResult()
        {
            var dto = new GetMessagesResponseDto
            {
                Messages = new List<MessageDto>
                {
                    new MessageDto
                    {
                        Id = Guid.NewGuid(),
                        Content = "Hello",
                        From = "me",
                        To = Guid.NewGuid(),
                        Status = "failed",
                        CreatedAt = DateTime.UtcNow
                    }
                },
                Page = 1,
                QuantityPerPage = 10
            };

            var response = new DevexpApiResponse<GetMessagesResponseDto>(dto, HttpStatusCode.OK);

            _httpMock
                .Setup(h =>
                    h.SendAsync<GetMessagesResponseDto>(
                        HttpMethod.Get,
                        It.Is<string>(p => p.StartsWith("/messages")),
                        null,
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(response);

            var result = await _client.GetMessagesAsync();

            Assert.That(result.Items.Count, Is.EqualTo(1));
            Assert.That(result.Items.First().Content, Is.EqualTo("Hello"));
            Assert.That(result.Items.First().Status, Is.EqualTo(MessageStatus.FAILED));
        }

        [Test]
        public async Task GetMessageByIdAsync_ShouldReturnMessage()
        {
            var id = Guid.NewGuid();
            var expectedMessage = new Message { Id = id, Content = "ById" };
            var response = new DevexpApiResponse<Message>(expectedMessage, HttpStatusCode.OK);

            _httpMock
                .Setup(h =>
                    h.SendAsync<Message>(
                        HttpMethod.Get,
                        $"/messages/{id}",
                        null,
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(response);

            var result = await _client.GetMessageByIdAsync(id);

            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Content, Is.EqualTo("ById"));
        }

        [Test]
        public async Task SendMessageAsync_WithContactId_ShouldPostMessage()
        {
            var contactId = Guid.NewGuid();
            var expectedMessage = new Message { Id = Guid.NewGuid(), Content = "Sent" };
            var response = new DevexpApiResponse<Message>(expectedMessage, HttpStatusCode.Created);

            _httpMock
                .Setup(h =>
                    h.SendAsync<Message>(
                        HttpMethod.Post,
                        "/messages",
                        It.IsAny<CreateMessageRequest>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(response);

            var result = await _client.SendMessageAsync("me", "hi", contactId);

            Assert.That(result.Content, Is.EqualTo("Sent"));
        }

        [Test]
        public async Task SendMessageAsync_WithContact_ShouldPostMessage()
        {
            var contact = new Contact { Id = Guid.NewGuid(), Name = "John" };
            var expectedMessage = new Message { Id = Guid.NewGuid(), Content = "Hello John" };
            var response = new DevexpApiResponse<Message>(expectedMessage, HttpStatusCode.Created);

            _httpMock
                .Setup(h =>
                    h.SendAsync<Message>(
                        HttpMethod.Post,
                        "/messages",
                        It.IsAny<CreateMessageRequest>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(response);

            var result = await _client.SendMessageAsync("me", "Hello John", contact);

            Assert.That(result.Content, Is.EqualTo("Hello John"));
        }

        [Test]
        public async Task SendMessageAsync_WithMultipleContacts_NoBulk_ShouldSendSequentially()
        {
            var contact1 = new Contact { Id = Guid.NewGuid(), Name = "A" };
            var contact2 = new Contact { Id = Guid.NewGuid(), Name = "B" };

            var callCount = 0;

            _httpMock
                .Setup(h =>
                    h.SendAsync<Message>(
                        HttpMethod.Post,
                        "/messages",
                        It.IsAny<CreateMessageRequest>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(() =>
                {
                    callCount++;
                    var msg = new Message { Id = Guid.NewGuid(), Content = $"Msg{callCount}" };
                    return new DevexpApiResponse<Message>(msg, HttpStatusCode.Created);
                });

            var results = await _client.SendMessageAsync("me", "hi", new[] { contact1, contact2 });

            Assert.That(results.Count, Is.EqualTo(2));
            Assert.That(results[0].Content, Is.EqualTo("Msg1"));
            Assert.That(results[1].Content, Is.EqualTo("Msg2"));
        }
    }
}
