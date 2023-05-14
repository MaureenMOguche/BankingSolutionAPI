using AutoMapper;
using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using BS.Domain;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Commands.BankUsrs.UpdateUsers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, APIResponse>
    {
        private readonly UserManager<BankUser> _userManager;
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly CloudinarySettings _cloudinarySettings;

        public UpdateUserCommandHandler(UserManager<BankUser> userManager,
            IUnitOfWork db,
            IOptions<CloudinarySettings> cloudinarySettings,
            IMapper mapper)
        {
            this._userManager = userManager;
            this._db = db;
            this._mapper = mapper;
            this._cloudinarySettings = cloudinarySettings.Value;
        }
        public async Task<APIResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.userId);
            if (user == null)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Messages = new() { "User does not exist" }
                };
            }
            
            if (request.UpdateUser == null)
            {
                return new APIResponse
                {
                    isSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Messages = new() { "No changes to be made" }
                };
            }


            user.Address = request.UpdateUser.Address;
            user.City = request.UpdateUser.City;
            user.Region = request.UpdateUser.Region;
            await _db.SaveAsync();

            return new APIResponse
            {
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Messages = new() { "Successfully updated changes" }
            };

        }

        private object UploadToCloudinary(string imagePath)
        {
            var cloudinary = new Cloudinary(new Account(_cloudinarySettings.CloudName, _cloudinarySettings.Key, _cloudinarySettings.Secret));

            //Upload
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imagePath),
                UseFilename = true,
                Overwrite = true,
                UniqueFilename = false
            };

            return cloudinary.Upload(uploadParams);
        }
    }
}
