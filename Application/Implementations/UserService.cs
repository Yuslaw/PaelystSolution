using Microsoft.AspNetCore.Hosting;
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
            var attached = documented.Datas
            .Select(dto => new Document
            {

                Title = dto.Name,
                DocumentSize = dto.Filesize,
                DocumentType = dto.FileType,
                DocumentStream = dto.Name,
                DocumentPath= dto.Extension,
                
            })
            .ToList();


            var attachmentList3 = new List<SendSmtpEmailAttachment>();


            foreach (var document in documented.Datas)
            {

                var mail = new SendSmtpEmailAttachment
                {
                    Name = document.Title,

                    Content = File.ReadAllBytes(Path.Combine(_webroot.WebRootPath, "Uploads", document.Name)),

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
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,

                
                

            };

            await _userRepository.Create(user);

            var mailrequest = new MailRequest
            {
                Subject = user.UserName,
                ToEmail = user.UserEmail,
                ToName = user.UserName,
                HtmlContent = $"<html><body><h1>hello {user.UserName}, welcome to paelyst solution.</h1><h4>here are your check info details {user.UserId} and your mail is {user.UserEmail}</h4></body></html>",
                Attachments = attachmentList3,

            };
            _mailService.SendEMailAsync(mailrequest);

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


        public DocumentsResponseModel CreateDocument(IList<IFormFile> documents)
        {
            var fileInfos = new List<DocumentDto>();
            foreach (var item in documents)
            {
                var fileinfo =  UploadFileToSystem(item);
                fileInfos.Add(fileinfo.Data);
            }
            return new DocumentsResponseModel
            {
                Status = true,
                Message = "File successfully Saved",
                Datas = fileInfos,
            };
        }

        public DocumentResponseModel UploadFileToSystem(IFormFile formFile)
        {
            string path = Path.Combine(_webroot.WebRootPath, "Uploads");

            if (formFile is null || formFile.Length is 0)
            {
                var response = new DocumentResponseModel
                {
                    Status = false,
                    Message = "file not found",
                };
                return response;
            }

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);



            
            var fileType = formFile.ContentType;
            var fileExtension = Path.GetExtension(formFile.FileName);
            var fileSizeInKb = formFile.Length / 1024;
            string fileName = Path.GetFileName(formFile.FileName);
            var fileWithoutName = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(fileWithoutName, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            var fileSystemReponse = new DocumentResponseModel
            {
                Status = true,
                Message = "file successfully uploaded",
                Data = new DocumentDto
                {
                    Extension = fileExtension,
                    FileType = fileType,
                    Name = fileName,
                    Title = fileWithoutName,
                    Filesize = fileSizeInKb,
                },
            };
            return fileSystemReponse;
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
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,

                    Documents= user.Documents.ToList(),
                }
            };
        }
    }
}
