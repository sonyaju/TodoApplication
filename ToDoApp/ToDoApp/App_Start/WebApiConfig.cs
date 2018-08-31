using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ToDoApp.Entities;
using Unity;
using Unity.Lifetime;
using Microsoft.Practices.Unity;
using System.Net.Http.Headers;
using WebApi2UnityDemo.Resolver;
using ToDoApp.Models;
using ToDoApp.WebApi_Exception;

namespace ToDoApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //registering custom exception
            config.Filters.Add(new TodoCustomExceptionFilterAttribute());

            //code to register and resolve injection using unity resolver
            var container = new UnityContainer();
            container.RegisterType<ITodoRepository, TodoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<DBEntities, DBEntities>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
