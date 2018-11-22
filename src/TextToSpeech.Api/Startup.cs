using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TextToSpeech.Api.Application.Rest;
using TextToSpeech.Api.Application.Services;

namespace TextToSpeech.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddHttpClient<CognitiveAuthRest>(
                c => {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("CognitiveTokenUrl"));
                    c.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Configuration.GetValue<string>("CognitiveApiKey"));
                });

            services.AddHttpClient<CognitiveRest>(
                c => {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("CognitiveUrl"));
                    c.DefaultRequestHeaders.Add("User-Agent", Configuration.GetValue<string>("User-Agent"));
                });
            
            services.AddTransient<ISpeechService, SpeechService>();
            services.AddTransient<IContainerService, ContainerService>();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
