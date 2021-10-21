using B2B.Installers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace API.Installers
{
    public class FireBaseAdminInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var credential = Path.Combine(outPutDirectory, "gecko-b3c27-firebase-adminsdk-ifp2n-c7fbae9866.json");
            string credentialPath = new Uri(credential).LocalPath;
            //FirebaseApp.Create(new AppOptions
            //{
            //    Credential = GoogleCredential.FromFile(credentialPath)
            //});
        }
    }
}
