using IDFCR.Shared.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace IDFCR.Shared.Abstractions;

public interface IPIIRowVersion
{
    public string RowVersion { get; set; }
}