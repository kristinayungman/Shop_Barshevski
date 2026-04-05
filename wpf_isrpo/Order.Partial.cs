namespace UserApp
{
    public partial class Order
    {
        public string timestamp_text => timestamp.ToString("g");

        public string status_text => OrderStatuses.Label(status);
    }
}
