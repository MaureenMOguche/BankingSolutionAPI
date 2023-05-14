//using BS.Application.Contracts.Persistence;
//using BS.Application.Models;
//using BS.Domain;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BS.Application.Features.Queries.BankUsers.GetAll
//{
//    public class GetAllBankUsersQueryHandler : IRequestHandler<GetAllBankUsersQuery, APIResponse>
//    {
//        private readonly IUnitOfWork _db;
//        private readonly UserManager<BankUser> _userManager;
//        private readonly UserStore _store;

//        public GetAllBankUsersQueryHandler(IUnitOfWork db, UserManager<BankUser> userManager, UserStore store)
//        {
//            this._db = db;
//            this._userManager = userManager;
//            this._store = store;
//        }
//        public Task<APIResponse> Handle(GetAllBankUsersQuery request, CancellationToken cancellationToken)
//        {
//            using (var db = _store.Context)
//            {
//                var bankusers = db.
//            }
            
//        }
//    }
//}
