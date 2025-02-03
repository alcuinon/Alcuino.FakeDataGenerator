using System.Linq.Expressions;
using Bogus;

namespace Alcuino.FakeDataGenerator
{
    /// <summary>
    /// Provides functionality for generating fake data using the Bogus library.
    /// This class includes methods to create realistic dummy data based on common property name patterns.
    /// </summary>
    public static class FakeDataGenerator
    {

        public static Config Config { get; set; } = new Config
        {
            Seed = 123,
            Locale = "en",
            MoneySymbol = "$"
        };

        /// <summary>
        /// Generates a list of fake data for the specified generic type <typeparamref name="T"/>.
        /// This method utilizes the Bogus library to create realistic dummy data based on property names.
        /// </summary>
        /// <typeparam name="T">The class type for which fake data will be generated.</typeparam>
        /// <param name="obj">A list of objects of type <typeparamref name="T"/> (not used directly in generation).</param>
        /// <param name="total">The number of fake data entries to generate. Defaults to 100.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> collection containing the generated fake data.</returns>
        /// <remarks>
        /// The method assigns values based on common property name patterns. 
        /// - "id" is assigned a sequential integer or a GUID.
        /// - "email", "phone", "address", etc., are assigned realistic data.
        /// - Names like "firstname" and "lastname" get corresponding fake names.
        /// - Numeric fields like "amount", "price", and "quantity" get relevant random values.
        /// - Boolean properties prefixed with "is" or "has" are randomly set to true/false.
        /// - Date-related properties receive past dates.
        /// </remarks>
        public static IEnumerable<T> GenerateFakeData<T>(this List<T> obj, int total = 100) where T : class, new()
        {
            Randomizer.Seed = new Random(Config.Seed);

            var faker = new Faker<T>(Config.Locale);

            foreach (var property in typeof(T).GetProperties())
            {
                Type propType = property.PropertyType;

                // Generate a parameter expression for the property
                var param = Expression.Parameter(typeof(T), "p");
                var propertyAccess = Expression.Property(param, property);
                var lambda = Expression.Lambda(propertyAccess, param);

                // Check the property value and apply the appropriate RuleFor
                var propertyValue = property.Name;
                var idscnt = 1;
                try
                {
                    if (propertyValue != null)
                    {
                        var propertyValueType = propertyValue.GetType();

                        var swval = propertyValue.ToString()?.ToLower();
                        switch (swval)
                        {
                            case "id":
                                if (propType == typeof(Guid))
                                    faker.RuleFor((Expression<Func<T, Guid>>)lambda, f => f.Random.Guid());
                                faker.RuleFor((Expression<Func<T, int>>)lambda, f => idscnt++);
                                break;
                            case string s when swval.EndsWith("id"):
                                faker.RuleFor((Expression<Func<T, int>>)lambda, f => f.Random.Number(1, 5));
                                break;
                            case "item":
                            case "product":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Commerce.Product());
                                break;
                            case "itemname":
                            case "productname":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Commerce.ProductName());
                                break;
                            case string s when swval.Contains("color"):
                                if (propType == typeof(string))
                                    faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Commerce.Color());
                                break;
                            case "qty":
                            case "quantity":
                                faker.RuleFor((Expression<Func<T, int>>)lambda, f => f.Random.Number(1, 10));
                                break;
                            case "amnt":
                            case "amount":
                                faker.RuleFor((Expression<Func<T, decimal>>)lambda, f => f.Finance.Amount(10, 1000, 2));
                                break;
                            case "price":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Commerce.Price(100, 1000, 2, Config.MoneySymbol));
                                break;
                            case "fullname":
                            case "name":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Name.FullName());
                                break;
                            case "username":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Internet.UserName());
                                break;
                            case "password":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Internet.Password());
                                break;
                            case "fname":
                            case "firstname":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Name.FirstName());
                                break;
                            case "lname":
                            case "lastname":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Name.LastName());
                                break;
                            case "email":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, (f, u) => f.Internet.Email());
                                break;
                            case "phone":
                            case "contactno":
                            case "cp":
                            case "contactnumber":
                            case "phonenumber":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Phone.PhoneNumber());
                                break;
                            case "address":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Address.FullAddress());
                                break;
                            case "streetaddress":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Address.StreetAddress());
                                break;
                            case "city":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Address.City());
                                break;
                            case "state":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Address.State());
                                break;
                            case "zipcode":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Address.ZipCode());
                                break;
                            case "country":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Address.Country());
                                break;
                            case "countrycode":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Address.CountryCode());
                                break;
                            case string s when swval.Contains("score"):
                                faker.RuleFor((Expression<Func<T, int>>)lambda, f => f.Random.Int(30, 50));
                                break;
                            case string s when swval.Contains("grade"):
                                faker.RuleFor((Expression<Func<T, int>>)lambda, f => f.Random.Int(65, 100));
                                break;
                            case "body":
                            case "description":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Lorem.Paragraphs());
                                break;
                            case "title":
                                faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Lorem.Word());
                                break;
                            case string s1 when swval.StartsWith("has"):
                            case string s2 when swval.StartsWith("is"):
                                if (propType == typeof(bool))
                                    faker.RuleFor((Expression<Func<T, bool>>)lambda, f => f.Random.Bool());
                                break;
                            case "gender":
                                if (propType == typeof(int))
                                    faker.RuleFor((Expression<Func<T, int>>)lambda, f => f.Random.Number(1, 2));
                                break;
                            case string s when swval.Contains("date"):
                                faker.RuleFor((Expression<Func<T, DateTime?>>)lambda, f => f.Date.Past());
                                break;
                            default:
                                if (propType == typeof(string))
                                    faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Lorem.Word());

                                else if (propType == typeof(int))
                                    faker.RuleFor((Expression<Func<T, int>>)lambda, f => f.Random.Int(1, 100));

                                else if (propType == typeof(long))
                                    faker.RuleFor((Expression<Func<T, long>>)lambda, f => f.Random.Long(1, 10000));

                                else if (propType == typeof(short))
                                    faker.RuleFor((Expression<Func<T, short>>)lambda, f => (short)f.Random.Int(1, 100));

                                else if (propType == typeof(float))
                                    faker.RuleFor((Expression<Func<T, float>>)lambda, f => (float)f.Random.Double(1, 100));

                                else if (propType == typeof(double))
                                    faker.RuleFor((Expression<Func<T, double>>)lambda, f => f.Random.Double(1, 100));

                                else if (propType == typeof(decimal))
                                    faker.RuleFor((Expression<Func<T, decimal>>)lambda, f => (decimal)f.Random.Double(1, 100));

                                else if (propType == typeof(bool))
                                    faker.RuleFor((Expression<Func<T, bool>>)lambda, f => f.Random.Bool());

                                else if (propType == typeof(DateTime))
                                    faker.RuleFor((Expression<Func<T, DateTime>>)lambda, f => f.Date.Past());

                                else if (propType == typeof(Guid))
                                    faker.RuleFor((Expression<Func<T, Guid>>)lambda, f => f.Random.Guid());

                                else if (propType == typeof(Uri))
                                    faker.RuleFor((Expression<Func<T, Uri>>)lambda, f => new Uri(f.Internet.Url()));

                                else if (propType == typeof(TimeSpan))
                                    faker.RuleFor((Expression<Func<T, TimeSpan>>)lambda, f => TimeSpan.FromMinutes(f.Random.Int(1, 500)));

                                else if (Nullable.GetUnderlyingType(propType) != null)
                                {
                                    Type underlyingType = Nullable.GetUnderlyingType(propType);
                                    if (underlyingType == typeof(int))
                                        faker.RuleFor((Expression<Func<T, int?>>)lambda, f => f.Random.Int(1, 100));

                                    else if (underlyingType == typeof(double))
                                        faker.RuleFor((Expression<Func<T, double?>>)lambda, f => f.Random.Double(1, 100));

                                    else if (underlyingType == typeof(bool))
                                        faker.RuleFor((Expression<Func<T, bool?>>)lambda, f => f.Random.Bool());
                                }
                                break;
                        }
                    }
                }
                catch
                {
                    // Apply RuleFor() dynamically using expressions (not compiled lambdas)
                    if (propType == typeof(string))
                        faker.RuleFor((Expression<Func<T, string>>)lambda, f => f.Lorem.Word());

                    else if (propType == typeof(int))
                        faker.RuleFor((Expression<Func<T, int>>)lambda, f => f.Random.Int(1, 100));

                    else if (propType == typeof(long))
                        faker.RuleFor((Expression<Func<T, long>>)lambda, f => f.Random.Long(1, 10000));

                    else if (propType == typeof(short))
                        faker.RuleFor((Expression<Func<T, short>>)lambda, f => (short)f.Random.Int(1, 100));

                    else if (propType == typeof(float))
                        faker.RuleFor((Expression<Func<T, float>>)lambda, f => (float)f.Random.Double(1, 100));

                    else if (propType == typeof(double))
                        faker.RuleFor((Expression<Func<T, double>>)lambda, f => f.Random.Double(1, 100));

                    else if (propType == typeof(decimal))
                        faker.RuleFor((Expression<Func<T, decimal>>)lambda, f => (decimal)f.Random.Double(1, 100));

                    else if (propType == typeof(bool))
                        faker.RuleFor((Expression<Func<T, bool>>)lambda, f => f.Random.Bool());

                    else if (propType == typeof(DateTime))
                        faker.RuleFor((Expression<Func<T, DateTime>>)lambda, f => f.Date.Past());

                    else if (propType == typeof(Guid))
                        faker.RuleFor((Expression<Func<T, Guid>>)lambda, f => f.Random.Guid());

                    else if (propType == typeof(Uri))
                        faker.RuleFor((Expression<Func<T, Uri>>)lambda, f => new Uri(f.Internet.Url()));

                    else if (propType == typeof(TimeSpan))
                        faker.RuleFor((Expression<Func<T, TimeSpan>>)lambda, f => TimeSpan.FromMinutes(f.Random.Int(1, 500)));

                    // Handle Nullable types (int?, double?, bool?, etc.)
                    else if (Nullable.GetUnderlyingType(propType) != null)
                    {
                        Type underlyingType = Nullable.GetUnderlyingType(propType);
                        if (underlyingType == typeof(int))
                            faker.RuleFor((Expression<Func<T, int?>>)lambda, f => f.Random.Int(1, 100));

                        else if (underlyingType == typeof(double))
                            faker.RuleFor((Expression<Func<T, double?>>)lambda, f => f.Random.Double(1, 100));

                        else if (underlyingType == typeof(bool))
                            faker.RuleFor((Expression<Func<T, bool?>>)lambda, f => f.Random.Bool());
                    }
                }
            }

            return faker.GenerateForever().Take(total);
        }

    }

    public class Config
    {
        public int Seed { get; set; }
        public string Locale { get; set; } = null!;
        public string MoneySymbol { get; set; } = null!;
    }
}
