using Ejercicio.Utilities;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Microsoft.Web.Http.Versioning;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;
using System.Web.Mvc;

namespace Ejercicio.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web
            //config.EnableCors(); // new EnableCorsAttribute("*", "*", "*"));
            //Configurando autofac:
            AutofacWebapiConfig.Initialize(config);
            AreaRegistration.RegisterAllAreas();


            // Versionado del api en la url
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof( ApiVersionRouteConstraint )
                }
            };

            // Rutas de API web
            config.MapHttpAttributeRoutes(constraintResolver, new CustomDirectRouteProvider());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.AddApiVersioning(opt => {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionSelector = new ConstantApiVersionSelector(new ApiVersion(1, 0));
            });

            var apiExplorer = config.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VV";
                o.SubstituteApiVersionInUrl = true;
                o.SubstitutionFormat = "VV";
            });

            // Genera una propiedad en los HttpControllerDescriptor que contiene todas las versiones que filtra ese controlador
            var versiones = apiExplorer.ApiDescriptions.ToList()
                .SelectMany(apg => apg.ApiDescriptions)
                .Select(api => api.ActionDescriptor.ControllerDescriptor)
                .Distinct()
                .ToList();

            versiones.ForEach(cd =>
            {
                var apiVersionModel = cd.GetApiVersionModel();
                var apisSoportadas = apiVersionModel.DeclaredApiVersions.Select(p => "v" + p.ToString()).ToList();

                cd.Properties.TryAdd("versionesSoportadas", apisSoportadas);
            });
            config.EnableSwagger(elemento =>
            {
                elemento.MultipleApiVersions(
                   (apiDesc, targetApiVersion) => ResolveVersionSupportByRouteConstraint(apiDesc, targetApiVersion),
                   versionBuilder =>
                   {
                       versionBuilder.Version("v1_0", "Ejercicio v1.0");
                   });
                elemento.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                elemento.ApiKey("apiKey")
                    .Description("API Key Authentication")
                    .Name("Authorization")
                    .In("header");
                elemento.IncludeXmlComments(GetXmlCommentsPath());
            })
                 .EnableSwaggerUi(elemento => {
                     elemento.EnableDiscoveryUrlSelector();
                     elemento.EnableApiKeySupport("Authorization", "header");
                 });
            config.MessageHandlers.Add(new TokenValidationHandler());
        }

        private static string GetXmlCommentsPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + Utils.Setting<string>("RutaXMLDocumentacion");
        }

        private static bool ResolveVersionSupportByRouteConstraint(ApiDescription apiDesc, string targetApiVersion)
        {
            var apiVersionModel = apiDesc.GetGroupName().Replace('.', '_');
            return targetApiVersion == apiVersionModel;
        }
    }
    }
}
