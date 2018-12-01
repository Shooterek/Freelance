using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels;
using Freelance.Infrastructure.ViewModels.Announcements;
using Freelance.Infrastructure.ViewModels.Jobs;
using Ninject.Activation;

namespace Freelance.Utilities
{
    public class AutoMapperFactory
    {
        public static IMapper AutoMapper(IContext context)
        {
            Mapper.Initialize(config =>
            {
                config.AllowNullCollections = true;

                config.CreateMap<Announcement, AnnouncementViewModel>();
                config.CreateMap<AnnouncementOffer, AnnouncementOfferViewModel>();
                config.CreateMap<ApplicationUser, ApplicationUserViewModel>()
                    .ForMember(dest => dest.ReceivedOpinionsAverage, opt => opt.Ignore());

                config.CreateMap<Job, JobViewModel>();
                config.CreateMap<JobOffer, JobOfferViewModel>();

            });

            return Mapper.Instance;
        }
    }
}