using System.Net;

namespace DDHelpers.Api
{
    /// <summary>Representa um retorno do resultado da solicitação de um serviço.</summary>
    public interface IServiceResult
    {
        /// <summary>Obtém se a solicitação foi bem sucedida.</summary>
        bool Succeeded { get; }
        /// <summary>Obtém uma mensagem do resultado da solicitação.</summary>
        string? Message { get; }
        /// <summary>Obtém o código de estado da solicitação.</summary>
        HttpStatusCode StatusCode();
        /// <summary>Obtém o conteúdo da resposta pelo tipo especificado.</summary>
        TData? GetData<TData>() where TData : class;
    }

    /// <summary>Representa um retorno do resultado da solicitação de um serviço.</summary>
    public class ServiceResult : IServiceResult
    {
        private HttpStatusCode _statusCode;
        public bool Succeeded { get; set; }
        public string? Message { get; set; }

        public ServiceResult(bool succeeded, string? message, HttpStatusCode statusCode)
        {
            Succeeded = succeeded;
            Message = message;
            _statusCode = statusCode;
        }

        public HttpStatusCode StatusCode()
        {
            return _statusCode;
        }

        public virtual T? GetData<T>() where T : class
            => null;

        /// <summary>Retorna um status code 200 (Ok).</summary>
        public static ServiceResult Ok()
            => new(true, null, HttpStatusCode.OK);

        /// <summary>Retorna um status code 200 (Ok).</summary>
        public static ServiceResult<T> Ok<T>(T? data) where T : class
            => new(true, null, HttpStatusCode.OK, data);

        /// <summary>Retorna um status code 400 (Bad Request).</summary>
        public static ServiceResult Error(string? errorMessage)
            => new(false, errorMessage, HttpStatusCode.BadRequest);

        /// <summary>Retorna um status code 400 (Bad Request).</summary>
        public static ServiceResult<T> Error<T>(T? data) where T : class
            => new(false, null, HttpStatusCode.BadRequest, data);

        /// <summary>Retorna um status code 500 (Internal Server Error).</summary>
        public static ServiceResult InternalError(string? errorMessage)
            => new(false, errorMessage, HttpStatusCode.InternalServerError);

        /// <summary>Retorna um status code 400 (Bad Request).</summary>
        public static ServiceResult<T> InternalError<T>(T? data) where T : class
            => new(false, null, HttpStatusCode.InternalServerError, data);
    }

    /// <summary>Representa um retorno do resultado da solicitação de um serviço com um tipo específico.</summary>
    public class ServiceResult<T> : ServiceResult where T : class
    {
        public T? Data { get; set; }

        public ServiceResult(bool succeeded, string? message, HttpStatusCode statusCode, T? data)
            : base(succeeded, message, statusCode)
        {
            Data = data;
        }

        public override TData? GetData<TData>() where TData : class
        {
            if (Data is TData tdata)
                return tdata;

            return null;
        }
    }
}
