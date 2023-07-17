using PaelystSolution.Application.Dtos;
using PaelystSolution.Application.Interfaces;
using PaelystSolution.Domain.Entities;
using PaelystSolution.Infrastructure.Interfaces;
using PaelystSolution.Infrastructure.Mail;
using sib_api_v3_sdk.Model;

namespace PaelystSolution.Application.Implementations
{
    public class UserService : IUserService, IDocumentService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _webroot;
        public List<Document> documentList = new List<Document>();
        public IList<DocumentDto> documentList2 = new List<DocumentDto>();
        


        public UserService(IUserRepository userRepository, IDocumentRepository documentRepository, IMailService mailService, IWebHostEnvironment webroot)
        {
            _userRepository = userRepository;
            _documentRepository = documentRepository;
            _mailService = mailService;
            _webroot = webroot;
        }

        public async Task<BaseResponse<UserDto>> CheckInfo(LoginUserViewModel model)
        {
            var user = await _userRepository.Get(model.Email);
            var tn = await _userRepository.Get(a => a.UserId == model.UserId);
            if (user == null || tn==null) return new BaseResponse<UserDto>
            {
                Message = "Transaction Number  or email is incorrect",
                Status = false,
            };
            return new BaseResponse<UserDto>
            {
                Message = "Succesful",
                Status = true,
                Data = new UserDto
                {
                    UserId = user.UserId,
                    UserEmail = user.UserEmail,

                    BrowseDocuments = await _documentRepository.GetSelected(a => a.UserId == model.UserId),
                }
            };
        }

        public async Task<BaseResponse<UserViewModel>> Create(UserViewModel model)
        {
            var initiatorExist = await _userRepository.Get(a=> a.UserEmail== model.UserEmail);
            if (initiatorExist != null)
            {
                return new BaseResponse<UserViewModel>
                {
                    Message = "user already exist",
                    Status = false,
                };
            }

            var documented = CreateDocument(model.BrowseDocuments);
            var attached = documented.Data
            .Select(dto => new Document
            {

                Title = dto.Title,
                DocumentSize = dto.Size,
                DocumentStream = dto.DocumentStream,
                DocumentType = dto.Type,
                DocumentPath = dto.DocumentPath,
               
            })
            .ToList();


            var attachmentList3 = new List<SendSmtpEmailAttachment>();


            foreach (var document in attached)
            {

                var path = Path.Combine(_webroot.WebRootPath, "Yuslaw", document.DocumentType);

                var mail = new SendSmtpEmailAttachment
                {
                    Name = document.Title,
                    Content = File.ReadAllBytes(path),
                };
                attachmentList3.Add(mail);

            }


            await _documentRepository.AddRange(attached);
           

            var user = new User
            {
                UserName = model.UserName,
                UserId = Guid.NewGuid(),
                UserEmail = model.UserEmail,
                CreatedOn = DateTime.Now,
                Documents = attached,
                
                

            };

            await _userRepository.Create(user);

            var mailRequest = new MailRequest
            {
                Subject = user.UserName,
                ToEmail = user.UserEmail,
                ToName = user.UserName,
                HtmlContent = $"<html><body><h1>Hello {user.UserName}, Welcome to Paelyst Solution.</h1><h4>Here are your check info details {user.UserId} and your mail is {user.UserEmail}</h4></body></html>",
                Attachments = attachmentList3,

            };
            _mailService.SendEMailAsync(mailRequest);

            return new BaseResponse<UserViewModel>
            {
                Message = "Created successfully",
                Status = true,
                Data = new UserViewModel
                {
                    UserEmail = user.UserEmail,
                    UserId = user.UserId,

                }
            };

        }





        public DocumentsResponse CreateDocument(IList<IFormFile> documents)
        {
            var documentList = new List<DocumentDto>();

            foreach (var item in documents)
            {
                if (item != null)
                {
                    string documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Yuslaw");
                    if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);

                    string documentType = item.FileName;
                    var title = Path.GetFileNameWithoutExtension(item.FileName);
                    var size = item.Length / 1024;
                    var fileName = Path.GetFileName(item.FileName);
                    var destinationFullPath = Path.Combine(documentPath, fileName);

                    using (var fileStream = new FileStream(destinationFullPath, FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                    }

                    using (var dataStream = new MemoryStream())
                    {
                        item.CopyTo(dataStream);

                        var documentDto = new DocumentDto
                        {
                            Size = size,
                            Title = title,
                            Type = documentType,
                            DocumentPath = destinationFullPath,
                            DocumentStream = dataStream.ToArray()
                        };

                        documentList.Add(documentDto);
                    }
                }
            }

            return new DocumentsResponse
            {
                Status = true,
                Message = "We are good to go",
                Data = documentList
            };
        }



        Task<IList<Document>> IDocumentService.GetUserDocuments(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<User>> Get(Guid id)
        {
           
            var user = await _userRepository.Get(a => a.UserId == id);
            if (user == null) return new BaseResponse<User>
            {
                Message = "Transaction Number  or email is incorrect",
                Status = false,
            };
            return new BaseResponse<User>
            {
                Message = "Succesful",
                Status = true,
                Data = new User
                {
                    UserId = user.UserId,
                    UserEmail = user.UserEmail,
                    UserName = user.UserName,

                    Documents= user.Documents.ToList(),
                }
            };
        }
    }
}
