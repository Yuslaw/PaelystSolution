namespace PaelystSolution.Application.Dtos
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class DocumentResponse: BaseResponse<DocumentDto>
    {
       
    }
    public class DocumentsResponse: BaseResponse<DocumentDto>
    {
        public IList<DocumentDto> Data { get; set; }
    }
    
}
