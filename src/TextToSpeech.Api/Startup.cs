using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using TextToSpeech.Api.Application.Options;
using TextToSpeech.Api.Application.Rest;
using TextToSpeech.Api.Application.Services;

namespace TextToSpeech.Api
{
    public class Startup
    {
        const string SwaggerName = "TextToSpeech.Api";
        const string SwaggerVersion = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.Configure<StorageOptions>(
                this.Configuration.GetSection("StorageOptions"));

            services.Configure<List<AcronymsOptions>>(
               this.Configuration.GetSection("AcronymsOptions"));

            services.Configure<List<RegexOptions>>(
              this.Configuration.GetSection("RegexOptions"));

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
            services.AddTransient<IReplaceService, AcronymsService>();
            services.AddTransient<IReplaceService, RegexService>();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new Info
                {
                    Title = SwaggerName,
                    Version = SwaggerVersion
                });
            });

            services.AddCors();
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

            app.UseCors(
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    $"/swagger/{SwaggerVersion}/swagger.json",
                    $"{SwaggerName} {SwaggerVersion}");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
