﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using QueueStepchenko.Models;
using QueueStepchenko.Hubs;

namespace QueueStepchenko.Utils
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем контроллер в текущей сборке
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            

            // регистрируем споставление типов
            builder.RegisterType<OperationRepository>().As<IRepositoryOperation>().InstancePerRequest();
            builder.RegisterType<EmployeeRepository>().As<IRepositoryEmployee>().InstancePerRequest(); ;
            builder.RegisterType<UserRepository>().As<IRepositoryUser>().InstancePerRequest();
            builder.RegisterType<QueueRepository>().As<IRepositoryQueue>().InstancePerRequest();
            builder.RegisterType<SettingRepository>().As<IRepositorySetting>().InstancePerRequest();

            builder.RegisterInstance(new QueueHub()).As<IQueueHub>();

            // создаем новый контейнер с теми зависимостями, которые определены выше
            var container = builder.Build();

            // установка сопоставителя зависимостей
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}