namespace ExerciseTracker.Study.Models.DTO
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; }
        public string ResponseMethod { get; set; }
        public string Message { get; set; }
        public List<T>? Data { get; set; }
        public ResponseDto<T> CreateResponse(bool IsSuccess, string Message, string method, List<T>? Data)
        {
            return new ResponseDto<T>
            {
                IsSuccess = IsSuccess,
                Message = Message,
                ResponseMethod = method,
                Data = Data
            };
        }
    }
}
