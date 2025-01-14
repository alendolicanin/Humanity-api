using Humanity.Domain.Models;
using Humanity.Infrastructure;
using Humanity.Repository.Interfaces;

namespace Humanity.Repository.Repositories
{
    public class ReceiptRepository : Repository<Receipt, int>, IReceiptRepository
    {
        public ReceiptRepository(PlutoContext context) : base(context)
        {
        }
    }
}
