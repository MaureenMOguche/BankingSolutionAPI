using AutoMapper;
using BS.Application.Features.Queries.Transactions.GetAll;
using BS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.MappingProfiles
{
    public class TransactionsMapping : Profile
    {
        public TransactionsMapping()
        {
            CreateMap<Transaction, TransactionsDto>()
                .ForMember(dest => dest.From, o => o.MapFrom(x => x.SenderAccount.AccountNumber))
                .ForMember(j => j.To, o => o.MapFrom(x => x.ReceiverAccount.AccountNumber))
                .ForMember(d => d.DateTime, o => o.MapFrom(x => x.DateTime))
                .ForMember(d => d.Amount, o => o.MapFrom(x => x.Amount))
                .ForMember(d => d.Description, o => o.MapFrom(x => x.Description));
        }
    }
}
