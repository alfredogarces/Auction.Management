using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Management.Domain.Exceptions.Auction
{
    public class AuctionAlreadyEndedException : InvalidOperationException
    {
        public AuctionAlreadyEndedException()
            : base("Auction has already ended.") { }
    }
}
