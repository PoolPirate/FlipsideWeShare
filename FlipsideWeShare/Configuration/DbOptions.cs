using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace FlipsideWeShare.Configuration;
public class DbOptions : Option
{
    [Required]
    public string ConnectionString { get; set; } = String.Empty;
}
