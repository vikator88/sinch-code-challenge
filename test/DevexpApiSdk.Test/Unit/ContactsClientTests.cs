using System.Net;
using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts;
using DevexpApiSdk.Contacts.ApiResponseDtos;
using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Http;
using Moq;

namespace DevexpApiSdk.Tests.Contacts
{
    [TestFixture]
    public class ContactsClientTests
    {
        private Mock<IDevexpApiHttpClient> _httpMock = null!;
        private DevexpApiOptions _options = null!;
        private ContactsClient _sut = null!;

        [SetUp]
        public void Setup()
        {
            _httpMock = new Mock<IDevexpApiHttpClient>();
            _options = new DevexpApiOptionsBuilder().WithApiKey("there-is-no-key").Build();

            _sut = new ContactsClient(_httpMock.Object, _options);
        }

        [Test]
        public async Task GetContactsAsync_ShouldReturnPagedResult()
        {
            // Arrange
            var dto = new ListContactsResponseDto
            {
                ContactList = new List<Contact>
                {
                    new Contact
                    {
                        Id = Guid.NewGuid(),
                        Name = "Victor",
                        Phone = "+34600111222"
                    }
                }.ToArray(),
                PageNumber = 1,
                PageSize = 10
            };

            _httpMock
                .Setup(h =>
                    h.SendAsync<ListContactsResponseDto>(
                        HttpMethod.Get,
                        It.Is<string>(p => p.StartsWith("/contacts")),
                        null,
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(
                    new DevexpApiResponse<ListContactsResponseDto>(dto, HttpStatusCode.OK, "{}")
                );

            // Act
            var result = await _sut.GetContactsAsync(); // pageNumber = 1 by default, pageSize = 10 by default

            // Assert
            Assert.That(result.Items, Has.Count.EqualTo(1));
            Assert.That(result.Items[0].Name, Is.EqualTo("Victor"));
            Assert.That(result.CurrentPage, Is.EqualTo(1));
            Assert.That(result.PageSize, Is.EqualTo(10));
        }

        [Test]
        public async Task AddContactAsync_ShouldReturnCreatedContact()
        {
            // Arrange
            var newContact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = "Victor",
                Phone = "+34666555444"
            };

            _httpMock
                .Setup(h =>
                    h.SendAsync<Contact>(
                        HttpMethod.Post,
                        "/contacts",
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(
                    new DevexpApiResponse<Contact>(newContact, HttpStatusCode.Created, "{}")
                );

            // Act
            var result = await _sut.AddContactAsync("Victor", "+34666555444");

            // Assert
            Assert.That(result.Name, Is.EqualTo("Victor"));
            Assert.That(result.Phone, Is.EqualTo("+34666555444"));
        }

        [Test]
        public async Task DeleteContactAsync_ShouldCallHttpDelete()
        {
            // Arrange
            var contactId = Guid.NewGuid();

            _httpMock
                .Setup(h =>
                    h.SendAsync(
                        HttpMethod.Delete,
                        $"/contacts/{contactId}",
                        null,
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(new DevexpApiResponse(HttpStatusCode.NoContent, "{}"));

            // Act
            await _sut.DeleteContactAsync(contactId);

            // Assert → verificamos que se llamó con DELETE
            _httpMock.Verify(
                h =>
                    h.SendAsync(
                        HttpMethod.Delete,
                        $"/contacts/{contactId}",
                        null,
                        It.IsAny<CancellationToken>()
                    ),
                Times.Once
            );
        }
    }
}
