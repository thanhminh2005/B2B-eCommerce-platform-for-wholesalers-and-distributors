using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class FirebaseCredential
    {
        public async Task<string> GetToken()
        {
            GoogleCredential credential;
            using (var stream = new System.IO.FileStream("gecko-b3c27-firebase-adminsdk-ifp2n-c7fbae9866.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(
                    new string[] {
                        "https://www.googleapis.com/auth/firebase.database",
                        "https://www.googleapis.com/auth/firebase.messaging",
                        "https://www.googleapis.com/auth/identitytoolkit",
                        "https://www.googleapis.com/auth/userinfo.email"}
                    );
            }

            ITokenAccess c = credential as ITokenAccess;
            return await c.GetAccessTokenForRequestAsync();
        }
    }
}
