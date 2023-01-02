using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace FlipsideWeShare.Configuration;
public class ShareOptions : Option
{
    [Required]
    [MinLength(6)]
    public string Secret { get; set; } = String.Empty;
}
