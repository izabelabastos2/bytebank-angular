using Vale.Geographic.Domain.Base.Interfaces;

namespace Vale.Geographic.Application.Base
{
    public class FilterDto : IFilterParameters
    {
        /// <summary>
        ///     Campo de ordenação
        /// </summary>
        private string _sort;

        public FilterDto()
        {
            sort = "Id";
            page = 1;
            per_page = int.MaxValue;
            filter = string.Empty;
        }

        public int page { get; set; }

        public int per_page { get; set; }

        public string sort
        {
            get
            {
                if (_sort.IndexOf('-') == 0)
                    return _sort.Remove(0, 1) + " desc";
                return _sort + " asc";
            }
            set => _sort = value;
        }

        public string filter { get; set; }

    }
}