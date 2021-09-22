namespace DTO
{
    public class PaginationParams
    {
        private const int _manxItemsPerPage = 50;
        private int itemsPerPage;

        public int Page { get; set; } = 1;
        public int ItemsPerPage
        {
            get => itemsPerPage;
            set => itemsPerPage = value > _manxItemsPerPage ? _manxItemsPerPage : value;
        }
    }
}
