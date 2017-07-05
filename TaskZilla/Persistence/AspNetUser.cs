namespace TaskZilla.Persistence
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AspNetUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}