using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ConsoleHttpClient
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void ShowEntities(IEnumerable<string> entities)
        {
            foreach (var entity in entities) 
            {
                Console.WriteLine(entity);
            }
        }

        static async Task<IEnumerable<string>?> GetAsync(string path)
        {
            // use var
            var response = await client.GetAsync(path);

            // please change it into tenatary condition when response is Success just return response.Content or null then you don't need this enntities variable            
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<IEnumerable<string>>() : null;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            string categoriesApiUri = "https://localhost:7140/api/CategoriesApi/GetCategories";
            string productsApiUri = "https://localhost:7140/api/ProductsApi/GetProducts";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var categories = await GetAsync(categoriesApiUri);

                if (categories != null)
                {
                    Console.WriteLine("\n Categories: \n");
                    ShowEntities(categories);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                var products = await GetAsync(productsApiUri);

                if (products != null)
                {
                    Console.WriteLine("\n Products: \n");
                    ShowEntities(products);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
