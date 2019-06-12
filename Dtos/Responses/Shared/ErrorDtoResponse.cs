namespace WebApiDapperCrud.Dtos.Responses.Shared
{
    public class ErrorDtoResponse : AppResponse
    {
        public ErrorDtoResponse() : base(false)
        {
        }

        public ErrorDtoResponse(string message) : base(false, message)
        {
        }
    }
}