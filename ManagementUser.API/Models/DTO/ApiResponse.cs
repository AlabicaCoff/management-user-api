namespace ManagementUser.API.Models.DTO
{
    public class ApiResponse<T>
    {
        public ApiStatus? Status { get; set; }
        public T? Data { get; set; }
    }

    public class ApiStatus
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }

    public class ApiResponsePagination<T>
    {
        public T? dataSource { get; set; }
        public int? page { get; set; }
        public int? pageSize { get; set; }
        public int? totalCount { get; set; }
    };

    public class NewUserIdResponse
    {
        public string? NewUserId { get; set; }
    }
}