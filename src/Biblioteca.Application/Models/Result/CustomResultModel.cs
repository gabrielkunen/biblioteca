namespace Biblioteca.Application.Models.Result
{
    public class CustomResultModel<T>
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public T Data { get; private set; }
        public CustomErrorModel Error { get; private set; }

        private CustomResultModel(T value)
        {
            Data = value;
            IsSuccess = true;
            Error = CustomErrorModel.None;
        }
        private CustomResultModel(CustomErrorModel error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static CustomResultModel<T> Success(T value)
        {
            return new CustomResultModel<T>(value);
        }
        public static CustomResultModel<T> Failure(CustomErrorModel error)
        {
            return new CustomResultModel<T>(error);
        }
    }
}
