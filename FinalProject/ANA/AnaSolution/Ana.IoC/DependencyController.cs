using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using System.Web.Mvc;
using Ana.Contracts.Repository;
using Ana.Contracts.Business;
using Microsoft.WindowsAzure;
using Ana.Repository.Azure;
using Ana.Business.Managers;

namespace Ana.IoC
{
    public static class DependencyController
    {
        public static IContainer ConfigureForMVC()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                x.For<CloudStorageAccount>().Add(CloudStorageAccount.FromConfigurationSetting("DataConnectionString"));


                x.For<IUserRepository>().Use<UserRepository>();
                x.For<IUserBoardShareRepository>().Use<UserBoardShareRepository>();
                x.For<IBoardUserShareRepository>().Use<BoardUserShareRepository>();
                x.For<IBoardRepository>().Use<BoardRepository>();
                x.For<ICardRepository>().Use<CardRepository>();
                x.For<IUserNotificationRepository>().Use<UserNotificationRepository>();
                x.For<IDeveloperRepository>().Use<DeveloperRepository>();
                x.For<IGrantRepository>().Use<GrantRepository>();

                x.For<IUserManager>().HttpContextScoped().Use<UserManager>();
                x.For<IBoardManager>().HttpContextScoped().Use<BoardManager>();
                x.For<ICardManager>().HttpContextScoped().Use<CardManager>();
                x.For<INotificationManager>().HttpContextScoped().Use<NotificationManager>();
                x.For<IOAuth2Manager>().HttpContextScoped().Use<OAuth2Manager>();

                x.For<IEmailProvider>().HttpContextScoped().Use<GoogleEmailProvider>();
                x.For<IUserProvider>().HttpContextScoped().Use<HttpContextUserProvider>();

                x.For<IControllerActivator>().Use<StructureMapControllerActivator>();


            });

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
            
            return ObjectFactory.Container;
        }


        public static IContainer ConfigureForWorker()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                x.For<CloudStorageAccount>().Add(CloudStorageAccount.FromConfigurationSetting("DataConnectionString"));


                x.For<IUserRepository>().Use<UserRepository>();
                x.For<IUserBoardShareRepository>().Use<UserBoardShareRepository>();
                x.For<IBoardUserShareRepository>().Use<BoardUserShareRepository>();
                x.For<IBoardRepository>().Use<BoardRepository>();
                x.For<ICardRepository>().Use<CardRepository>();
                x.For<IUserNotificationRepository>().Use<UserNotificationRepository>();
                x.For<IDeveloperRepository>().Use<DeveloperRepository>();
                x.For<IGrantRepository>().Use<GrantRepository>();

                x.For<IUserManager>().Use<UserManager>();
                x.For<IBoardManager>().Use<BoardManager>();
                x.For<ICardManager>().Use<CardManager>();
                x.For<INotificationManager>().Use<NotificationManager>();

                x.For<IEmailProvider>().Use<GoogleEmailProvider>();
                x.For<IUserProvider>().Use<HttpContextUserProvider>();
            });

            return ObjectFactory.Container;
        }

    }
}
