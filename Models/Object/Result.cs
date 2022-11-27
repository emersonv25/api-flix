using System.Drawing.Printing;

namespace Api.MyFlix.Models.Object
{
    public class Result
    {
        public Result() { }

        public Result(List<dynamic> results, int totalResult, int currentPage, int pageSize) 
        {
            TotalResults = totalResult;
            CurrentPage = currentPage;
            ItemsPerPage = pageSize;
            TotalPages = (int)Math.Ceiling(totalResult / (double)pageSize);
            HasPreviousPage = currentPage > 1;
            HasNextPage = currentPage < TotalPages;
            Results = results;
        }

        public List<object> Results { get; set; }
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
