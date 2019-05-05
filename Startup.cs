using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CfSample.Read.Locators;
using CfSample.Read.Models;
using CfSample.Read.Query;
using CfSample.Write.Commands;
using CfSample.Write.Events;
using EventFlow;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.Autofac.Extensions;
using EventFlow.Elasticsearch.Extensions;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using EventFlow.ReadStores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json.Linq;

namespace cf_sample_app_dotnet
{
    public class Startup
    {
		private readonly ILogger<Startup> _logger;
		
        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
			_logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var vcapServices = Configuration.GetValue<string>("VCAP_SERVICES");
            JObject obj = JObject.Parse(vcapServices);
            JToken token = obj["searchly"][0]["credentials"]["uri"];
			_logger.LogInformation($"Taking credentials {token.ToString()}");
            var connectionSettings = new ConnectionSettings(new Uri(token.ToString()));
            connectionSettings.DisablePing();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            var containerBuilder = new ContainerBuilder();
            var container = EventFlowOptions.New
            .UseAutofacContainerBuilder(containerBuilder)
            .AddQueryHandler<GetAllPurchasesQueryHandler, GetAllPurchasesQuery, IEnumerable<PurchaseViewModel>>()
            .AddEvents(typeof(PurchaseCreatedEvent))
            .AddCommands(typeof(CreatePurchaseCommand))
            .AddCommandHandlers(typeof(CreatePurchaseCommandHandler))
            .ConfigureElasticsearch(connectionSettings)
            .UseElasticsearchReadModel<PurchaseViewModel, PurchaseViewModelDateTypeLocator>()
            .RegisterServices(sr => sr.RegisterType(typeof(PurchaseViewModelDateTypeLocator)));
             containerBuilder.Populate(services);
            return new AutofacServiceProvider(containerBuilder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseMvcWithDefaultRoute();
            app.UseMiddleware<CommandPublishMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
