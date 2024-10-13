namespace Project01.Models {
    public class PagedResponse<T> {
        public List<T> data {get;set;}
        public int totalData {get;set;}
        public PagedResponse(List<T> data) {
            this.data = data;
            totalData = data.Count();
        }
    }
}