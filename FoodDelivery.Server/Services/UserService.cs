using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Google.Apis.Auth.OAuth2;

namespace FoodDelivery.Server.Services
{
    public class UserService {

        private readonly FirestoreDb _firestore;
        private readonly string _firebaseApiKey = "AIzaSyC1EHPs1TFl6tmmab3k_4187hH0ppLzh7I";

        public UserService() 
        {
            // load admin credentials
             
            var credential = GoogleCredential.FromFile(
            Path.Combine(AppContext.BaseDirectory, "firebase-adminsdk.json")
        );


        // firestore connection
        var channel = new FirestoreClientBuilder
        {
            Credential = credential
        }.Build();

        _firestore = FirestoreDb.Create("food-delivery-system-5e4c7", channel);
        }

        // create users in firebase and store details in firestore
        public async Task<string> RegisterUser(string name, string email, string password)
        { 
            UserRecordArgs args = new UserRecordArgs()
            {
                DisplayName = name,
                Email = email,
                Password = password
            };

            UserRecord newUser = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);

            var userDoc = _firestore.Collection("users").Document(newUser.Uid);

            await userDoc.SetAsync(new
            {
                uid = newUser.Uid,
                name = name,
                email = email,
                createdAt = Timestamp.GetCurrentTimestamp()
            });

            return newUser.Uid;
        }
        // login the registered user
        public async Task<LoginResponse?> LoginUser(string email, string password)
        {
            var loadUser = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            using var client = new HttpClient();

            //Firebase REST API endpoint for email /password login 
            var response = await client.PostAsJsonAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseApiKey}",
                loadUser
            );

            if(!response.IsSuccessStatusCode)
            {
                return null; //login failed
            }

            
            var firebaseData = await response.Content.ReadFromJsonAsync<FirebaseLoginResult>();

            return new LoginResponse
            {
                Token = firebaseData.idToken,
                RefreshToken = firebaseData.refreshToken,
                UserId = firebaseData.localId
            };
        }
    }

    public class FirebaseLoginResult
    {
        public string? idToken {get; set;}
        public string? refreshToken {get; set;}
        public string? localId {get; set;}
    }

    public class LoginResponse
    {
        public string? Token {get; set;} // backend uses this to authorize requests
        public string? RefreshToken {get; set;}  //user request a new token without the user login again and again
        public string? UserId {get; set;}  //unique firebase user ID

    }
   
}