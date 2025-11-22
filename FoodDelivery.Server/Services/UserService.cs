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

        public UserService() 
        {
            var credential = GoogleCredential.FromFile(
            Path.Combine(AppContext.BaseDirectory, "firebase-adminsdk.json")
        );

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
    }

   
}