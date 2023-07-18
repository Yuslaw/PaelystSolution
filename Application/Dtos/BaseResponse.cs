namespace PaelystSolution.Application.Dtos
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class DocumentResponseModel
    {
        public string Message { get; set; }

        public bool Status { get; set; }
        public DocumentDto Data { get; set; }
    }
    public class DocumentsResponseModel
    {
        public string Message { get; set; }

        public bool Status { get; set; }
        public IList<DocumentDto> Datas { get; set; }
    }
    public class DocumentDto
    {
        public string Name { get; set; }
        public long Filesize { get; set; }
        public string FileType { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }
        public byte[] FileStream { get; set; }
    }

    



}
